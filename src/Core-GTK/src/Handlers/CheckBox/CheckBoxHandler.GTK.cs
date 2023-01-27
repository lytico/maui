using System.Diagnostics;
using Gtk;

namespace Microsoft.Maui.Handlers
{
	public partial class CheckBoxHandler : ViewHandler<ICheckBox, MauiView>
	{
		protected override MauiView CreatePlatformView(IView checkBox)
		{
			var plat = new MauiView();
			var platformCheckBox = new Gtk.CheckButton();

			Gtk.Widget widget = platformCheckBox;
			SetMargins(checkBox, ref widget);

			plat.AddChildWidget(platformCheckBox);

			return plat;
		}

		protected override void ConnectHandler(MauiView platformView)
		{
			if (platformView.GetChildWidget() is Gtk.CheckButton checkButton)
			{
				checkButton.Clicked += CheckButton_Clicked;
			}
			//platformView.CheckedChange += OnCheckedChange;
		}

		private void CheckButton_Clicked(object sender, System.EventArgs e)
		{
			Debug.WriteLine("Clicked");
			if (VirtualView != null)
			{
				if (sender is Gtk.CheckButton checkButton)
				{
					if (checkButton.Active)
						VirtualView.IsChecked = true;
					else
						VirtualView.IsChecked = false;
				}
			}
		}

		protected override void DisconnectHandler(MauiView platformView)
		{
			if (platformView.GetChildWidget() is Gtk.CheckButton checkButton)
			{
				checkButton.Clicked -= CheckButton_Clicked;
			}
			//platformView.CheckedChange -= OnCheckedChange;
		}

		// This is an Android-specific mapping
		public static void MapBackground(ICheckBoxHandler handler, ICheckBox check)
		{
			//handler.PlatformView?.UpdateBackground(check);
		}

		public static void MapIsChecked(ICheckBoxHandler handler, ICheckBox check)
		{
			//handler.PlatformView?.UpdateIsChecked(check);
		}

		public static void MapForeground(ICheckBoxHandler handler, ICheckBox check)
		{
			//handler.PlatformView?.UpdateForeground(check);
		}

		//void OnCheckedChange(object? sender, CompoundButton.CheckedChangeEventArgs e)
		//{
		//	if (VirtualView != null)
		//		VirtualView.IsChecked = e.IsChecked;
		//}
	}
}