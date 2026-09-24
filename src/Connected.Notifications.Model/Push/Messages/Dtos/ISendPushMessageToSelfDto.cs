using Connected.Services;

namespace Connected.Notifications.Push.Messages.Dtos;

public interface ISendPushMessageToSelfDto
	: IDto
{
	string Title { get; set; }
	string Body { get; set; }
	string? Url { get; set; }
}
