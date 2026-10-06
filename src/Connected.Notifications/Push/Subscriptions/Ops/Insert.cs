using Connected.Entities;
using Connected.Notifications.Push.Subscriptions.Dtos;
using Connected.Services;
using Connected.Storage;
using Microsoft.Extensions.Logging;

namespace Connected.Notifications.Push.Subscriptions.Ops;

/// <remarks>
/// An upsert keyed on the endpoint. A browser endpoint is one physical device, so whoever registers it last owns
/// it: the same user re-registering on every app load changes nothing, and a different user signing in on the
/// same device takes it over instead of failing on the unique EndpointHash index while the previous user keeps
/// receiving that device's notifications.
/// </remarks>
internal sealed class Insert(IStorageProvider storage, ISubscriptionService subscriptions, IEventService events, ISubscriptionCache cache,
	IInsertSubscriptionAmbient ambient, ILogger<Insert> logger)
	: ServiceFunction<IInsertSubscriptionDto, int>
{
	protected override async Task<int> OnInvoke()
	{
		if (await cache.AsEntity(f => string.Equals(f.Endpoint, Dto.Endpoint, StringComparison.Ordinal)) is Subscription existing)
			return await Update(existing);

		var entity = await storage.Open<Subscription>().Update(Dto.AsEntity<Subscription>(State.Add, ambient))
			?? throw new NullReferenceException("Expected an entity.");

		SetState(entity);

		await cache.Refresh(entity.Id);
		await events.Inserted(this, subscriptions, entity.Id);

		return entity.Id;
	}

	private async Task<int> Update(Subscription existing)
	{
		var unchanged = string.Equals(existing.AuthenticationToken, ambient.AuthenticationToken, StringComparison.Ordinal)
			&& string.Equals(existing.P256dh, Dto.P256dh, StringComparison.Ordinal)
			&& string.Equals(existing.Auth, Dto.Auth, StringComparison.Ordinal)
			&& string.Equals(existing.KeyId, ambient.KeyId, StringComparison.Ordinal);

		// Every app load registers again, so the common case writes nothing.
		if (unchanged)
			return existing.Id;

		if (!string.Equals(existing.AuthenticationToken, ambient.AuthenticationToken, StringComparison.Ordinal))
			logger.LogInformation("Subscription {Subscription} was taken over by another identity signing in on the same device.", existing.Id);

		var entity = await storage.Open<Subscription>().Update(existing.Merge(Dto, State.Update, ambient))
			?? throw new NullReferenceException("Expected an entity.");

		SetState(entity);

		await cache.Refresh(entity.Id);
		await events.Updated(this, subscriptions, entity.Id);

		return entity.Id;
	}
}
