using System;
using System.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.Controls.Internals;

namespace Microsoft.Maui.Controls.Handlers.Compatibility
{
	public class CellRenderer : ElementHandler<Cell, System.Object>, IRegisterable
	{
		static readonly PropertyChangedEventHandler PropertyChangedHandler = OnGlobalCellPropertyChanged;

		//EventHandler _onForceUpdateSizeRequested;

		public static PropertyMapper<Cell, CellRenderer> Mapper =
				new PropertyMapper<Cell, CellRenderer>(ElementHandler.ElementMapper);

		public static CommandMapper<Cell, CellRenderer> CommandMapper =
			new CommandMapper<Cell, CellRenderer>(ElementHandler.ElementCommandMapper);

		public CellRenderer() : base(Mapper, CommandMapper)
		{
		}

		public View ParentView { get; set; }

		protected Cell Cell { get; set; }

		protected override System.Object CreatePlatformElement()
		{
			return GetCell(VirtualView, null);
		}

		public System.Object GetCell(Cell item, System.Object parent)
		{
			if (item.Parent is View parentView)
				ParentView = parentView;

			if (parent == null && ParentView?.Handler?.PlatformView is MauiView platformParent)
				parent = platformParent;

			Performance.Start(out string reference);

			Cell = item;
			Cell.PropertyChanged -= PropertyChangedHandler;

			System.Object view = GetCellCore(item, parent);

			WireUpForceUpdateSizeRequested(item, view);

			Cell.PropertyChanged += PropertyChangedHandler;
			((ICellController)Cell).SendAppearing();

			Performance.Stop(reference);

			return view;
		}

		protected virtual System.Object GetCellCore(Cell item, System.Object parent)
		{
			Performance.Start(out string reference, "GetCellCore");

			//LayoutInflater inflater = LayoutInflater.FromContext(context);
			//const int type = global::Android.Resource.Layout.SimpleListItem1;
			//AView view = inflater.Inflate(type, null);

			//var textView = view.FindViewById<TextView>(global::Android.Resource.Id.Text1);
			//textView.Text = item.ToString();
			//textView.SetBackgroundColor(global::Android.Graphics.Color.Transparent);
			//view.SetBackgroundColor(global::Android.Graphics.Color.Black);
			var view = new MauiView();

			Performance.Stop(reference, "GetCellCore");

			return view;
		}

		protected virtual void OnCellPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
		}

		protected void WireUpForceUpdateSizeRequested(Cell cell, System.Object platformCell)
		{
			//ICellController cellController = cell;
			//cellController.ForceUpdateSizeRequested -= _onForceUpdateSizeRequested;

			//_onForceUpdateSizeRequested = (sender, e) =>
			//{
			//	if (platformCell.Handle == IntPtr.Zero)
			//		return;
			//	// RenderHeight may not be changed, but that's okay, since we
			//	// don't actually use the height argument in the OnMeasure override.
			//	platformCell.Measure(platformCell.Width, (int)cell.RenderHeight);
			//	platformCell.SetMinimumHeight(platformCell.MeasuredHeight);
			//	platformCell.SetMinimumWidth(platformCell.MeasuredWidth);
			//};

			//cellController.ForceUpdateSizeRequested += _onForceUpdateSizeRequested;
		}

		internal static CellRenderer GetRenderer(Cell cell)
		{
			return (CellRenderer)cell.Handler;
		}

		static void OnGlobalCellPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			var cell = (Cell)sender;
			CellRenderer renderer = GetRenderer(cell);
			if (renderer == null)
			{
				cell.PropertyChanged -= PropertyChangedHandler;
				return;
			}

			renderer.OnCellPropertyChanged(sender, e);
		}

		class RendererHolder : Object
		{
			readonly WeakReference<CellRenderer> _rendererRef;

			public RendererHolder(CellRenderer renderer)
			{
				_rendererRef = new WeakReference<CellRenderer>(renderer);
			}

			public CellRenderer Renderer
			{
				get
				{
					CellRenderer renderer;
					return _rendererRef.TryGetTarget(out renderer) ? renderer : null;
				}
				set { _rendererRef.SetTarget(value); }
			}
		}
	}
}