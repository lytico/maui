using System;
using System.Runtime.InteropServices;
using GLib;
using Microsoft.Maui.Devices;
using Microsoft.Maui.LifecycleEvents;

namespace Microsoft.Maui
{
	public class MauiGTKWindow : Gtk.Window
	{
		public MauiGTKWindow(string title) : base(title)
		{
		}

		//public void PopulateFromXaml(IWindow window, IMauiContext mauiContext)
		//{
		//	if (window is Window win)
		//	{
		//		foreach (var child in window.NativeWindow.Children)
		//		{
		//			Add(child);
		//		}
		//	}
		//}
	}
}
