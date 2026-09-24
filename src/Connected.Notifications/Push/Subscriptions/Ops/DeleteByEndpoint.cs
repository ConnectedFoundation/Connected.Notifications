using Connected.Authentication;
using Connected.Entities;
using Connected.Notifications.Push.Subscriptions.Dtos;
using Connected.Services;

namespace Connected.Notifications.Push.Subscriptions.Ops;

/// <remarks>
/// Only ever removes subscriptions of the calling identity, so an endpoint belonging to somebody else is simply
/// not found. Matches on Endpoint rather than the primary key, since that's all the frontend has on hand when
/// it deletes before it (re)subscribes. Loops rather than assuming a single row only as a leftover safety net
/// for rows inserted before EndpointHash made the endpoint unique - going forward there's at most one match.
/// </remarks>
internal sealed class DeleteByEndpoint(IAuthenticationService authentication, ISubscriptionService subscriptions, ISubscriptionCache cache)
	: ServiceAction<IDeleteSubscriptionDto>
{
	protected override async Task OnInvoke()
	{
		var token = (await authentication.SelectIdentity())?.Token
			?? throw new InvalidOperationException("An authenticated identity is required to unsubscribe from push notifications.");

		var all = await cache.All();

		foreach (var subscription in all.Where(f => f.AuthenticationToken == token && f.Endpoint == Dto.Endpoint).ToList())
			await subscriptions.Delete(DtoFactory.Create<IPrimaryKeyDto<int>>(f => f.Id = subscription.Id));
	}
}
