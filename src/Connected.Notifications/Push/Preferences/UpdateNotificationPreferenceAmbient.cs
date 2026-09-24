using Connected.Authentication;
using Connected.Notifications.Push.Preferences.Dtos;
using Connected.Services;

namespace Connected.Notifications.Push.Preferences;

/// <remarks>
/// Same reasoning as <see cref="Subscriptions.InsertSubscriptionAmbient"/>: whose preference this is
/// comes from the ambient identity of the request, never from the DTO.
/// </remarks>
internal sealed class UpdateNotificationPreferenceAmbient(IAuthenticationService authentication)
	: AmbientProvider<IUpdateNotificationPreferenceDto>, IUpdateNotificationPreferenceAmbient
{
	public string AuthenticationToken { get; set; } = string.Empty;

	protected override async Task OnInvoke()
	{
		AuthenticationToken = (await authentication.SelectIdentity())?.Token
			?? throw new InvalidOperationException("An authenticated identity is required to set the notification preference.");
	}
}
