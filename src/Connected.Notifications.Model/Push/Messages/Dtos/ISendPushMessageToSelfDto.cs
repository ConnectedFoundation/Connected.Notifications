using Connected.Services;

namespace Connected.Notifications.Push.Messages.Dtos;

public interface ISendPushMessageToSelfDto
	: IDto
{
	string Title { get; set; }
	string Body { get; set; }
	string? Url { get; set; }

	/// <summary>
	/// Gets or sets the message identifier the receiving worker looks up against whatever locale map the
	/// app last gave it, showing that text instead of <see cref="Title"/>/<see cref="Body"/> when it has
	/// a match. Left null to see this call's own text exactly as sent - useful for trying delivery end
	/// to end.
	/// </summary>
	string? MessageKey { get; set; }
}
