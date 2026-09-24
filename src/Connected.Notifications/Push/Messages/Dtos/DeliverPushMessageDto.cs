using Connected.Services;

namespace Connected.Notifications.Push.Messages.Dtos;

internal sealed class DeliverPushMessageDto
	: Dto, IDeliverPushMessageDto
{
	public int Subscription { get; set; }
	public string Title { get; set; } = string.Empty;
	public string Body { get; set; } = string.Empty;
	public string? Url { get; set; }
}
