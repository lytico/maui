namespace Microsoft.Maui.Handlers
{
	public partial class CheckBoxHandler : ViewHandler<ICheckBox, CustomView>
	{
		protected override CustomView CreatePlatformView()
		{
			var plat = new CustomView();
			var platformCheckBox = new Gtk.CheckButton();

			plat.Add(platformCheckBox);

			return plat;
		}

		protected override void ConnectHandler(CustomView platformView)
		{
			//platformView.CheckedChange += OnCheckedChange;
		}

		protected override void DisconnectHandler(CustomView platformView)
		{
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