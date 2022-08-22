// using Gtk.Primitives;

namespace Microsoft.Maui.Controls.Compatibility.Platform.GTK.Extensions
{
	public static class GtkRectangleExtensions
	{
		public static Graphics.Size ToSize(this Gdk.Rectangle rect)
		{
			return new Graphics.Size(rect.Width, rect.Height);
		}
	}
}
