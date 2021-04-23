namespace Microsoft.Maui.Graphics.Native.Gtk
{

	public static class GraphicsExtensions
	{

		public static Rectangle ToRectangle(this Gdk.Rectangle it)
			=> new(it.X, it.Y, it.Width, it.Height);

	}

}