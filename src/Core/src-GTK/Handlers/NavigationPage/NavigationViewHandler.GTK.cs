using System;
using Gtk;

namespace Microsoft.Maui.Handlers
{
	public partial class NavigationViewHandler : ViewHandler<IStackNavigationView, MauiView>
	{
		//StackNavigationManager? _stackNavigationManager;
		//internal StackNavigationManager? StackNavigationManager => _stackNavigationManager;

		protected override MauiView CreatePlatformView()
		{
			return new MauiView();
		}

		public override Graphics.Size GetDesiredSize(double widthConstraint, double heightConstraint)
		{
			return base.GetDesiredSize(widthConstraint, heightConstraint);
		}

		public override void PlatformArrange(Graphics.Rect frame)
		{
			base.PlatformArrange(frame);
		}

		protected override void ConnectHandler(MauiView platformView)
		{
			base.ConnectHandler(platformView);
		}

		protected override void DisconnectHandler(MauiView platformView)
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
