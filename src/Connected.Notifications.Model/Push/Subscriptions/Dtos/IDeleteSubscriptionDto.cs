using Connected.Services;

namespace Connected.Notifications.Push.Subscriptions.Dtos;

public interface IDeleteSubscriptionDto
	: IDto
{
	string Endpoint { get; set; }
}
