using System;
using System.ComponentModel;
using Microsoft.Maui.Graphics;

namespace Microsoft.Maui.Graphics
{
	public static class ColorExtensions
	{
		public static string ToColorString(this Color color)
		{
			// ‘#00FF007F’ will be interpreted as specifying both a foreground color and foreground alpha.
			color.ToRgba(out byte r, out byte g, out byte b, out byte a);
			var byteArrayRed = new byte[1];
			byteArrayRed[0] = r;
			var red = BitConverter.ToString(byteArrayRed);
			var byteArrayGreen = new byte[1];
			byteArrayGreen[0] = g;
			var green = BitConverter.ToString(byteArrayGreen);
			var byteArrayBlue = new byte[1];
			byteArrayBlue[0] = b;
			var blue = BitConverter.ToString(byteArrayBlue);
			var byteArrayAlpha = new byte[1];
			byteArrayAlpha[0] = a;
			var alpha = BitConverter.ToString(byteArrayAlpha);
			return "#" + red.ToUpper() + green.ToUpper() + blue.ToUpper() + alpha.ToUpper();
		}
	}
}