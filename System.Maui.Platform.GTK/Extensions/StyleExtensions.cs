using Gtk;

namespace System.Maui.Platform.GTK.Extensions
{

	public static class StyleExtensions
	{
		public static Gdk.Color ToGdkColor(this Gdk.RGBA color) => new Gdk.Color((byte)(color.Red * 255), (byte)(color.Green * 255), (byte)(color.Blue * 255));

		public static Gdk.Color ColorFor(this Gtk.StyleContext ctx, string postfix, Gtk.StateType state)
		{
			var prefix = string.Empty;
			// see: https://developer.gnome.org/gtk3/stable/gtk-migrating-GtkStyleContext-css.html
			// examples: (see: gtk.css) selected_bg_color insensitive_bg_color base_color theme_text_color insensitive_base_color theme_unfocused_fg_color theme_unfocused_text_color theme_unfocused_bg_color

			switch (state)
			{
				case StateType.Normal:
					prefix = "theme_unfocused_";
					break;
				case StateType.Active:
					break;
				case StateType.Prelight:
					break;
				case StateType.Selected:
					prefix = "selected_";
					break;
				case StateType.Insensitive:
					prefix = "insensitive_";
					break;
				case StateType.Inconsistent:
					break;
				case StateType.Focused:
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(state), state, null);
			}

			if (ctx.LookupColor($"{prefix}{postfix}_color", out var col))
			{
				return col.ToGdkColor();
			}

			ctx.LookupColor("base_color", out col);
			return col.ToGdkColor();
		}

		public static Gdk.Color BackgroundColor(this Gtk.Widget it, Gtk.StateType state)
		{
			return it.StyleContext.ColorFor("bg", state);
		}

		public static Gdk.Color ForegroundColor(this Gtk.Widget it, Gtk.StateType state)
		{
			return it.StyleContext.ColorFor("fg", state);
		}

		public static Gdk.Color TextColor(this Gtk.Widget it, Gtk.StateType state)
		{
			return it.StyleContext.ColorFor("text_color", state);
		}

		public static Gdk.Color BaseColor(this Gtk.Widget it, Gtk.StateType state)
		{
			return it.StyleContext.ColorFor("", state);
		}

		public static void ModifyBase(this Gtk.Widget it, Gtk.StateType state, Gdk.Color color)
		{
			// TODO
		}

		public static void ModifyTextColor(this Gtk.Widget it, Gtk.StateType state, Gdk.Color color)
		{
			// TODO
		}
	}

}