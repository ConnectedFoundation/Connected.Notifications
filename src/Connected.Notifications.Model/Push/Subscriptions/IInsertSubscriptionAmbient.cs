using Connected.Notifications.Push.Subscriptions.Dtos;
using Connected.Services;

namespace Connected.Notifications.Push.Subscriptions;

public interface IInsertSubscriptionAmbient
	: IAmbientProvider<IInsertSubscriptionDto>
{
	string AuthenticationToken { get; set; }
	string EndpointHash { get; set; }
	string KeyId { get; set; }
	DateTimeOffset Created { get; set; }
}
