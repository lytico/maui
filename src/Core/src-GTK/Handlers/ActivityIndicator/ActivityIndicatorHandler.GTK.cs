using System;
using Cairo;
using Gdk;
using Gtk;

namespace Microsoft.Maui.Handlers
{
	public partial class ActivityIndicatorHandler : ViewHandler<IActivityIndicator, ActivityIndicator>
	{
		//public override bool NeedsContainer =>
		//	VirtualView?.Background != null ||
		//	base.NeedsContainer;

		protected override ActivityIndicator CreatePlatformView()
		{
			var eventBox = new ActivityIndicator();
			return eventBox;
		}

		public static void MapBackground(IActivityIndicatorHandler handler, IActivityIndicator activityIndicator)
		{
			handler.UpdateValue(nameof(IViewHandler.ContainerView));
			//handler.PlatformView?.UpdateBackground(activityIndicator);
		}

		public static void MapIsRunning(IActivityIndicatorHandler handler, IActivityIndicator activityIndicator)
		{
			if (activityIndicator.IsRunning)
			{
				((ActivityIndicator)handler.PlatformView)?.Start();
			}
			else
			{
				((ActivityIndicator)handler.PlatformView)?.Stop();
			}
		}

		public static void MapColor(IActivityIndicatorHandler handler, IActivityIndicator activityIndicator)
		{
			((ActivityIndicator)handler.PlatformView)?.UpdateColor(activityIndicator.Color);
		}

		public static void MapWidth(IActivityIndicatorHandler handler, IActivityIndicator activityIndicator)
		{
			//if (handler.PlatformView is ActivityIndicator platformView)
			//{
			//	platformView.UpdateWidth(activityIndicator);
			//}
		}

		public static void MapHeight(IActivityIndicatorHandler handler, IActivityIndicator activityIndicator)
		{
			//if (handler.PlatformView is ActivityIndicator platformView)
			//{
			//	platformView.UpdateHeight(activityIndicator);
			//}
		}

		public override Graphics.Size GetDesiredSize(double widthConstraint, double heightConstraint)
		{
			if (PlatformView != null)
			{
				return new Graphics.Size((double)PlatformView.HeightRequest, (double)PlatformView.WidthRequest);
			}

			return new Graphics.Size(48.0, 48.0);
		}

		public override void PlatformArrange(Graphics.Rect frame)
		{
		}

		protected override void ConnectHandler(ActivityIndicator platformView)
		{
			base.ConnectHandler(platformView);
		}

		protected override void DisconnectHandler(ActivityIndicator platformView)
		{
			base.DisconnectHandler(platformView);
		}

		//private protected override void OnConnectHandler(MauiView platformView)
		//{
		//	//base.OnConnectHandler(platformView);
		//}

		//private protected override void OnDisconnectHandler(MauiView platformView)
		//{
		//	//base.OnDisconnectHandler(platformView);
		//}

		//private protected override void OnConnectHandler(EventBox platformView)
		//{
		//}

		//private protected override void OnDisconnectHandler(EventBox platformView)
		//{
		//}

		//// new IView? VirtualView { get; }

		//public override void SetMauiContext(IMauiContext mauiContext) { }

		//// public override void SetVirtualView(IElement view) { }

		//public override void UpdateValue(string property) { }

		//public override void Invoke(string command, object? args = null) { }

		//protected override void DisconnectHandler(ActivityIndicator platformView)
		//{
		//	base.DisconnectHandler(platformView);
		//}

		//// public override void DisconnectHandler() { }

		//// object? PlatformView { get; }

		//// IElement? VirtualView { get; }

		//// IMauiContext? MauiContext { get; }
	}
}