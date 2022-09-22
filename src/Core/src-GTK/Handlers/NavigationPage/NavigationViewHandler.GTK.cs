using System;
using Gtk;

namespace Microsoft.Maui.Handlers
{
	public partial class NavigationViewHandler : AltViewHandler<IStackNavigationView, CustomAltView>
	{
		//StackNavigationManager? _stackNavigationManager;
		//internal StackNavigationManager? StackNavigationManager => _stackNavigationManager;

		protected override CustomAltView CreatePlatformView()
		{
			return new CustomAltView();
		}

		public override Graphics.Size GetDesiredSize(double widthConstraint, double heightConstraint)
		{
			return base.GetDesiredSize(widthConstraint, heightConstraint);
		}

		public override void PlatformArrange(Graphics.Rect frame)
		{
			base.PlatformArrange(frame);
		}

		protected override void ConnectHandler(CustomAltView platformView)
		{
			base.ConnectHandler(platformView);
		}

		protected override void DisconnectHandler(CustomAltView platformView)
		{
			base.DisconnectHandler(platformView);
		}

		public static void RequestNavigation(INavigationViewHandler arg1, IStackNavigation arg2, object? arg3)
		{
			//if (arg1 is NavigationViewHandler platformHandler && arg3 is NavigationRequest ea)
			//	platformHandler._stackNavigationManager?.RequestNavigation(ea);
		}
	}
}
