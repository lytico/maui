using Microsoft.Maui.Graphics.Platform;

namespace Microsoft.Maui.Handlers
{
	public partial class GraphicsViewHandler : ViewHandler<IGraphicsView, Gtk.Widget>
	{
		protected override Gtk.Widget CreatePlatformView() => new Gtk.Widget();

		public static void MapDrawable(IGraphicsViewHandler handler, IGraphicsView graphicsView)
		{
			//handler.PlatformView?.UpdateDrawable(graphicsView);
		}

		public static void MapFlowDirection(IGraphicsViewHandler handler, IGraphicsView graphicsView)
		{
			//handler.PlatformView?.UpdateFlowDirection(graphicsView);
			//handler.PlatformView?.Invalidate();
		}

		public static void MapInvalidate(IGraphicsViewHandler handler, IGraphicsView graphicsView, object? arg)
		{
			//handler.PlatformView?.Invalidate();
		}

		protected override void SetupContainer()
		{
		}

		protected override void RemoveContainer()
		{
		}
	}
}