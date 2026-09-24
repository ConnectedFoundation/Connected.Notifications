using Connected.Services;

namespace Connected.Notifications.Push.Preferences.Dtos;

internal abstract class NotificationPreferenceDto
	: Dto, INotificationPreferenceDto
{
	public bool Enabled { get; set; }
}
