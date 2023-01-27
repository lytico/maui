using System;
using Gtk;

namespace Microsoft.Maui.Handlers
{
	public partial class NavigationViewHandler : ViewHandler<IStackNavigationView, ContentViewGroup>
	{
		StackNavigationManager? _stackNavigationManager;
		//internal StackNavigationManager? StackNavigationManager => _stackNavigationManager;

		protected override ContentViewGroup CreatePlatformView(IView navigationView)
		{
			var view = new ContentViewGroup();
			_stackNavigationManager = CreateNavigationManager(view);
			return view;
		}

		public override Graphics.Size GetDesiredSize(double widthConstraint, double heightConstraint)
		{
			return base.GetDesiredSize(widthConstraint, heightConstraint);
		}

		public override void PlatformArrange(Graphics.Rect frame)
		{
			base.PlatformArrange(frame);
		}

		protected override void ConnectHandler(ContentViewGroup platformView)
		{
			_stackNavigationManager?.Connect(VirtualView, platformView);
			base.ConnectHandler(platformView);
		}

		protected override void DisconnectHandler(ContentViewGroup platformView)
		{
			_stackNavigationManager?.Disconnect(VirtualView, platformView);
			base.DisconnectHandler(platformView);
		}

		public static void RequestNavigation(INavigationViewHandler arg1, IStackNavigation arg2, object? arg3)
		{
			if (arg1 is NavigationViewHandler platformHandler && arg3 is NavigationRequest nr)
			{
				platformHandler._stackNavigationManager?.NavigateTo(nr);
			}
			else
			{
				throw new InvalidOperationException("Args must be NavigationRequest");
			}
		}

		// this should move to a factory method
		protected virtual StackNavigationManager CreateNavigationManager(ContentViewGroup view) =>
			_stackNavigationManager ??= new StackNavigationManager(view, MauiContext ?? throw new InvalidOperationException("MauiContext cannot be null"));
	}
}
