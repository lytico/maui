using System;
using System.Diagnostics;
using Gtk;
using Microsoft.Maui.LifecycleEvents;
using Application = GLib.Application;

namespace Microsoft.Maui
{

	public class MauiGtkMainWindow : Gtk.Window
	{

		public MauiGtkMainWindow() : base(WindowType.Toplevel)
		{
			WindowStateEvent += OnWindowStateEvent;
			Shown += OnShown;
			Hidden += OnHidden;
			VisibilityNotifyEvent += OnVisibilityNotifyEvent;
			DeleteEvent += OnDeleteEvent;

		}

		void OnDeleteEvent(object o, DeleteEventArgs args)
		{
			MauiGtkApplication.Current.Services?.InvokeLifecycleEvents<LinuxLifecycle.OnDelete>(del => del(this, args));

			if (MauiGtkApplication.Current.MainWindow == o)
			{

				((Application)MauiGtkApplication.CurrentGtkApplication).Quit();

				args.Event.SendEvent = true;
			}
		}

		void OnVisibilityNotifyEvent(object o, VisibilityNotifyEventArgs args)
		{
			MauiGtkApplication.Current.Services?.InvokeLifecycleEvents<LinuxLifecycle.OnVisibilityChanged>(del => del(this, args));
		}

		void OnHidden(object? sender, EventArgs args)
		{
			MauiGtkApplication.Current.Services?.InvokeLifecycleEvents<LinuxLifecycle.OnHidden>(del => del(this, args));
		}

		void OnShown(object? sender, EventArgs args)
		{
			MauiGtkApplication.Current.Services?.InvokeLifecycleEvents<LinuxLifecycle.OnShown>(del => del(this, args));
		}

		void OnWindowStateEvent(object o, WindowStateEventArgs args)
		{
			MauiGtkApplication.Current.Services?.InvokeLifecycleEvents<LinuxLifecycle.OnStateChanged>(del => del(this, args));
		}

	}

}