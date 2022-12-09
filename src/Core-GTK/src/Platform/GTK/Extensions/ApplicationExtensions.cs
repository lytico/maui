using System;
using System.Collections.Generic;
using System.Runtime.Versioning;
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

			var mauiContext = applicationContext!.MakeWindowScope(winuiWndow, out var windowScope);

			applicationContext.Services.InvokeLifecycleEvents<GTKLifecycle.OnMauiContextCreated>(del => del(mauiContext));

			var window = application.CreateWindow(null!);

			winuiWndow.SetWindowHandler(window, mauiContext);
			//winuiWndow.ModifyBg(StateType.Normal, new Gdk.Color(200, 0, 200));

			var x = Convert.ToInt32(window.Content.AnchorX);
			var y = Convert.ToInt32(window.Content.AnchorY);
			var width = Convert.ToInt32(window.Content.Width);
			var height = Convert.ToInt32(window.Content.Height);

			winuiWndow.Move(x, y);
			winuiWndow.Resize(width, height);

			applicationContext.Services.InvokeLifecycleEvents<GTKLifecycle.OnWindowCreated>(del => del(winuiWndow));

			winuiWndow.ShowAll();
			//winuiWndow.ModifyBg(StateType.Normal, new Gdk.Color(200, 0, 200));
		}
	}
}