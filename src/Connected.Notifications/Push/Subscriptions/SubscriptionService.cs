using Connected.Entities;
using Connected.Notifications.Push.Subscriptions.Dtos;
using Connected.Notifications.Push.Subscriptions.Ops;
using Connected.Services;
using System.Collections.Immutable;

namespace Connected.Notifications.Push.Subscriptions;

internal sealed class SubscriptionService(IServiceProvider services)
	: Service(services), ISubscriptionService
{
	public async Task<int> Insert(IInsertSubscriptionDto dto) => await Invoke(GetOperation<Insert>(), dto);

	public async Task Delete(IDeleteSubscriptionDto dto) => await Invoke(GetOperation<DeleteByEndpoint>(), dto);

	public async Task<string> SelectPublicKey() => await Invoke(GetOperation<SelectPublicKey>(), Dto.Empty);

	public async Task Delete(IPrimaryKeyDto<int> dto) => await Invoke(GetOperation<Delete>(), dto);

	public async Task<ISubscription?> Select(IPrimaryKeyDto<int> dto) => await Invoke(GetOperation<Select>(), dto);

	public async Task<IImmutableList<ISubscription>> Query(IValueDto<string> dto) => await Invoke(GetOperation<Query>(), dto);
}
