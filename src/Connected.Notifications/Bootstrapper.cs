using Connected.Notifications.Push.Configuration;
using Connected.Runtime;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Connected.Notifications;

internal sealed class Bootstrapper : Startup
{
	protected override void OnConfigureServices(IServiceCollection services)
	{
		var section = Configuration.GetSection(VapidOptions.Path);

		// A bad subject fails the deployment immediately instead of surfacing later as a delivery that silently never arrives.
		services.AddOptions<VapidOptions>()
			.Bind(section)
			.Validate(
				options => Uri.TryCreate(options.Subject, UriKind.Absolute, out var subject) && subject.Scheme is "mailto" or "https",
				$"'{VapidOptions.Path}:{nameof(VapidOptions.Subject)}' must be an absolute 'mailto:' or 'https:' address.")
			.ValidateOnStart();

		services.Configure<PushDeliveryOptions>(Configuration.GetSection(PushDeliveryOptions.Path));

		services.AddPushServiceClient(options =>
		{
			var vapid = section.Get<VapidOptions>() ?? new VapidOptions();

			options.Subject = vapid.Subject;
			options.PublicKey = vapid.PublicKey;
			options.PrivateKey = vapid.PrivateKey;

			/*
			 * Without a cap the client retries a 429 (Too Many Requests) on its own, honoring Retry-After,
			 * for as long as the push service keeps asking. After 3 attempts it gives up and lets the queue's
			 * own retry take over instead.
			 */
			options.AutoRetryAfter = true;
			options.MaxRetriesAfter = 3;
		});
	}
}
