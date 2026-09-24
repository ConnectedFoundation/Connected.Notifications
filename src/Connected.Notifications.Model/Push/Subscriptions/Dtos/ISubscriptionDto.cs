using Connected.Services;

namespace Connected.Notifications.Push.Subscriptions.Dtos;

public interface ISubscriptionDto
	: IDto
{
	string Endpoint { get; set; }
	string P256dh { get; set; }
	string Auth { get; set; }
}
