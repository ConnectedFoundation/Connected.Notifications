using Connected.Authentication;
using Connected.Notifications.Push.Messages.Dtos;
using Connected.Services;

namespace Connected.Notifications.Push.Messages.Ops;

internal sealed class SendToSelf(IAuthenticationService authentication, IPushMessageService messages)
	: ServiceAction<ISendPushMessageToSelfDto>
{
	protected override async Task OnInvoke()
	{
		var token = (await authentication.SelectIdentity())?.Token
			?? throw new InvalidOperationException("An authenticated identity is required to send a push notification.");

		await messages.Send(DtoFactory.Create<ISendPushMessageDto>(f =>
		{
			f.AuthenticationToken = token;
			f.Title = Dto.Title;
			f.Body = Dto.Body;
			f.Url = Dto.Url;
		}));
	}
}
