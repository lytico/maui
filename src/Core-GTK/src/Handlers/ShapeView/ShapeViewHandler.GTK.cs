#if __IOS__ || MACCATALYST
using PlatformView = Microsoft.Maui.Platform.MauiShapeView;
#elif MONOANDROID
using PlatformView = Microsoft.Maui.Platform.MauiShapeView;
#elif WINDOWS && !__GTK__
using PlatformView = Microsoft.Maui.Graphics.Win2D.W2DGraphicsView;
#elif WINDOWS && __GTK__
using PlatformView = Gtk.Box;
#elif TIZEN
using PlatformView = Microsoft.Maui.Platform.MauiShapeView;
#elif (NETSTANDARD || !PLATFORM) || (NET6_0_OR_GREATER && !IOS && !ANDROID && !TIZEN)
using PlatformView = System.Object;
#endif

namespace Microsoft.Maui.Handlers
{
	public partial class ShapeViewHandler : ViewHandler<IShapeView, Gtk.Box>, IShapeViewHandler
	{
		public static IPropertyMapper<IShapeView, IShapeViewHandler> Mapper = new PropertyMapper<IShapeView, IShapeViewHandler>(ViewHandler.ViewMapper)
		{
			[nameof(IShapeView.Background)] = MapBackground,
			[nameof(IShapeView.Shape)] = MapShape,
			[nameof(IShapeView.Aspect)] = MapAspect,
			[nameof(IShapeView.Fill)] = MapFill,
			[nameof(IShapeView.Stroke)] = MapStroke,
			[nameof(IShapeView.StrokeThickness)] = MapStrokeThickness,
			[nameof(IShapeView.StrokeDashPattern)] = MapStrokeDashPattern,
			[nameof(IShapeView.StrokeDashOffset)] = MapStrokeDashOffset,
			[nameof(IShapeView.StrokeLineCap)] = MapStrokeLineCap,
			[nameof(IShapeView.StrokeLineJoin)] = MapStrokeLineJoin,
			[nameof(IShapeView.StrokeMiterLimit)] = MapStrokeMiterLimit
		};

		public static CommandMapper<IShapeView, IShapeViewHandler> CommandMapper = new(ViewCommandMapper)
		{
		};

		public ShapeViewHandler() : base(Mapper)
		{
		}

		public ShapeViewHandler(IPropertyMapper? mapper)
			: base(mapper ?? Mapper, CommandMapper)
		{
		}

		public ShapeViewHandler(IPropertyMapper? mapper, CommandMapper? commandMapper)
			: base(mapper ?? Mapper, commandMapper ?? CommandMapper)
		{
		}

		IShapeView IShapeViewHandler.VirtualView => VirtualView;

		PlatformView IShapeViewHandler.PlatformView => PlatformView;

		protected override Gtk.Box CreatePlatformView(IView button)
		{
			var plat = new Gtk.Box(Gtk.Orientation.Vertical, 0);
			Gtk.Widget widget = (Gtk.Widget)plat;
			SetMargins(button, ref widget);
			return plat;
		}
		//protected override Gtk.Box CreatePlatformView()
		//	=> new Gtk.Box(Gtk.Orientation.Vertical, 0);

		public override bool NeedsContainer =>
			VirtualView?.Background != null ||
			base.NeedsContainer;

		public static void MapBackground(IShapeViewHandler handler, IShapeView shapeView)
		{
			handler.UpdateValue(nameof(IViewHandler.ContainerView));
			//handler.ToPlatform().UpdateBackground(shapeView);

			//handler.PlatformView?.InvalidateShape(shapeView);
		}

		public static void MapShape(IShapeViewHandler handler, IShapeView shapeView)
		{
			//handler.PlatformView?.UpdateShape(shapeView);
		}

		public static void MapAspect(IShapeViewHandler handler, IShapeView shapeView)
		{
			//handler.PlatformView?.InvalidateShape(shapeView);
		}

		public static void MapFill(IShapeViewHandler handler, IShapeView shapeView)
		{
			//handler.PlatformView?.InvalidateShape(shapeView);
		}

		public static void MapStroke(IShapeViewHandler handler, IShapeView shapeView)
		{
			//handler.PlatformView?.InvalidateShape(shapeView);
		}

		public static void MapStrokeThickness(IShapeViewHandler handler, IShapeView shapeView)
		{
			//handler.PlatformView?.InvalidateShape(shapeView);
		}

		public static void MapStrokeDashPattern(IShapeViewHandler handler, IShapeView shapeView)
		{
			//handler.PlatformView?.InvalidateShape(shapeView);
		}

		public static void MapStrokeDashOffset(IShapeViewHandler handler, IShapeView shapeView)
		{
			//handler.PlatformView?.InvalidateShape(shapeView);
		}

		public static void MapStrokeLineCap(IShapeViewHandler handler, IShapeView shapeView)
		{
			//handler.PlatformView?.InvalidateShape(shapeView);
		}

		public static void MapStrokeLineJoin(IShapeViewHandler handler, IShapeView shapeView)
		{
			//handler.PlatformView?.InvalidateShape(shapeView);
		}

		public static void MapStrokeMiterLimit(IShapeViewHandler handler, IShapeView shapeView)
		{
			//handler.PlatformView?.InvalidateShape(shapeView);
		}
	}
}