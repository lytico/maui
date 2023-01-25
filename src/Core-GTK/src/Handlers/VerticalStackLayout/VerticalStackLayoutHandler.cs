using System;
using Gtk;

namespace Microsoft.Maui.Handlers
{
	public class VerticalStackLayoutHandler : ViewHandler<ILayout, Gtk.Box>, IVerticalStackLayoutHandler
	{
		protected override Gtk.Box CreatePlatformView(IView layout)
		{
			if (VirtualView == null)
			{
				throw new InvalidOperationException($"{nameof(VirtualView)} must be set to create a LayoutViewGroup");
			}

			// var viewGroup = new LayoutViewGroup();
			var view = new Gtk.Box(Gtk.Orientation.Vertical, 0);

			return view;
		}

		public override void SetVirtualView(IView view)
		{
			base.SetVirtualView(view);

			_ = PlatformView ?? throw new InvalidOperationException($"{nameof(PlatformView)} should have been set by base class.");
			_ = VirtualView ?? throw new InvalidOperationException($"{nameof(VirtualView)} should have been set by base class.");
			_ = MauiContext ?? throw new InvalidOperationException($"{nameof(MauiContext)} should have been set by base class.");

			// PlatformView.RemoveAllViews();

			foreach (var child in VirtualView.OrderByZIndex())
			{
				// var widget = (Gtk.Widget)child.ToPlatform(MauiContext);
				//var hbox = new Gtk.HBox();
				//var vbox = new Gtk.VBox();
				//hbox.PackStart(vbox, false, true, 20);

				//hbox.PackStart((Gtk.Widget)child.ToPlatform(MauiContext), false, true, 0);
				PlatformView.PackStart((Gtk.Widget)child.ToPlatform(MauiContext), false, false, 20);
			}
		}

		public void Add(IView child)
		{
			_ = PlatformView ?? throw new InvalidOperationException($"{nameof(PlatformView)} should have been set by base class.");
			_ = VirtualView ?? throw new InvalidOperationException($"{nameof(VirtualView)} should have been set by base class.");
			_ = MauiContext ?? throw new InvalidOperationException($"{nameof(MauiContext)} should have been set by base class.");

			//var targetIndex = VirtualView.GetLayoutHandlerIndex(child);
			//PlatformView.Add(child.ToPlatform(MauiContext), targetIndex);
			//PlatformView.PackStart(child.ToPlatform(MauiContext));
		}

		public void Remove(IView child)
		{
			_ = PlatformView ?? throw new InvalidOperationException($"{nameof(PlatformView)} should have been set by base class.");
			_ = VirtualView ?? throw new InvalidOperationException($"{nameof(VirtualView)} should have been set by base class.");

			if (child?.ToPlatform() is MauiView view)
			{
				PlatformView.Remove(view);
			}
		}

		void Clear(Gtk.Box platformView)
		{
			//platformView.RemoveAllViews();
		}

		public void Clear()
		{
			//Clear(PlatformView);
		}

		public void Insert(int index, IView child)
		{
			_ = PlatformView ?? throw new InvalidOperationException($"{nameof(PlatformView)} should have been set by base class.");
			_ = VirtualView ?? throw new InvalidOperationException($"{nameof(VirtualView)} should have been set by base class.");
			_ = MauiContext ?? throw new InvalidOperationException($"{nameof(MauiContext)} should have been set by base class.");

			//var targetIndex = VirtualView.GetLayoutHandlerIndex(child);
			//PlatformView.AddView(child.ToPlatform(MauiContext), targetIndex);
			//PlatformView.PackStart(child.ToPlatform(MauiContext));
		}

		public void Update(int index, IView child)
		{
			_ = PlatformView ?? throw new InvalidOperationException($"{nameof(PlatformView)} should have been set by base class.");
			_ = VirtualView ?? throw new InvalidOperationException($"{nameof(VirtualView)} should have been set by base class.");
			_ = MauiContext ?? throw new InvalidOperationException($"{nameof(MauiContext)} should have been set by base class.");

			//PlatformView.RemoveViewAt(index);
			//var targetIndex = VirtualView.GetLayoutHandlerIndex(child);
			//PlatformView.AddView(child.ToPlatform(MauiContext), targetIndex);
		}

		public void UpdateZIndex(IView child)
		{
			_ = PlatformView ?? throw new InvalidOperationException($"{nameof(PlatformView)} should have been set by base class.");
			_ = VirtualView ?? throw new InvalidOperationException($"{nameof(VirtualView)} should have been set by base class.");
			_ = MauiContext ?? throw new InvalidOperationException($"{nameof(MauiContext)} should have been set by base class.");

			EnsureZIndexOrder(child);
		}

		protected override void DisconnectHandler(Gtk.Box platformView)
		{
			// If we're being disconnected from the xplat element, then we should no longer be managing its chidren
			Clear(platformView);
			base.DisconnectHandler(platformView);
		}

