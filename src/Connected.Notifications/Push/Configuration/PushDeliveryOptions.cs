namespace Connected.Notifications.Push.Configuration;

/// <summary>
/// A global switch for whether this instance sends push notifications at all.
/// </summary>
/// <remarks>
/// The kill switch: flipping <see cref="Enabled"/> to false in config (no deploy needed) stops every
/// <c>Send</c>/<c>SendToSelf</c> call from queuing new deliveries, without touching subscriptions or
/// preferences. Meant for an incident where sending itself needs to stop - a runaway sender, a push
/// service actively misbehaving - independent of any single user's or device's state.
/// </remarks>
public sealed class PushDeliveryOptions
{
	public const string Path = "notifications:push:delivery";

	public bool Enabled { get; set; } = true;
}
