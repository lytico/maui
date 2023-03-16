using System.Diagnostics;
using Gtk;

namespace Microsoft.Maui.Handlers
{
	public partial class CheckBoxHandler : ViewHandler<ICheckBox, Gtk.CheckButton>
	{
		protected override Gtk.CheckButton CreatePlatformView(IView checkBox)
		{
			var plat = new Gtk.CheckButton();

			Gtk.Widget widget = plat;
			SetMargins(checkBox, ref widget);

			if (checkBox is ICheckBox checkBoxView)
			{
				if (checkBoxView.Visibility == Visibility.Visible)
				{
					plat.Show();
				}
			}

			return plat;
		}

		protected override void ConnectHandler(Gtk.CheckButton platformView)
		{
			if (platformView is Gtk.CheckButton checkButton)
			{
				checkButton.Clicked += CheckButton_Clicked;
			}
			//platformView.CheckedChange += OnCheckedChange;
		}

		private void CheckButton_Clicked(object sender, System.EventArgs e)
		{
			// Debug.WriteLine("Clicked");
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

		protected override void DisconnectHandler(Gtk.CheckButton platformView)
		{
			if (platformView is Gtk.CheckButton checkButton)
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
			if (handler.PlatformView is Gtk.CheckButton checkBox)
			{
				if (check.IsChecked)
				{
					checkBox.Active = true;
				}
				else
				{
					checkBox.Active = false;
				}
			}
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