using Connected.Collections.Queues;
using Connected.Notifications.Push.Messages.Dtos;
using Connected.Notifications.Queue;
using Connected.Storage;

namespace Connected.Notifications.Push.Messages;

/// <remarks>
/// The queue message is the only place a notification is kept: it is deleted when delivery succeeds, or by
/// the queue after <c>MaxDequeueCount</c> (10) failed tries. There is no inbox or history.
/// Never debounced (the DTO has no primary key, so no group): two notifications to the same device in the
/// same minute are two notifications.
/// </remarks>
internal sealed class PushMessageDeliveryQueueContext(IStorageProvider storage, INotificationQueueMessageCache cache)
	: QueueContext<NotificationQueueMessage, PushMessageDeliveryQueueAction, IDeliverPushMessageDto>(storage, cache)
{
	/// <summary>
	/// How long a notification is worth delivering. Used both for the queue message and for how long the
	/// push service keeps it for a device that is offline.
	/// </summary>
	public static readonly TimeSpan Lifetime = TimeSpan.FromDays(1);

	protected override async Task OnInitialize()
	{
		Expire = DateTimeOffset.UtcNow.Add(Lifetime);

		await base.OnInitialize();
	}
}
