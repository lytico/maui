using System;
using Gtk;
using Pango;

namespace Microsoft.Maui.Platform
{
	public class SwitchControl : CustomAltView
	{
		public SwitchControl()
		{
			SwitchWidget = new CheckButton();
			Add(SwitchWidget);
		}

		public Gtk.CheckButton SwitchWidget { get; set; }
	}
}
