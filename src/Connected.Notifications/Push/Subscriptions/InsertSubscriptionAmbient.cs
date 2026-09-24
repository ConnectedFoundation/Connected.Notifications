using Connected.Authentication;
using Connected.Notifications.Push.Configuration;
using Connected.Notifications.Push.Subscriptions.Dtos;
using Connected.Services;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using System.Text;

namespace Connected.Notifications.Push.Subscriptions;

/// <remarks>
/// The subscribing user is resolved here, from the ambient identity of the request, rather than
/// trusted from the DTO - a caller must not be able to register a subscription under someone else's
/// identity by simply putting a different token on the wire.
/// </remarks>
internal sealed class InsertSubscriptionAmbient(IAuthenticationService authentication, IOptionsMonitor<VapidOptions> vapid)
	: AmbientProvider<IInsertSubscriptionDto>, IInsertSubscriptionAmbient
{
	public string AuthenticationToken { get; set; } = string.Empty;

	public string EndpointHash { get; set; } = string.Empty;

	public string KeyId { get; set; } = string.Empty;

	public DateTimeOffset Created { get; set; }

	protected override async Task OnInvoke()
	{
		AuthenticationToken = (await authentication.SelectIdentity())?.Token
			?? throw new InvalidOperationException("An authenticated identity is required to subscribe to push notifications.");

		EndpointHash = Convert.ToHexStringLower(SHA256.HashData(Encoding.UTF8.GetBytes(Dto.Endpoint)));
		KeyId = vapid.CurrentValue.KeyId ?? string.Empty;
		Created = DateTimeOffset.UtcNow;
	}
}
