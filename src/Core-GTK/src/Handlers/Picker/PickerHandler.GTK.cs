using System;
using System.Collections.Specialized;
using System.Linq;

namespace Microsoft.Maui.Handlers
{
	public partial class PickerHandler : ViewHandler<IPicker, MauiView>
	{
		protected override MauiView CreatePlatformView(IView picker)
		{
			var plat = new MauiView();
			Gtk.ComboBox platformChild = null!;

			if (picker is IPicker pickerView) {
				if (pickerView.Items != null) {
					platformChild = new Gtk.ComboBox(pickerView.Items.ToArray());
					// name_combo.set_entry_text_column(1)
					platformChild.EntryTextColumn = 0;
					platformChild.Active = 1;
					platformChild.Changed += PlatformChild_Changed;
					plat.AddChildWidget(platformChild);
				} else {
					platformChild = new Gtk.ComboBox();
					// name_combo.set_entry_text_column(1)
					platformChild.EntryTextColumn = 0;
					platformChild.Changed += PlatformChild_Changed;
					platformChild.Active = 1;
					plat.AddChildWidget(platformChild);
				}

				//if (pickerView.Items.Count > 0) {
				//	var name_store = new Gtk.ListStore(typeof(int), typeof(string));
				//	for (int i = 0; i < pickerView.Items.Count; i++) {
				//		var item = pickerView.Items[i];
				//		name_store.AppendValues(i, item);
				//	}
				//	platformChild.Model = name_store;
				//}
				 
				if (pickerView.Visibility == Visibility.Visible)
				{
					if (platformChild != null!) {
						platformChild.Show();
					}
					plat.Show();
				}
			}

			return plat;
		}

		private void PlatformChild_Changed(object sender, EventArgs e)
		{
			if (sender is Gtk.ComboBox comboBox) {
				this.VirtualView.SelectedIndex = comboBox.Active;
			}
		}

		protected override void ConnectHandler(MauiView platformView)
		{
			//platformView.FocusChange += OnFocusChange;
			//platformView.Click += OnClick;

			base.ConnectHandler(platformView);
		}

		protected override void DisconnectHandler(MauiView platformView)
		{
			//platformView.FocusChange -= OnFocusChange;
			//platformView.Click -= OnClick;

			base.DisconnectHandler(platformView);
		}

		// This is a Android-specific mapping
		public static void MapBackground(IPickerHandler handler, IPicker picker)
		{
			//handler.PlatformView?.UpdateBackground(picker);
		}

		// Uncomment me on NET7 [Obsolete]
		public static void MapReload(IPickerHandler handler, IPicker picker, object? args) => Reload(handler, picker);

		internal static void MapItems(IPickerHandler handler, IPicker picker) => Reload(handler, picker);

		public static void MapTitle(IPickerHandler handler, IPicker picker)
		{
			//handler.PlatformView?.UpdateTitle(picker);
		}

		public static void MapTitleColor(IPickerHandler handler, IPicker picker)
		{
			//handler.PlatformView?.UpdateTitleColor(picker);
		}

		public static void MapSelectedIndex(IPickerHandler handler, IPicker picker)
		{
			if (handler.PlatformView is MauiView mView) {
				if (mView.GetChildWidget() is Gtk.ComboBox comboBox) {
					comboBox.Active = picker.SelectedIndex;
				}
			}
			//handler.PlatformView?.UpdateSelectedIndex(picker);
		}

		public static void MapCharacterSpacing(IPickerHandler handler, IPicker picker)
		{
			//handler.PlatformView?.UpdateCharacterSpacing(picker);
		}

		public static void MapFont(IPickerHandler handler, IPicker picker)
		{
			//var fontManager = handler.GetRequiredService<IFontManager>();

			//handler.PlatformView?.UpdateFont(picker, fontManager);
		}

		public static void MapHorizontalTextAlignment(IPickerHandler handler, IPicker picker)
		{
			//handler.PlatformView?.UpdateHorizontalAlignment(picker.HorizontalTextAlignment);
		}

		public static void MapTextColor(IPickerHandler handler, IPicker picker)
		{
			//handler.PlatformView.UpdateTextColor(picker);
		}

		public static void MapVerticalTextAlignment(IPickerHandler handler, IPicker picker)
		{
			//handler.PlatformView?.UpdateVerticalAlignment(picker.VerticalTextAlignment);
		}

		//		void OnFocusChange(object? sender, global::Android.Views.View.FocusChangeEventArgs e)
		//		{
		//			if (PlatformView == null)
		//				return;

		//			if (e.HasFocus)
		//			{
		//				if (PlatformView.Clickable)
		//					PlatformView.CallOnClick();
		//				else
		//					OnClick(PlatformView, EventArgs.Empty);
		//			}
		//			else if (_dialog != null)
		//			{
		//				_dialog.Hide();
		//				_dialog = null;
		//			}
		//		}

		//		void OnClick(object? sender, EventArgs e)
		//		{
		//			if (_dialog == null && VirtualView != null)
		//			{
		//				using (var builder = new AlertDialog.Builder(Context))
		//				{
		//					if (VirtualView.TitleColor == null)
		//					{
		//						builder.SetTitle(VirtualView.Title ?? string.Empty);
		//					}
		//					else
		//					{
		//						var title = new SpannableString(VirtualView.Title ?? string.Empty);
		//#pragma warning disable CA1416 // https://github.com/xamarin/xamarin-android/issues/6962
		//						title.SetSpan(new ForegroundColorSpan(VirtualView.TitleColor.ToPlatform()), 0, title.Length(), SpanTypes.ExclusiveExclusive);
		//#pragma warning restore CA1416
		//						builder.SetTitle(title);
		//					}

		//					string[] items = VirtualView.GetItemsAsArray();

		//					for (var i = 0; i < items.Length; i++)
		//					{
		//						var item = items[i];
		//						if (item == null)
		//							items[i] = String.Empty;
		//					}

		//					builder.SetItems(items, (s, e) =>
		//					{
		//						var selectedIndex = e.Which;
		//						VirtualView.SelectedIndex = selectedIndex;
		//						base.PlatformView?.UpdatePicker(VirtualView);
		//					});

		//					builder.SetNegativeButton(AResource.String.Cancel, (o, args) => { });

		//					_dialog = builder.Create();
		//				}

		//				if (_dialog == null)
		//					return;

		//				_dialog.SetCanceledOnTouchOutside(true);

		//				_dialog.DismissEvent += (sender, args) =>
		//				{
		//					_dialog = null;
		//				};

		//				_dialog.Show();
		//			}
		//		}

		static void Reload(IPickerHandler handler, IPicker picker)
		{
			if (handler.PlatformView.GetChildWidget() is Gtk.ComboBox childComboBox) {
				if (picker.Items.Count > 0) {
					var name_store = new Gtk.ListStore(typeof(string));
					for (int i = 0; i < picker.Items.Count; i++) {
						var item = picker.Items[i];
						name_store.AppendValues(item);
					}
					childComboBox.Model = name_store;
				}
				childComboBox.Active = 1;
			}
		}
	}
}