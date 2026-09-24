using Connected.Caching;
using Connected.Storage;

namespace Connected.Notifications.Push.Subscriptions;

internal sealed class SubscriptionCache(ICachingService cache, IStorageProvider storage)
	: EntityCache<ISubscription, Subscription, int>(cache, storage, NotificationsMetaData.SubscriptionKey), ISubscriptionCache
{
}
