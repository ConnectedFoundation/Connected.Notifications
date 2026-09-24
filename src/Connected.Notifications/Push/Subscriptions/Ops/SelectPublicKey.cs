using Connected.Notifications.Push.Configuration;
using Connected.Services;
using Microsoft.Extensions.Options;

namespace Connected.Notifications.Push.Subscriptions.Ops;

/// <remarks>
/// The browser reads the key from here rather than from its own configuration, so each tenant's key is set in
/// one place only. A browser subscribed with a different key than the one the server signs with is refused by
/// the push service.
/// </remarks>
internal sealed class SelectPublicKey(IOptionsMonitor<VapidOptions> vapid)
	: ServiceFunction<IDto, string>
{
	protected override async Task<string> OnInvoke()
	{
		var publicKey = vapid.CurrentValue.PublicKey;

		if (string.IsNullOrWhiteSpace(publicKey))
			throw new InvalidOperationException($"'{VapidOptions.Path}:publicKey' is not configured.");

		return await Task.FromResult(publicKey);
	}
}
