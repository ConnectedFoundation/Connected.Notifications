using Connected.Services;
using System.Collections.Immutable;

namespace Connected.Notifications.Push.Subscriptions.Ops;

/// <remarks>
/// Built directly from the cache's full set rather than a predicate-based cache query, since only
/// <c>Get(id)</c>/<c>Get(predicate)</c> (single result) and <c>All()</c> are confirmed cache primitives -
/// this stays on ground that's verified rather than assuming an unverified multi-result predicate overload.
/// </remarks>
internal sealed class Query(ISubscriptionCache cache)
	: ServiceFunction<IValueDto<string>, IImmutableList<ISubscription>>
{
	protected override async Task<IImmutableList<ISubscription>> OnInvoke()
	{
		var all = await cache.All();

		return all.Where(f => f.AuthenticationToken == Dto.Value).ToImmutableList();
	}
}
