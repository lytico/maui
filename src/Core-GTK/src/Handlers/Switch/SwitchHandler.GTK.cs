using Gtk;
using Microsoft.Maui.Graphics;

namespace Microsoft.Maui.Handlers
{
	public partial class SwitchHandler : ViewHandler<ISwitch, MauiSwitch>
	{
		protected override MauiSwitch CreatePlatformView(IView switchView)
		{
			var aSwitch = new MauiSwitch();
			aSwitch.SwitchWidget.Toggled += ASwitch_Toggled;

			return aSwitch;
		}

		protected override void ConnectHandler(MauiSwitch platformView)
		{
			base.ConnectHandler(platformView);
		}

		protected override void DisconnectHandler(MauiSwitch platformView)
		{
			base.DisconnectHandler(platformView);
		}

		public override Size GetDesiredSize(double widthConstraint, double heightConstraint)
		{
			Size size = base.GetDesiredSize(widthConstraint, heightConstraint);

			if (size.Width == 0)
			{
				int width = (int)widthConstraint;

				if (widthConstraint <= 0)
					width = 30;

				//width = Context != null ? (int)Context.GetThemeAttributeDp(global::Android.Resource.Attribute.SwitchMinWidth) : 0;

				size = new Size(width, size.Height);
			}

			return size;
		}

		public static void MapIsOn(ISwitchHandler handler, ISwitch view)
		{
			if (handler.PlatformView != null)
			{
				view.IsOn = handler.PlatformView.SwitchWidget.Active;
			}
		}

		public static void MapTrackColor(ISwitchHandler handler, ISwitch view)
		{
			//if (handler.PlatformView != null)
			//{
			//	view.IsOn = handler.PlatformView.Active;
			//}

			//if (handler is SwitchHandler platformHandler)
			//	handler.PlatformView?.UpdateTrackColor(view);
		}

		public static void MapThumbColor(ISwitchHandler handler, ISwitch view)
		{
			//if (handler is SwitchHandler platformHandler)
			//	handler.PlatformView?.UpdateThumbColor(view);
		}

		private void ASwitch_Toggled(object? sender, System.EventArgs e)
		{
			if (sender is MauiSwitch aSwitch)
			{
				if (VirtualView is null || VirtualView.IsOn == aSwitch.SwitchWidget.Active)
					return;

				VirtualView.IsOn = aSwitch.SwitchWidget.Active;
			}
		}

		//void OnCheckedChanged(bool isOn)
		//{
		//	if (VirtualView is null || VirtualView.IsOn == isOn)
		//		return;

		//	VirtualView.IsOn = isOn;
		//}
	}
}