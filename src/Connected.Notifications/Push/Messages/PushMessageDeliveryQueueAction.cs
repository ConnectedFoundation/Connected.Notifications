using Connected.Collections.Queues;
using Connected.Notifications.Push.Configuration;
using Connected.Notifications.Push.Messages.Dtos;
using Connected.Notifications.Push.Subscriptions;
using Connected.Services;
using Lib.Net.Http.WebPush;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace Connected.Notifications.Push.Messages;

/// <summary>
/// Delivers one push notification to one device.
/// </summary>
/// <remarks>
/// Failures split by what retrying could achieve.
/// </remarks>
internal sealed class PushMessageDeliveryQueueAction(PushServiceClient client, ISubscriptionService subscriptions,
	IOptionsMonitor<VapidOptions> vapid, ILogger<PushMessageDeliveryQueueAction> logger)
	: QueueAction<IDeliverPushMessageDto>
{
	private static readonly JsonSerializerOptions PayloadOptions = new()
	{
		Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
	};

	protected override async Task OnInvoke()
	{
		var subscription = await subscriptions.Select(DtoFactory.Create<IPrimaryKeyDto<int>>(f => f.Id = Dto.Subscription));

		// The device unsubscribed after this was queued.
		if (subscription is null)
			return;

		var target = new PushSubscription { Endpoint = subscription.Endpoint };

		target.SetKey(PushEncryptionKeyName.P256DH, subscription.P256dh);
		target.SetKey(PushEncryptionKeyName.Auth, subscription.Auth);

		/*
		 * The tag is the queue message id, which stays the same across its retries. It lets the service worker
		 * replace rather than stack a notification delivered twice (sent, but the queue message failed to complete).
		 */
		var payload = JsonSerializer.Serialize(new
		{
			tag = Message.Id.ToString(),
			title = Dto.Title,
			body = Dto.Body,
			url = Dto.Url
		}, PayloadOptions);

		try
		{
			await client.RequestPushMessageDeliveryAsync(target, new PushMessage(payload)
			{
				TimeToLive = Convert.ToInt32(PushMessageDeliveryQueueContext.Lifetime.TotalSeconds)
			}, Cancel);
		}
		catch (PushServiceClientException ex) when (ex.StatusCode is HttpStatusCode.NotFound or HttpStatusCode.Gone)
		{
			logger.LogInformation("Subscription {Subscription} expired at the push service ({StatusCode}) and was removed.",
				subscription.Id, (int)ex.StatusCode);

			await subscriptions.Delete(DtoFactory.Create<IPrimaryKeyDto<int>>(f => f.Id = subscription.Id));
		}
		catch (PushServiceClientException ex) when (ex.StatusCode is HttpStatusCode.Unauthorized or HttpStatusCode.Forbidden)
		{
			var currentKeyId = vapid.CurrentValue.KeyId ?? string.Empty;

			/*
			 * An empty currentKeyId means rotations aren't tracked (nothing to compare to), so this branch
			 * only fires once a KeyId has actually been set and this row predates it.
			 */
			if (!string.IsNullOrEmpty(currentKeyId) && !string.Equals(subscription.KeyId, currentKeyId, StringComparison.Ordinal))
			{
				logger.LogInformation("Subscription {Subscription} was signed under a retired VAPID key ({SubscriptionKeyId}, current is {CurrentKeyId}) and was removed.",
					subscription.Id, subscription.KeyId, currentKeyId);

				await subscriptions.Delete(DtoFactory.Create<IPrimaryKeyDto<int>>(f => f.Id = subscription.Id));

				return;
			}

			logger.LogCritical("The push service rejected the VAPID identity ({StatusCode}) delivering to subscription {Subscription}. Check notifications:push:vapid. {Body}",
				(int)ex.StatusCode, subscription.Id, ex.Body);
		}
		catch (PushServiceClientException ex) when (ex.StatusCode is HttpStatusCode.BadRequest or HttpStatusCode.RequestEntityTooLarge)
		{
			logger.LogError("The push service refused the notification for subscription {Subscription} ({StatusCode}). {Body}",
				subscription.Id, (int)ex.StatusCode, ex.Body);
		}
	}
}
