using Connected.Entities;
using Connected.Services;

namespace Connected.Notifications.Push.Preferences.Ops;

internal sealed class SelectByToken(INotificationPreferenceCache cache)
	: ServiceFunction<IValueDto<string>, INotificationPreference?>
{
	protected override async Task<INotificationPreference?> OnInvoke()
	{
		return await cache.AsEntity(f => f.AuthenticationToken == Dto.Value);
	}
}
