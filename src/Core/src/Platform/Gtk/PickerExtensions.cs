using System.Linq;
using Microsoft.Maui.Graphics;

namespace Microsoft.Maui
{

	public static class PickerExtensions
	{

		public static Gtk.CellRendererText? GetCellRendererText(this Gtk.ComboBox? nativeView) =>
			nativeView?.Cells.FirstOrDefault() is Gtk.CellRendererText cell ? cell : null;

		public static void UpdateTextColor(this Gtk.ComboBox? nativeView, Color? color)
		{
			if (nativeView == null || color == null)
				return;

			if (nativeView.HasEntry)
				nativeView.SetColor(color, "color", "box.linked > entry.combo");

			nativeView.GetCellRendererText().SetForeground(color);
		}

	}

}