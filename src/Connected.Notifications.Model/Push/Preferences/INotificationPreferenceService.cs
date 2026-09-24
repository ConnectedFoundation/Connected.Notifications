using Connected.Annotations;
using Connected.Notifications.Push.Preferences.Dtos;
using Connected.Services;

namespace Connected.Notifications.Push.Preferences;

/// <remarks>
/// Only the caller's own preference is published as an HTTP route. Reading by identity token is for backend
/// code only, the same as on <see cref="Subscriptions.ISubscriptionService"/>.
/// </remarks>
[Service, ServiceUrl(NotificationsUrls.NotificationPreferenceService)]
public interface INotificationPreferenceService
{
	[ServiceOperation(ServiceOperationVerbs.Put)]
	Task Update(IUpdateNotificationPreferenceDto dto);

	/// <summary>
	/// The caller's own preference, or <see langword="null"/> when they never set one (which counts as off).
	/// </summary>
	[ServiceOperation(ServiceOperationVerbs.Get)]
	Task<INotificationPreference?> Select();

	Task<INotificationPreference?> Select(IValueDto<string> dto);
}
