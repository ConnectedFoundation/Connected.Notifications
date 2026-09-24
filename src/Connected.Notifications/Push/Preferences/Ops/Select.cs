using Connected.Authentication;
using Connected.Entities;
using Connected.Services;

namespace Connected.Notifications.Push.Preferences.Ops;

internal sealed class Select(IAuthenticationService authentication, INotificationPreferenceCache cache)
	: ServiceFunction<IDto, INotificationPreference?>
{
	protected override async Task<INotificationPreference?> OnInvoke()
	{
		var token = (await authentication.SelectIdentity())?.Token
			?? throw new InvalidOperationException("An authenticated identity is required to read the notification preference.");

		return await cache.AsEntity(f => f.AuthenticationToken == token);
	}
}
