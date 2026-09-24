using Connected.Caching;
using Connected.Storage;

namespace Connected.Notifications.Push.Preferences;

internal sealed class NotificationPreferenceCache(ICachingService cache, IStorageProvider storage)
	: EntityCache<INotificationPreference, NotificationPreference, int>(cache, storage, NotificationsMetaData.NotificationPreferenceKey), INotificationPreferenceCache
{
}
