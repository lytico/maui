using System;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.Maui.Platform
{
	internal static partial class MauiContextExtensions
	{
		public static NavigationRootManager GetNavigationRootManager(this IMauiContext mauiContext) =>
			mauiContext.Services.GetRequiredService<NavigationRootManager>();

		public static Gdk.Window GetPlatformWindow(this IMauiContext mauiContext) =>
			mauiContext.Services.GetRequiredService<Gdk.Window>();

		public static UI.Xaml.Window? GetOptionalPlatformWindow(this IMauiContext mauiContext) =>
			mauiContext.Services.GetService<UI.Xaml.Window>();

		public static IServiceProvider GetApplicationServices(this IMauiContext mauiContext)
		{
			return MauiGTKApplication.Current.Services
				?? throw new InvalidOperationException("Unable to find Application Services");
		}


		public static IMauiContext MakeScoped(this IMauiContext mauiContext, bool registerNewNavigationRoot)
		{
			var scopedContext = new MauiContext(mauiContext.Services);

			if (registerNewNavigationRoot)
			{
				scopedContext.AddWeakSpecific(new NavigationRootManager(scopedContext.GetPlatformWindow()));
			}

			return scopedContext;
		}
	}
}