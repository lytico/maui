namespace Microsoft.Maui.Handlers
{
	public partial class CheckBoxHandler : ViewHandler<ICheckBox, MauiView>
	{
		protected override MauiView CreatePlatformView(IView checkBox)
		{
			var plat = new MauiView();
			var platformCheckBox = new Gtk.CheckButton();

			plat.AddChildWidget(platformCheckBox);

			return plat;
		}

		protected override void ConnectHandler(MauiView platformView)
		{
			//platformView.CheckedChange += OnCheckedChange;
		}

		protected override void DisconnectHandler(MauiView platformView)
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