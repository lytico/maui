using System;
using Gtk;

namespace Microsoft.Maui.Handlers
{

	public partial class PickerHandler : ViewHandler<IPicker, ComboBox>
	{

		protected override ComboBox CreateNativeView()
		{
			var model = new ListStore(typeof(string));
			var cell = new CellRendererText();

			var cb = new ComboBox(model);
			cb.PackStart(cell, false);
			cb.SetAttributes(cell, "text", 0);

			return cb;
		}

		public override void SetVirtualView(IView view)
		{
			base.SetVirtualView(view);

			_ = NativeView ?? throw new InvalidOperationException($"{nameof(NativeView)} should have been set by base class.");
			_ = VirtualView ?? throw new InvalidOperationException($"{nameof(VirtualView)} should have been set by base class.");

			SetValues(NativeView, VirtualView);
		}

		public static void SetValues(ComboBox nativeView, IPicker virtualView)
		{
			var list = new ItemDelegateList<string>(virtualView);

			if (nativeView.Model is not ListStore model)
				return;

			model.Clear();

			foreach (var text in list)
			{
				model.AppendValues(text);
			}
		}

		[MissingMapper]
		public static void MapTitle(PickerHandler handler, IPicker view) { }

		public static void MapSelectedIndex(PickerHandler handler, IPicker view) { }

		[MissingMapper]
		public static void MapCharacterSpacing(PickerHandler handler, IPicker view) { }

		public static void MapFont(PickerHandler handler, IPicker view)
		{
			handler.MapFont(view);

		}

		[MissingMapper]
		public static void MapTextColor(PickerHandler handler, IPicker view) { }

		public static void MapReload(PickerHandler handler, IPicker picker)
		{
			var nativeView = handler.NativeView ?? throw new InvalidOperationException($"{nameof(NativeView)} should have been set by base class.");
			_ = picker ?? throw new InvalidOperationException($"{nameof(picker)} should have been set by base class.");

			SetValues(nativeView, picker);

		}

		[MissingMapper]
		public static void MapHorizontalTextAlignment(PickerHandler handler, IPicker view) { }

	}

}