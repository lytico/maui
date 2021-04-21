using Gtk;
using Microsoft.Maui.Graphics.Native.Gtk;
using MG = Microsoft.Maui.Graphics;

namespace Microsoft.Maui
{
	public static class WidgetExtensions
	{

		public static void UpdateIsEnabled(this Widget native, bool isEnabled) =>
			native.Sensitive = isEnabled;

		public static void SetBackgroundColor(this Gtk.Widget widget, MG.Color color)
		{
			widget.SetBackgroundColor(Gtk.StateType.Normal, color);
		}

		public static void SetBackgroundColor(this Gtk.Widget widget, Gtk.StateType state, MG.Color color)
		{
			widget.SetBackgroundColor(state.ToGtk3StateFlags(), color);
		}

		public static void SetBackgroundColor(this Gtk.Widget widget, Gtk.StateFlags state, MG.Color color)
		{
#pragma warning disable 612
			widget.OverrideColor(state, color.ToGtkRgbaValue());
#pragma warning restore 612
		}

		public static Gtk.StateFlags ToGtk3StateFlags(this Gtk.StateType state)
		{
			switch (state)
			{
				case Gtk.StateType.Active:
					return Gtk.StateFlags.Active;
				case Gtk.StateType.Prelight:
					return Gtk.StateFlags.Prelight;
				case Gtk.StateType.Insensitive:
					return Gtk.StateFlags.Insensitive;
				case Gtk.StateType.Focused:
					return Gtk.StateFlags.Active;
				case Gtk.StateType.Inconsistent:
					return Gtk.StateFlags.Normal;
				case Gtk.StateType.Selected:
					return Gtk.StateFlags.Selected;
			}

			return Gtk.StateFlags.Normal;
		}

	}

}