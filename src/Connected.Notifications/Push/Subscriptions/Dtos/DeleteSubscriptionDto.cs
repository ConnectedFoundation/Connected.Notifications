using Connected.Services;
using System.ComponentModel.DataAnnotations;

namespace Connected.Notifications.Push.Subscriptions.Dtos;

internal sealed class DeleteSubscriptionDto
	: Dto, IDeleteSubscriptionDto
{
	[Required]
	public required string Endpoint { get; set; }
}
