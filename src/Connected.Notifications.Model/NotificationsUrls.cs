namespace Connected.Notifications;

public static class NotificationsUrls
{
	public const string Namespace = "services/notifications";

	public const string SubscriptionService = $"{Namespace}/push/subscriptions";
	public const string NotificationPreferenceService = $"{Namespace}/push/preferences";
	public const string PushMessageService = $"{Namespace}/push/messages";
}
