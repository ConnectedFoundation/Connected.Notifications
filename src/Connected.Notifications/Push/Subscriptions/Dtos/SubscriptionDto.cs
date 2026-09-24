using Connected.Services;

namespace Connected.Notifications.Push.Subscriptions.Dtos;

internal abstract class SubscriptionDto
	: Dto, ISubscriptionDto
{
	public required string Endpoint { get; set; }
	public required string P256dh { get; set; }
	public required string Auth { get; set; }
}
