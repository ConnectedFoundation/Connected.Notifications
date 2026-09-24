using Connected.Entities;
using Connected.Services;
using Connected.Storage;

namespace Connected.Notifications.Push.Subscriptions.Ops;

internal sealed class Delete(IStorageProvider storage, ISubscriptionService subscriptions, IEventService events, ISubscriptionCache cache)
	: ServiceAction<IPrimaryKeyDto<int>>
{
	protected override async Task OnInvoke()
	{
		var entity = SetState(await subscriptions.Select(Dto)) as Subscription
			?? throw new NullReferenceException("Expected an entity.");

		await storage.Open<Subscription>().Update(entity.Merge(Dto, State.Delete));
		await cache.Remove(Dto.Id);
		await events.Deleted(this, subscriptions, Dto.Id);
	}
}
