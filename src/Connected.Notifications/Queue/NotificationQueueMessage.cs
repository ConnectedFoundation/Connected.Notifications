using Connected.Annotations.Entities;
using Connected.Collections.Queues;

namespace Connected.Notifications.Queue;

[Table(NotificationsMetaData.Schema)]
internal sealed record NotificationQueueMessage
	: QueueMessage
{
}
