using System;
using Gtk;
using Microsoft.Maui.LifecycleEvents;

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

		}

		void OnVisibilityNotifyEvent(object o, VisibilityNotifyEventArgs args)
		{
			MauiGtkApplication0.Current.Services?.InvokeLifecycleEvents<LinuxLifecycle.OnVisibilityChanged>(del => del(this, args));
		}

		void OnHidden(object? sender, EventArgs args)
		{
			MauiGtkApplication0.Current.Services?.InvokeLifecycleEvents<LinuxLifecycle.OnHidden>(del => del(this, args));
		}

		void OnShown(object? sender, EventArgs args)
		{
			MauiGtkApplication0.Current.Services?.InvokeLifecycleEvents<LinuxLifecycle.OnShown>(del => del(this, args));
		}

		void OnWindowStateEvent(object o, WindowStateEventArgs args)
		{
			MauiGtkApplication0.Current.Services?.InvokeLifecycleEvents<LinuxLifecycle.OnStateChanged>(del => del(this, args));
		}


	}

}