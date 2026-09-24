using Connected.Collections.Queues;

namespace Connected.Notifications.Queue;

internal sealed class NotificationQueueMessageHost
	: QueueHost<NotificationQueueMessage, INotificationQueueMessageCache>
{
}
