using Connected.Annotations;
using Connected.Annotations.Entities;
using Connected.Entities;

namespace Connected.Notifications.Push.Preferences;

[Table(NotificationsMetaData.Schema)]
internal sealed record NotificationPreference
	: ConsistentEntity<int>, INotificationPreference
{
	[Ordinal(0), Length(128), Index(true)]
	public required string AuthenticationToken { get; init; }

	[Ordinal(1)]
	public bool Enabled { get; init; }
}
