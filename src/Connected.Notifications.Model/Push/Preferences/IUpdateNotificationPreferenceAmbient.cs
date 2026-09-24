using Connected.Notifications.Push.Preferences.Dtos;
using Connected.Services;

namespace Connected.Notifications.Push.Preferences;

public interface IUpdateNotificationPreferenceAmbient
	: IAmbientProvider<IUpdateNotificationPreferenceDto>
{
	string AuthenticationToken { get; set; }
}
