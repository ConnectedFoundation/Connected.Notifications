using Connected.Notifications.Push.Preferences.Dtos;
using Connected.Notifications.Push.Preferences.Ops;
using Connected.Services;

namespace Connected.Notifications.Push.Preferences;

internal sealed class NotificationPreferenceService(IServiceProvider services)
	: Service(services), INotificationPreferenceService
{
	public async Task Update(IUpdateNotificationPreferenceDto dto) => await Invoke(GetOperation<Update>(), dto);

	public async Task<INotificationPreference?> Select() => await Invoke(GetOperation<Select>(), Dto.Empty);

	public async Task<INotificationPreference?> Select(IValueDto<string> dto) => await Invoke(GetOperation<SelectByToken>(), dto);
}
