using Connected.Entities;
using Connected.Notifications.Push.Subscriptions.Dtos;
using Connected.Services;
using Connected.Storage;

namespace Connected.Notifications.Push.Subscriptions.Ops;

internal sealed class Insert(IStorageProvider storage, ISubscriptionService subscriptions, IEventService events, ISubscriptionCache cache, IInsertSubscriptionAmbient ambient)
	: ServiceFunction<IInsertSubscriptionDto, int>
{
	protected override async Task<int> OnInvoke()
	{
		var entity = await storage.Open<Subscription>().Update(Dto.AsEntity<Subscription>(State.Add, ambient))
			?? throw new NullReferenceException("Expected an entity.");

		SetState(entity);

		await cache.Refresh(entity.Id);
		await events.Inserted(this, subscriptions, entity.Id);

		return entity.Id;
	}
}
