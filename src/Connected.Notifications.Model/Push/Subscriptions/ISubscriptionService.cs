using Connected.Annotations;
using Connected.Entities;
using Connected.Notifications.Push.Subscriptions.Dtos;
using Connected.Services;
using System.Collections.Immutable;

namespace Connected.Notifications.Push.Subscriptions;

/// <remarks>
/// Only operations that act on the caller's own subscriptions carry <c>[ServiceOperation]</c> and are published
/// as HTTP routes. The ones taking an arbitrary id or identity token are for backend code only - published, they
/// would let any signed-in user read or delete somebody else's subscriptions.
/// </remarks>
[Service, ServiceUrl(NotificationsUrls.SubscriptionService)]
public interface ISubscriptionService
{
	[ServiceOperation(ServiceOperationVerbs.Post)]
	Task<int> Insert(IInsertSubscriptionDto dto);

	/// <summary>
	/// Removes the caller's own subscription for a browser endpoint.
	/// </summary>
	[ServiceOperation(ServiceOperationVerbs.Delete)]
	Task Delete(IDeleteSubscriptionDto dto);

	/// <summary>
	/// The VAPID public key a browser subscription has to be created with.
	/// </summary>
	[ServiceOperation(ServiceOperationVerbs.Get)]
	Task<string> SelectPublicKey();

	Task Delete(IPrimaryKeyDto<int> dto);

	Task<ISubscription?> Select(IPrimaryKeyDto<int> dto);

	Task<IImmutableList<ISubscription>> Query(IValueDto<string> dto);
}
