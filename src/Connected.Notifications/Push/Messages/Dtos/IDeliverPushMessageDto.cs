using Connected.Services;

namespace Connected.Notifications.Push.Messages.Dtos;

internal interface IDeliverPushMessageDto
	: IDto
{
	int Subscription { get; set; }
	string Title { get; set; }
	string Body { get; set; }
	string? Url { get; set; }
	string? MessageKey { get; set; }
}
