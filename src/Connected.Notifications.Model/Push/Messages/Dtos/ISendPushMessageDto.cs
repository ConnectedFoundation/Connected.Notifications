using Connected.Services;

namespace Connected.Notifications.Push.Messages.Dtos;

public interface ISendPushMessageDto
	: IDto
{
	string AuthenticationToken { get; set; }
	string Title { get; set; }
	string Body { get; set; }
	string? Url { get; set; }
}
