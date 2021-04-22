using System.Linq;
using Gtk;

namespace Microsoft.Maui
{

	public static class FontExtensions
	{

		public static Pango.FontDescription GetPangoFontDescription(this Widget it)
#pragma warning disable 612
			=> it.StyleContext.GetFont(it.StateFlags);
#pragma warning restore 612

		/// <summary>
		/// size in points
		/// <seealso cref="https://developer.gnome.org/pygtk/stable/class-pangofontdescription.html#method-pangofontdescription--set-size"/>
		/// the size of a font description is specified in pango units.
		/// There are <see cref="Pango.Scale.PangoScale"/> pango units in one device unit (the device unit is a point for font sizes).
		/// </summary>
		/// <param name="it"></param>
		/// <returns></returns>
		public static double GetSize(this Pango.FontDescription it)
			=> it.Size / Pango.Scale.PangoScale;
		
		public static Pango.FontFamily GetPangoFontFamily(this Widget it)
			=> it.FontMap.Families.First();


	}

}