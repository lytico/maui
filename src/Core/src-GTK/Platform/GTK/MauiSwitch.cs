using System;
using Gtk;
using Pango;

namespace Microsoft.Maui.Platform
{
	public class MauiSwitch : MauiView
	{
		public MauiSwitch()
		{
			SwitchWidget = new CheckButton();
			Add(SwitchWidget);
		}

		public Gtk.CheckButton SwitchWidget { get; set; }
	}
}
