using System;
using Cairo;
using Gdk;
using GLib;
using Gtk;

namespace Microsoft.Maui.Platform.GTK
{
	public class MauiGTKLabel : Label
	{
		private Gdk.Color _defaultBorderColor;
		private Gdk.Color _defaultBackgroundColor;

		public MauiGTKLabel()
		{
			_defaultBackgroundColor = new Gdk.Color(0, 0, 0);
			_defaultBorderColor = new Gdk.Color(0, 0, 0);
			BackgroundColor = _defaultBackgroundColor;
			BorderColor = _defaultBorderColor;
		}

		public Gdk.Color? BorderColor { get; set; }
		public Gdk.Color? BackgroundColor { get; set; }
		public uint BorderWidthLabel { get; set; } = 5;
	}
}
