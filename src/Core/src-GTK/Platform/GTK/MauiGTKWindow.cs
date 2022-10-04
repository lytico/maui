using System;
using System.Runtime.InteropServices;
using Microsoft.Maui.Devices;
using Microsoft.Maui.LifecycleEvents;

namespace Microsoft.Maui
{
	public class MauiGTKWindow : Gtk.Window
	{
		public MauiGTKWindow(string title) : base(title)
		{
		}
	}
}
