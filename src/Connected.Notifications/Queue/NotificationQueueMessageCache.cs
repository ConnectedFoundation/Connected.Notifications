using Connected.Caching;
using Connected.Collections.Queues;
using Connected.Storage;

namespace Connected.Notifications.Queue;

internal sealed class NotificationQueueMessageCache(ICachingService cache, IStorageProvider storage)
	: QueueMessageCache<NotificationQueueMessage>(cache, storage, $"{NotificationsMetaData.Schema}.{nameof(NotificationQueueMessage)}"), INotificationQueueMessageCache
{
}
