namespace Connected.Notifications.Push.Configuration;

/// <summary>
/// The VAPID identity of this application server towards the browser push services.
/// </summary>
/// <remarks>
/// One key pair per tenant, generated when the tenant is provisioned. Every browser
/// subscription is bound to the public key it was created with, so replacing the pair
/// invalidates every existing subscription of that tenant.
/// </remarks>
public sealed class VapidOptions
{
	public const string Path = "notifications:push:vapid";

	public string? Subject { get; set; }
	public string? PublicKey { get; set; }
	public string? PrivateKey { get; set; }

	/// <summary>
	/// Stamped onto every subscription created while it's active, so a subscription signed under
	/// a since-retired pair can be told apart from one that is failing for some other reason.
	/// </summary>
	public string? KeyId { get; set; }
}
