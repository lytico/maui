using System;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.Maui.Platform
{
	internal static partial class MauiContextExtensions
	{
		public static NavigationRootManager GetNavigationRootManager(this IMauiContext mauiContext) =>
			mauiContext.Services.GetRequiredService<NavigationRootManager>();

		public static Gtk.Window GetPlatformWindow(this IMauiContext mauiContext) =>
			mauiContext.Services.GetRequiredService<Gtk.Window>();


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