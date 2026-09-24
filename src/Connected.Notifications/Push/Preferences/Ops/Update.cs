using Connected.Entities;
using Connected.Notifications.Push.Preferences.Dtos;
using Connected.Services;
using Connected.Storage;

namespace Connected.Notifications.Push.Preferences.Ops;

/// <remarks>
/// Upsert: a user's first toggle has no existing row yet, so this inserts one; every toggle after
/// that updates it. There's no separate Insert operation on this service - a preference always
/// belongs to exactly one identity, found by <see cref="IUpdateNotificationPreferenceAmbient.AuthenticationToken"/>,
/// so "insert" and "update" are really the same caller-facing action.
/// </remarks>
internal sealed class Update(IStorageProvider storage, INotificationPreferenceService preferences, IEventService events, INotificationPreferenceCache cache, IUpdateNotificationPreferenceAmbient ambient)
	: ServiceAction<IUpdateNotificationPreferenceDto>
{
	protected override async Task OnInvoke()
	{
		var existing = await cache.AsEntity(f => f.AuthenticationToken == ambient.AuthenticationToken);

		if (existing is NotificationPreference current)
		{
			var entity = await storage.Open<NotificationPreference>().Update(current.Merge(Dto, State.Update))
				?? throw new NullReferenceException("Expected an entity.");

			await cache.Refresh(entity.Id);
			await events.Updated(this, preferences, entity.Id);
		}
		else
		{
			var entity = await storage.Open<NotificationPreference>().Update(Dto.AsEntity<NotificationPreference>(State.Add, ambient))
				?? throw new NullReferenceException("Expected an entity.");

			await cache.Refresh(entity.Id);
			await events.Inserted(this, preferences, entity.Id);
		}
	}
}
