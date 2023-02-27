using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Versioning;
using GLib;
using Gtk;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Hosting;
using Microsoft.Maui.LifecycleEvents;

namespace Microsoft.Maui.Platform
{
	public static class ApplicationExtensions
	{
		public static void CreatePlatformWindow(this MauiGTKApplication platformApplication, IApplication application)
		{
			if (application.Handler?.MauiContext is not IMauiContext applicationContext)
				return;

			var winuiWndow = new MauiGTKWindow("My first GTK# Application! ");
			// winuiWndow.Resize(600, 300);
			var provider = new Gtk.CssProvider();
			if (!string.IsNullOrEmpty(platformApplication.WindowCssFileName))
			{
				var windowStyleCss = File.ReadAllText(platformApplication.WindowCssFileName);
				provider.LoadFromData(windowStyleCss);
				var context = winuiWndow.StyleContext;
				context.AddProvider(provider, Gtk.StyleProviderPriority.User);
				context.Save();
				Gtk.StyleContext.AddProviderForScreen(Gdk.Screen.Default, provider, Gtk.StyleProviderPriority.User);
			}

			var mauiContext = applicationContext!.MakeWindowScope(winuiWndow, out var windowScope);

			applicationContext.Services.InvokeLifecycleEvents<GTKLifecycle.OnMauiContextCreated>(del => del(mauiContext));

			var activationState = new ActivationState(mauiContext);

			var window = application.CreateWindow(activationState);

			winuiWndow.SetWindowHandler(window, mauiContext);
			//winuiWndow.ModifyBg(StateType.Normal, new Gdk.Color(200, 0, 200));

			var x = Convert.ToInt32(window.Content.AnchorX);
			var y = Convert.ToInt32(window.Content.AnchorY);
			winuiWndow.Move(x, y);

			if (window.Content.Width > 0 && window.Content.Height > 0)
			{
				var width = Convert.ToInt32(window.Content.Width);
				var height = Convert.ToInt32(window.Content.Height);
				winuiWndow.Resize(width, height);
			}

			applicationContext.Services.InvokeLifecycleEvents<GTKLifecycle.OnWindowCreated>(del => del(winuiWndow));

			winuiWndow.Show();
			// winuiWndow.ShowAll();
			//winuiWndow.ModifyBg(StateType.Normal, new Gdk.Color(200, 0, 200));
		}
	}
}