using Connected.Caching;

namespace Connected.Notifications.Push.Subscriptions;

internal interface ISubscriptionCache
	: IEntityCache<ISubscription, int>
{
}
