namespace Microsoft.Maui.Handlers
{
	public partial class CheckBoxHandler : AltViewHandler<ICheckBox, CustomAltView>
	{
		protected override CustomAltView CreatePlatformView()
		{
			var plat = new CustomAltView();
			var platformCheckBox = new Gtk.CheckButton();

			plat.Add(platformCheckBox);

			return plat;
		}

		protected override void ConnectHandler(CustomAltView platformView)
		{
			//platformView.CheckedChange += OnCheckedChange;
		}

		protected override void DisconnectHandler(CustomAltView platformView)
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