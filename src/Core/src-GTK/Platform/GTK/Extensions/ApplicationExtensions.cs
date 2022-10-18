using System.Collections.Generic;
using System.Runtime.Versioning;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.LifecycleEvents;

namespace Microsoft.Maui.Platform
{
	public static class ApplicationExtensions
	{
		public static void CreatePlatformWindow(this MauiGTKApplication platformApplication)
		{
			//if (application.Handler?.MauiContext is not IMauiContext applicationContext)
			//	return;

			Gtk.Window winuiWndow = new Gtk.Window("My first GTK# Application! ");
			if (winuiWndow is not null)
			{
				//var mauiContext = applicationContext!.MakeWindowScope(winuiWndow, out var windowScope);
				//applicationContext.Services.InvokeLifecycleEvents<GTKLifecycle.OnMauiContextCreated>(del => del(mauiContext));

				//var activationState = new ActivationState(mauiContext);

				//var window = application.CreateWindow(activationState);

				//winuiWndow.SetWindowHandler(window, mauiContext);

				//applicationContext.Services.InvokeLifecycleEvents<GTKLifecycle.OnWindowCreated>(del => del(winuiWndow));

				//winuiWndow.Activate();
				winuiWndow.Show();
			}
		}
	}
}