using Gtk;

namespace Microsoft.Maui.Handlers
{
	public partial class PickerHandler : ViewHandler<IPicker, ComboBox>
	{
		protected override ComboBox CreateNativeView()
		{
			return new ComboBox();
		}

		[MissingMapper]
		public static void MapTitle(PickerHandler handler, IPicker view) { }

		[MissingMapper]
		public static void MapSelectedIndex(PickerHandler handler, IPicker view) { }

		[MissingMapper]
		public static void MapCharacterSpacing(PickerHandler handler, IPicker view) { }

		[MissingMapper]
		public static void MapFont(PickerHandler handler, IPicker view) { }

		[MissingMapper]
		public static void MapTextColor(PickerHandler handler, IPicker view) { }
		
		[MissingMapper]
		public static void MapReload(PickerHandler handler, IPicker picker) { }
		
		[MissingMapper]
		public static void MapHorizontalTextAlignment(PickerHandler handler, IPicker view) { }


	}
}
