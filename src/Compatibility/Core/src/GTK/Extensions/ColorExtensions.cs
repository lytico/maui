namespace Microsoft.Maui.Controls.Compatibility.Platform.GTK.Extensions
{
	public static class ColorExtensions
	{
		public static Gdk.Color ToGtkColor(this Graphics.Color color)
		{
			string hex = color.ToRgbaColor();
			Gdk.Color gtkColor = new Gdk.Color();
			Gdk.Color.Parse(hex, ref gtkColor);

			return gtkColor;
		}

		//internal static Microsoft.Maui.Controls.Compatibility.Color ToXFColor(this Gdk.Color color, double opacity = 255)
		//{
		//	return new Color(color.Red, color.Green, color.Blue, opacity);
		//}

		internal static string ToRgbaColor(this Graphics.Color color)
		{
			int red = (int)(color.Red * 255);
			int green = (int)(color.Green * 255);
			int blue = (int)(color.Blue * 255);

			return string.Format("#{0:X2}{1:X2}{2:X2}", red, green, blue);
		}

		internal static bool IsDefaultOrTransparent(this Graphics.Color color)
		{
			return color.Alpha == 0 || color.ToGtkColor().Equals(color.GetDefault());
		}

		internal static Gdk.Color GetDefault(this Graphics.Color color)
		{
			return new Graphics.Color(0xFF, 0xFF, 0xFF, 0xFF).ToGtkColor();
		}

		internal static Gdk.Color GetBlack(this Graphics.Color color)
		{
			return new Graphics.Color(0, 0, 0, 0xFF).ToGtkColor();
		}
	}
}
