using Connected.Notifications.Push.Configuration;
using Connected.Notifications.Push.Messages.Dtos;
using Connected.Notifications.Push.Preferences;
using Connected.Notifications.Push.Subscriptions;
using Connected.Services;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Connected.Notifications.Push.Messages.Ops;

/// <remarks>
/// The identity-scoped <see cref="Send"/> resolves one token through the services; there is no service
/// query for "every enabled identity", so this reads both caches directly and joins them here. No topic or
/// audience narrowing exists yet - every identity with notifications enabled receives the broadcast.
/// </remarks>
internal sealed class SendBroadcast(INotificationPreferenceCache preferences, ISubscriptionCache subscriptions,
	PushMessageDeliveryQueueContext queue, IOptionsMonitor<PushDeliveryOptions> delivery)
	: ServiceAction<ISendBroadcastPushMessageDto>
{
	private const int MaxQueuedPayloadBytes = 1024;

	protected override async Task OnInvoke()
	{
		if (!delivery.CurrentValue.Enabled)
			return;

		var enabled = (await preferences.All()).Where(f => f.Enabled)
			.Select(f => f.AuthenticationToken)
			.ToHashSet(StringComparer.Ordinal);

		if (enabled.Count == 0)
			return;

		var targets = (await subscriptions.All()).Where(f => enabled.Contains(f.AuthenticationToken)).ToList();

		if (targets.Count == 0)
			return;

		var deliveries = targets.Select(f => new DeliverPushMessageDto
		{
			Subscription = f.Id,
			Title = Dto.Title,
			Body = Dto.Body,
			Url = Dto.Url,
			MessageKey = Dto.MessageKey
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
