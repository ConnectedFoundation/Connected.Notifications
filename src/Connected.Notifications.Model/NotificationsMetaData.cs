using Connected.Notifications.Push.Preferences;
using Connected.Notifications.Push.Subscriptions;

namespace Connected.Notifications;

public static class NotificationsMetaData
{
	public const string Schema = "notifications";

	public const string SubscriptionKey = $"{Schema}.{nameof(ISubscription)}";
	public const string NotificationPreferenceKey = $"{Schema}.{nameof(INotificationPreference)}";
}
