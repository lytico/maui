using System;
using Gtk;
using Pango;

namespace Microsoft.Maui.Platform
{
	public class SwitchControl : CustomView
	{
		public SwitchControl()
		{
			SwitchWidget = new CheckButton();
			Add(SwitchWidget);
		}

		public Gtk.CheckButton SwitchWidget { get; set; }
	}
}
