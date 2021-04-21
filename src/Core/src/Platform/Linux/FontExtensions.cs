using Gtk;

namespace Microsoft.Maui
{

	public static class FontExtensions
	{

		public static Pango.FontDescription GetPangoFontDescription(this Widget it)
#pragma warning disable 612
			=> it.StyleContext.GetFont(it.StateFlags);
#pragma warning restore 612

		public static double GetSize(this Pango.FontDescription it)
			=> it.Size / Pango.Scale.PangoScale;

	}

}