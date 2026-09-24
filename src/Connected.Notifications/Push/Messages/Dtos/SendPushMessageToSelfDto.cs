using Connected.Services;
using System.ComponentModel.DataAnnotations;

namespace Connected.Notifications.Push.Messages.Dtos;

internal sealed class SendPushMessageToSelfDto
	: Dto, ISendPushMessageToSelfDto
{
	[Required, MaxLength(128)]
	public required string Title { get; set; }

	[Required, MaxLength(512)]
	public required string Body { get; set; }

	[MaxLength(256)]
	public string? Url { get; set; }
}
