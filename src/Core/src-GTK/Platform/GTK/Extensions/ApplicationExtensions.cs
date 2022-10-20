using System.Collections.Generic;
using System.Runtime.Versioning;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Hosting;
using Microsoft.Maui.LifecycleEvents;

namespace Microsoft.Maui.Platform
{
	public static class ApplicationExtensions
	{
		public static void CreatePlatformWindow(this MauiGTKApplication platformApplication, IApplication? mauiApp)
		{
			//if (application.Handler?.MauiContext is not IMauiContext applicationContext)
			//	return;

			if (mauiApp == null)
				return;

			if (mauiApp.Handler?.MauiContext is not IMauiContext applicationContext)
				return;

			var winuiWndow = new MauiGTKWindow("My first GTK# Application! ");

			var window = mauiApp.CreateWindow(null!);
			
			//Gtk.Window winuiWndow = new Gtk.Window("My first GTK# Application! ");
			if (winuiWndow is not null)
			{
				var mauiContext = applicationContext!.MakeWindowScope(winuiWndow, out var windowScope);
				//applicationContext.Services.InvokeLifecycleEvents<GTKLifecycle.OnMauiContextCreated>(del => del(mauiContext));

				//var activationState = new ActivationState(mauiContext);

				//var window = application.CreateWindow(activationState);

				winuiWndow.SetWindowHandler(window, mauiContext);
				//winuiWndow.PopulateFromXaml(window, mauiContext);

				//applicationContext.Services.InvokeLifecycleEvents<GTKLifecycle.OnWindowCreated>(del => del(winuiWndow));

				winuiWndow.ShowAll();
				// ((Gtk.Window)winuiWndow).Show();
				//winuiWndow.NativeWindow.Show();
			}
		}
	}
}