namespace Microsoft.Maui.Graphics.Native.Gtk
{

	public static class TextExtensions
	{

		public static double GetLineHeigth(this Pango.Layout layout)
		{
			var inkRect = new Pango.Rectangle();
			var logicalRect = new Pango.Rectangle();
			layout.GetLineReadonly(0).GetExtents(ref inkRect, ref logicalRect);
			var lineHeigh = logicalRect.Height.ScaledFromPango();

			return lineHeigh;
		}

	}

}