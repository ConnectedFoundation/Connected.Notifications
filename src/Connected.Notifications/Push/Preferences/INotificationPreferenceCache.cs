using Connected.Caching;

namespace Connected.Notifications.Push.Preferences;

internal interface INotificationPreferenceCache
	: IEntityCache<INotificationPreference, int>
{
}