		void EnsureZIndexOrder(IView child)
		{
			//if (PlatformView.ChildCount == 0)
			//{
			//	return;
			//}

			//AView platformChildView = child.ToPlatform(MauiContext!);
			//var currentIndex = IndexOf(PlatformView, platformChildView);

			//if (currentIndex == -1)
			//{
			//	return;
			//}

			//var targetIndex = VirtualView.GetLayoutHandlerIndex(child);

			//if (currentIndex != targetIndex)
			//{
			//	PlatformView.RemoveViewAt(currentIndex);
			//	PlatformView.AddView(platformChildView, targetIndex);
			//}
		}

		//static int IndexOf(ViewGroup viewGroup, AView view)
		//{
		//	for (int n = 0; n < viewGroup.ChildCount; n++)
		//	{
		//		if (viewGroup.GetChildAt(n) == view)
		//		{
		//			return n;
		//		}
		//	}

		//	return -1;
		//}

		//static void MapInputTransparent(IVerticalStackLayoutHandler handler, ILayout layout)
		//{
		//	if (handler.PlatformView is LayoutViewGroup layoutViewGroup)
		//	{
		//		layoutViewGroup.InputTransparent = layout.InputTransparent;
		//	}
		//}

		public static IPropertyMapper<ILayout, IVerticalStackLayoutHandler> Mapper = new PropertyMapper<ILayout, IVerticalStackLayoutHandler>(ViewMapper)
		{
			[nameof(ILayout.Background)] = MapBackground,
			[nameof(ILayout.ClipsToBounds)] = MapClipsToBounds,
#if ANDROID || WINDOWS
			[nameof(IView.InputTransparent)] = MapInputTransparent,
#endif
		};

		public static CommandMapper<ILayout, IVerticalStackLayoutHandler> CommandMapper = new(ViewCommandMapper)
		{
			[nameof(IVerticalStackLayoutHandler.Add)] = MapAdd,
			[nameof(IVerticalStackLayoutHandler.Remove)] = MapRemove,
			[nameof(IVerticalStackLayoutHandler.Clear)] = MapClear,
			[nameof(IVerticalStackLayoutHandler.Insert)] = MapInsert,
			[nameof(IVerticalStackLayoutHandler.Update)] = MapUpdate,
			[nameof(IVerticalStackLayoutHandler.UpdateZIndex)] = MapUpdateZIndex,
		};

		public VerticalStackLayoutHandler() : base(Mapper, CommandMapper)
		{
		}

		public VerticalStackLayoutHandler(IPropertyMapper? mapper = null, CommandMapper? commandMapper = null)
			: base(mapper ?? Mapper, commandMapper ?? CommandMapper)
		{

		}

		ILayout IVerticalStackLayoutHandler.VirtualView => VirtualView;

		System.Object IVerticalStackLayoutHandler.PlatformView => PlatformView;

		public static void MapBackground(IVerticalStackLayoutHandler handler, ILayout layout)
		{
#if TIZEN
			handler.UpdateValue(nameof(handler.ContainerView));
			handler.ToPlatform()?.UpdateBackground(layout);
#endif
#if !__GTK__
			((PlatformView?)handler.PlatformView)?.UpdateBackground(layout);
#endif
		}

		public static void MapClipsToBounds(IVerticalStackLayoutHandler handler, ILayout layout)
		{
			//((PlatformView?)handler.PlatformView)?.UpdateClipsToBounds(layout);
		}

		public static void MapAdd(IVerticalStackLayoutHandler handler, ILayout layout, object? arg)
		{
			if (arg is LayoutHandlerUpdate args)
			{
				handler.Add(args.View);
			}
		}

		public static void MapRemove(IVerticalStackLayoutHandler handler, ILayout layout, object? arg)
		{
			if (arg is LayoutHandlerUpdate args)
			{
				handler.Remove(args.View);
			}
		}

		public static void MapInsert(IVerticalStackLayoutHandler handler, ILayout layout, object? arg)
		{
			if (arg is LayoutHandlerUpdate args)
			{
				handler.Insert(args.Index, args.View);
			}
		}

		public static void MapClear(IVerticalStackLayoutHandler handler, ILayout layout, object? arg)
		{
			handler.Clear();
		}

		static void MapUpdate(IVerticalStackLayoutHandler handler, ILayout layout, object? arg)
		{
			if (arg is LayoutHandlerUpdate args)
			{
				handler.Update(args.Index, args.View);
			}
		}

		static void MapUpdateZIndex(IVerticalStackLayoutHandler handler, ILayout layout, object? arg)
		{
			if (arg is IView view)
			{
				handler.UpdateZIndex(view);
			}
		}

		/// <summary>
		/// Converts a FlowDirection to the appropriate FlowDirection for cross-platform layout 
		/// </summary>
		/// <param name="flowDirection"></param>
		/// <returns>The FlowDirection to assume for cross-platform layout</returns>
		internal static FlowDirection GetLayoutFlowDirection(FlowDirection flowDirection)
		{
#if WINDOWS
			// The native LayoutPanel in Windows will automagically flip our layout coordinates if it's in RTL mode.
			// So for cross-platform layout purposes, we just always treat things as being LTR and let the Panel sort out the rest.
			return FlowDirection.LeftToRight;
#else
			return flowDirection;
#endif
		}
	}
}
