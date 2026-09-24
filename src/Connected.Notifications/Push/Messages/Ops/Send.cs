using Connected.Notifications.Push.Configuration;
using Connected.Notifications.Push.Messages.Dtos;
using Connected.Notifications.Push.Preferences;
using Connected.Notifications.Push.Subscriptions;
using Connected.Services;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Connected.Notifications.Push.Messages.Ops;

/// <remarks>
/// Takes the queue context by its concrete type: discovery registers a queue context under the class itself,
/// not under <c>IQueueContext&lt;,&gt;</c>, and asking for the interface leaves the service graph
/// unconstructible at startup.
/// </remarks>
internal sealed class Send(INotificationPreferenceService preferences, ISubscriptionService subscriptions,
	PushMessageDeliveryQueueContext queue, IOptionsMonitor<PushDeliveryOptions> delivery)
	: ServiceAction<ISendPushMessageDto>
{
	private const int MaxQueuedPayloadBytes = 1024;

	protected override async Task OnInvoke()
	{
		/*
		 * The kill switch. Checked before anything else so a disabled instance does no work at all - no
		 * cache reads, nothing queued - rather than queuing deliveries that would then need to be found and
		 * cancelled.
		 */
		if (!delivery.CurrentValue.Enabled)
			return;

		var identity = DtoFactory.Create<IValueDto<string>>(f => f.Value = Dto.AuthenticationToken);

		// No preference row means the user never turned notifications on, which counts as off. 
		if (await preferences.Select(identity) is not { Enabled: true })
			return;

		var targets = await subscriptions.Query(identity);

		if (targets.Count == 0)
			return;

		/*
		 * One queue message per device, so a device that fails is retried on its own without sending the
		 * notification again to the devices that already received it. All are built and measured before any
		 * is queued, so a notification that is too large is rejected as a whole rather than half sent.
		 */
		var deliveries = targets.Select(f => new DeliverPushMessageDto
		{
			Subscription = f.Id,
			Title = Dto.Title,
			Body = Dto.Body,
			Url = Dto.Url
		}).ToList();

		foreach (var delivery in deliveries)
		{
			var size = JsonSerializer.SerializeToUtf8Bytes(delivery, delivery.GetType(), JsonSerializerOptions.Default).Length;

			if (size > MaxQueuedPayloadBytes)
				throw new InvalidOperationException($"The push notification is too large to queue ({size} of {MaxQueuedPayloadBytes} bytes). Shorten the title or body.");
		}

		foreach (var delivery in deliveries)
			await queue.Invoke(delivery);
	}
}
