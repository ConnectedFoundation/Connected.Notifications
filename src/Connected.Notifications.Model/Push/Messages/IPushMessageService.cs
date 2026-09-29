using Connected.Annotations;
using Connected.Notifications.Push.Messages.Dtos;

namespace Connected.Notifications.Push.Messages;

/// <remarks>
/// <see cref="Send"/> deliberately has no <c>[ServiceOperation]</c>: only methods carrying one are published as
/// HTTP routes, so it stays callable from backend code (the triggers in later stories) but never from a browser,
/// where any signed-in user could otherwise push to anyone.
/// </remarks>
[Service, ServiceUrl(NotificationsUrls.PushMessageService)]
public interface IPushMessageService
{
	Task Send(ISendPushMessageDto dto);

	/// <summary>
	/// Sends a push notification to the caller's own devices, for trying delivery end to end.
	/// </summary>
	/// <remarks>
	/// Safe to publish because the recipient is always the calling identity, never taken from the request.
	/// </remarks>
	[ServiceOperation(ServiceOperationVerbs.Post)]
	Task SendToSelf(ISendPushMessageToSelfDto dto);

	/// <summary>
	/// Sends a push notification to every identity currently subscribed to notifications.
	/// </summary>
	/// <remarks>
	/// No <c>[ServiceOperation]</c>, for the same reason as <see cref="Send"/>: published, any signed-in
	/// user could push to everyone.
	/// </remarks>
	Task SendBroadcast(ISendBroadcastPushMessageDto dto);
}
