using Connected.Notifications.Push.Messages.Dtos;
using Connected.Notifications.Push.Messages.Ops;
using Connected.Services;

namespace Connected.Notifications.Push.Messages;

internal sealed class PushMessageService(IServiceProvider services)
	: Service(services), IPushMessageService
{
	public async Task Send(ISendPushMessageDto dto) => await Invoke(GetOperation<Send>(), dto);

	public async Task SendToSelf(ISendPushMessageToSelfDto dto) => await Invoke(GetOperation<SendToSelf>(), dto);

	public async Task SendBroadcast(ISendBroadcastPushMessageDto dto) => await Invoke(GetOperation<SendBroadcast>(), dto);
}
