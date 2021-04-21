

namespace Microsoft.Maui.Graphics.Native.Gtk
{

	public static class ColorExtensions
	{
		public static Gdk.RGBA ToNative(this Color color)
		{
			string hex = color.ToHex();
			Gdk.RGBA nativeColor = new Gdk.RGBA();
			nativeColor.Parse(hex);

			return nativeColor;
		}
		
		public static Gdk.RGBA ToGtkRgba (this Color color) 
			=> new() { Red = color.Red, Green = color.Green, Blue = color.Blue, Alpha = color.Alpha };
		
		public static Color ToColor(this Gdk.Color color, float opacity = 255)
		{
			return new Color(color.Red, color.Green, color.Blue, opacity);
		}
		
		public static Cairo.Color ToCairoColor (this Color col) 
			=> new(col.Red, col.Green, col.Blue, col.Alpha);

		public static Cairo.Color ToCairoColor (this Gdk.RGBA color) 
			=> new(color.Red, color.Green, color.Blue, color.Alpha);

		public static Gdk.Color ToGdkColor (this Color color) 
			=> new((byte)(color.Red * 255), (byte)(color.Green * 255), (byte)(color.Blue * 255));

		public static Color ToColor (this Gdk.Color color) 
			=> new((float)color.Red / (float)ushort.MaxValue, (float)color.Green / (float)ushort.MaxValue, (float)color.Blue / (float)ushort.MaxValue);

	}
}