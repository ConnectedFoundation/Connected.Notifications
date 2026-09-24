using Connected.Annotations.Entities;
using Connected.Entities;

namespace Connected.Notifications.Push.Preferences;

[EntityKey(NotificationsMetaData.NotificationPreferenceKey)]
public interface INotificationPreference
	: IEntity<int>
{
	string AuthenticationToken { get; init; }
	bool Enabled { get; init; }
}
