using Connected.Entities;
using Connected.Services;

namespace Connected.Notifications.Push.Subscriptions.Ops;

internal sealed class Select(ISubscriptionCache cache)
	: ServiceFunction<IPrimaryKeyDto<int>, ISubscription?>
{
	protected override async Task<ISubscription?> OnInvoke()
	{
		return await cache.AsEntity(f => f.Id == Dto.Id);
	}
}
