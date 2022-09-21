namespace Microsoft.Maui.Handlers
{
	public partial class CheckBoxHandler : ViewHandler<ICheckBox, Gtk.CheckButton>
	{
		protected override Gtk.CheckButton CreatePlatformView()
		{
			var platformCheckBox = new Gtk.CheckButton();

			return platformCheckBox;
		}

		protected override void ConnectHandler(Gtk.CheckButton platformView)
		{
			//platformView.CheckedChange += OnCheckedChange;
		}

		protected override void DisconnectHandler(Gtk.CheckButton platformView)
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

		protected override void RemoveContainer()
		{

		}

		protected override void SetupContainer()
		{

		}
	}
}