using Connected.Annotations.Entities;
using Connected.Entities;

namespace Connected.Notifications.Push.Subscriptions;

[EntityKey(NotificationsMetaData.SubscriptionKey)]
public interface ISubscription
	: IEntity<int>
{
	string AuthenticationToken { get; init; }
	string Endpoint { get; init; }
	string P256dh { get; init; }
	string Auth { get; init; }

	/// <summary>
	/// Which VAPID key pair (<see cref="Connected.Notifications.Push.Configuration.VapidOptions.KeyId"/>) this
	/// subscription was created under. Empty when the tenant has never set one.
	/// </summary>
	string KeyId { get; init; }

	DateTimeOffset Created { get; init; }
}
