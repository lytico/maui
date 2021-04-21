namespace Microsoft.Maui.Graphics.Native.Gtk {

	public static class GtkGraphicsExtensions {

		public static Cairo.Color ToCairoColor (this Color col) 
			=> new(col.Red, col.Green, col.Blue, col.Alpha);

		public static Cairo.Color ToCairoColor (this Gdk.RGBA color) 
			=> new(color.Red, color.Green, color.Blue, color.Alpha);

		public static Gdk.Color ToGdkColor (this Color color) 
			=> new((byte)(color.Red * 255), (byte)(color.Green * 255), (byte)(color.Blue * 255));

		public static Color ToColor (this Gdk.Color color) 
			=> new((float)color.Red / (float)ushort.MaxValue, (float)color.Green / (float)ushort.MaxValue, (float)color.Blue / (float)ushort.MaxValue);

		public static Gdk.RGBA ToGtkRgbaValue (this Color color) 
			=> new Gdk.RGBA { Red = color.Red, Green = color.Green, Blue = color.Blue, Alpha = color.Alpha };

	}

}