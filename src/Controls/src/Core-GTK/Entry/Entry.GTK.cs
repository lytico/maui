using Gtk;
using Microsoft.Maui.Controls.Platform;

namespace Microsoft.Maui.Controls
{
	public partial class Entry
	{
		// public static void MapTextType(EntryHandler handler, Entry entry) =>
		//	Platform.LabelExtensions.UpdateText(handler.PlatformView, label);

		public static void MapText(EntryHandler handler, Entry entry) =>
			Platform.EntryExtensions.UpdateText(handler.PlatformView, entry);
		//	Platform.LabelExtensions.UpdateText(handler.PlatformView, label);
	}
}
