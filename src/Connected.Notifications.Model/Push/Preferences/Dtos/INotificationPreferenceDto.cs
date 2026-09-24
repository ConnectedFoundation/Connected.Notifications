using Connected.Services;

namespace Connected.Notifications.Push.Preferences.Dtos;

public interface INotificationPreferenceDto
	: IDto
{
	bool Enabled { get; set; }
}
