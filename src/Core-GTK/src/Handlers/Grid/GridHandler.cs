using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Maui.Handlers
{
	public class GridHandler : ViewHandler<IGridLayout, Gtk.Grid>, IGridHandler
	{
		private int[]? colWidth;
		private int[]? rowHeight;

		protected override Gtk.Grid CreatePlatformView(IView layout)
		{
			if (VirtualView == null)
			{
				throw new InvalidOperationException($"{nameof(VirtualView)} must be set to create a LayoutViewGroup");
			}

			// var bounds = layout.GetWindow().GetBoundingBox();
			// var bounds = VirtualView.Page;

			// var viewGroup = new LayoutViewGroup();
			var width = 0;
			var colCount = 0;
			colWidth = new int[VirtualView.ColumnDefinitions.Count];
			foreach (var col in VirtualView.ColumnDefinitions)
			{
				if (col is IGridColumnDefinition def)
				{
					if (def.Width.GridUnitType == GridUnitType.Absolute)
					{
						colWidth[colCount] = (int)def.Width.Value;
						width += colWidth[colCount];
						colCount++;
					}
				}
			}
			var height = 0;
			var rowCount = 0;
			rowHeight = new int[VirtualView.RowDefinitions.Count];
			foreach (var row in VirtualView.RowDefinitions)
			{
				if (row is IGridRowDefinition def)
				{
					if (def.Height.GridUnitType == GridUnitType.Absolute)
					{
						rowHeight[rowCount] = (int)def.Height.Value;
						height += rowHeight[rowCount];
						rowCount++;
					}
				}
			}
			//var width = VirtualView.Width;
			//var height = VirtualView.Height;
			var view = new Gtk.Grid();
			view.WidthRequest = width;
			view.HeightRequest = height;

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

				// PlatformView.PackStart((Gtk.Widget)child.ToPlatform(MauiContext), false, false, 20);
				int col = VirtualView.GetColumn(child);
				int row = VirtualView.GetRow(child);
				int colSpan = VirtualView.GetColumnSpan(child);
				int rowSpan = VirtualView.GetRowSpan(child);
				var widget = (Gtk.Widget)child.ToPlatform(MauiContext);
				if (colWidth != null && rowHeight != null)
				{
					var colWide = colWidth[col];
					var rowHigh = rowHeight[row];
					if ((VirtualView.ColumnDefinitions.Count > col)
						&& ((VirtualView.ColumnDefinitions[col].Width.GridUnitType == GridUnitType.Auto)
							|| (VirtualView.ColumnDefinitions[col].Width.GridUnitType == GridUnitType.Star)))
					{
						widget.Hexpand = true;
					}
					else
					{
						widget.WidthRequest = colWide;
					}

					if ((VirtualView.RowDefinitions.Count > row)
						&& ((VirtualView.RowDefinitions[row].Height.GridUnitType == GridUnitType.Auto)
							|| (VirtualView.RowDefinitions[row].Height.GridUnitType == GridUnitType.Star)))
					{
						widget.Vexpand = true;
					}
					else
					{
						widget.HeightRequest = rowHigh;
					}
				}
				PlatformView.Attach(widget, col, row, colSpan, rowSpan);
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

		void Clear(Gtk.Grid platformView)
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

		protected override void DisconnectHandler(Gtk.Grid platformView)
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

		//static void MapInputTransparent(IGridHandler handler, ILayout layout)
		//{
		//	if (handler.PlatformView is LayoutViewGroup layoutViewGroup)
		//	{
		//		layoutViewGroup.InputTransparent = layout.InputTransparent;
		//	}
		//}

		public static IPropertyMapper<ILayout, IGridHandler> Mapper = new PropertyMapper<ILayout, IGridHandler>(ViewMapper)
		{
			[nameof(ILayout.Background)] = MapBackground,
			[nameof(ILayout.ClipsToBounds)] = MapClipsToBounds,
#if ANDROID || WINDOWS
			[nameof(IView.InputTransparent)] = MapInputTransparent,
#endif
		};

		public static CommandMapper<ILayout, IGridHandler> CommandMapper = new(ViewCommandMapper)
		{
			[nameof(IGridHandler.Add)] = MapAdd,
			[nameof(IGridHandler.Remove)] = MapRemove,
			[nameof(IGridHandler.Clear)] = MapClear,
			[nameof(IGridHandler.Insert)] = MapInsert,
			[nameof(IGridHandler.Update)] = MapUpdate,
			[nameof(IGridHandler.UpdateZIndex)] = MapUpdateZIndex,
		};

		public GridHandler() : base(Mapper, CommandMapper)
		{
			colWidth = null;
			rowHeight = null;
		}

		public GridHandler(IPropertyMapper? mapper = null, CommandMapper? commandMapper = null)
			: base(mapper ?? Mapper, commandMapper ?? CommandMapper)
		{

		}

		IGridLayout IGridHandler.VirtualView => VirtualView;

		System.Object IGridHandler.PlatformView => PlatformView;

		public static void MapBackground(IGridHandler handler, ILayout layout)
		{
#if TIZEN
			handler.UpdateValue(nameof(handler.ContainerView));
			handler.ToPlatform()?.UpdateBackground(layout);
#endif
#if !__GTK__
			((PlatformView?)handler.PlatformView)?.UpdateBackground(layout);
#endif
		}

		public static void MapClipsToBounds(IGridHandler handler, ILayout layout)
		{
			//((PlatformView?)handler.PlatformView)?.UpdateClipsToBounds(layout);
		}

		public static void MapAdd(IGridHandler handler, ILayout layout, object? arg)
		{
			if (arg is LayoutHandlerUpdate args)
			{
				handler.Add(args.View);
			}
		}

		public static void MapRemove(IGridHandler handler, ILayout layout, object? arg)
		{
			if (arg is LayoutHandlerUpdate args)
			{
				handler.Remove(args.View);
			}
		}

		public static void MapInsert(IGridHandler handler, ILayout layout, object? arg)
		{
			if (arg is LayoutHandlerUpdate args)
			{
				handler.Insert(args.Index, args.View);
			}
		}

		public static void MapClear(IGridHandler handler, ILayout layout, object? arg)
		{
			handler.Clear();
		}

		static void MapUpdate(IGridHandler handler, ILayout layout, object? arg)
		{
			if (arg is LayoutHandlerUpdate args)
			{
				handler.Update(args.Index, args.View);
			}
		}

		static void MapUpdateZIndex(IGridHandler handler, ILayout layout, object? arg)
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
