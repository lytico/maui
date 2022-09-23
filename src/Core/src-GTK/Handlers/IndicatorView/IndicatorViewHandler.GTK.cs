using Gtk;

namespace Microsoft.Maui.Handlers
{
	public partial class IndicatorViewHandler : ViewHandler<IIndicatorView, MauiPageControl>
	{
		protected override MauiPageControl CreatePlatformView()
		{
			return new MauiPageControl();
		}

		private protected override void OnConnectHandler(CustomView platformView)
		{
			base.OnConnectHandler(platformView);
			PlatformView.SetIndicatorView(VirtualView);
		}

		private protected override void OnDisconnectHandler(CustomView platformView)
		{
			base.OnDisconnectHandler(platformView);
			PlatformView.SetIndicatorView(null);
		}

		public static void MapCount(IIndicatorViewHandler handler, IIndicatorView indicator)
		{
			handler.PlatformView.UpdateIndicatorCount();
		}

		public static void MapPosition(IIndicatorViewHandler handler, IIndicatorView indicator)
		{
			handler.PlatformView.UpdatePosition();
		}

		public static void MapHideSingle(IIndicatorViewHandler handler, IIndicatorView indicator)
		{
			handler.PlatformView.UpdateIndicatorCount();
		}

		public static void MapMaximumVisible(IIndicatorViewHandler handler, IIndicatorView indicator)
		{
			handler.PlatformView.UpdateIndicatorCount();
		}

		public static void MapIndicatorSize(IIndicatorViewHandler handler, IIndicatorView indicator)
		{
			handler.PlatformView.ResetIndicators();
		}

		public static void MapIndicatorColor(IIndicatorViewHandler handler, IIndicatorView indicator)
		{
			handler.PlatformView.ResetIndicators();
		}
		public static void MapSelectedIndicatorColor(IIndicatorViewHandler handler, IIndicatorView indicator)
		{
			handler.PlatformView.ResetIndicators();
		}
		public static void MapIndicatorShape(IIndicatorViewHandler handler, IIndicatorView indicator)
		{
			handler.PlatformView.ResetIndicators();
		}
	}
}
