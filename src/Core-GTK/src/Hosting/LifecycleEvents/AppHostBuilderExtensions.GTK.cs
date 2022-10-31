using Microsoft.Maui.Hosting;

namespace Microsoft.Maui.LifecycleEvents
{
	public static partial class AppHostBuilderExtensions
	{
		internal static MauiAppBuilder ConfigureCrossPlatformLifecycleEvents(this MauiAppBuilder builder) =>
			builder.ConfigureLifecycleEvents(events => events.AddGTK(OnConfigureLifeCycle));
		internal static MauiAppBuilder ConfigureWindowEvents(this MauiAppBuilder builder) =>
			builder;

		static void OnConfigureLifeCycle(IGTKLifecycleBuilder gtk)
		{
		}
	}
}