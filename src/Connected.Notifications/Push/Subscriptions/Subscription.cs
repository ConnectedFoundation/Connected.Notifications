using Connected.Annotations;
using Connected.Annotations.Entities;
using Connected.Entities;

namespace Connected.Notifications.Push.Subscriptions;

[Table(NotificationsMetaData.Schema)]
internal sealed record Subscription
	: ConsistentEntity<int>, ISubscription
{
	[Ordinal(0), Length(128), Index(false)]
	public required string AuthenticationToken { get; init; }

	[Ordinal(1), Length(-1)]
	public required string Endpoint { get; init; }

	// SHA-256 hex digest of Endpoint - fixed length, indexable - so two rows can never carry the same endpoint
	[Ordinal(2), Length(64), Index(true)]
	public required string EndpointHash { get; init; }

	[Ordinal(3), Length(256)]
	public required string P256dh { get; init; }

	[Ordinal(4), Length(256)]
	public required string Auth { get; init; }

	// Every subscription made while a given key pair is active shares its id.
	[Ordinal(5), Length(64), Index(false)]
	public required string KeyId { get; init; }

	[Ordinal(6), Date(DateKind.DateTime)]
	public DateTimeOffset Created { get; init; }
}
