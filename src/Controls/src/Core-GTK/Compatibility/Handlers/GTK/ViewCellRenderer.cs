using System;
using System.Linq;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls.Internals;
using Microsoft.Maui.Graphics;

namespace Microsoft.Maui.Controls.Handlers.Compatibility
{
	public class ViewCellRenderer : CellRenderer
	{
		protected override System.Object GetCellCore(Cell item, System.Object parent)
		{
			Performance.Start(out string reference, "GetCellCore");
			var cell = (ViewCell)item;

			//var container = convertView as ViewCellContainer;
			//if (container != null)
			//{
			//	container.Update(cell);
			//	Performance.Stop(reference, "GetCellCore");
			//	return container;
			//}

			BindableProperty unevenRows = null, rowHeight = null;
			if (ParentView is TableView)
			{
				unevenRows = TableView.HasUnevenRowsProperty;
				rowHeight = TableView.RowHeightProperty;
			}
			else if (ParentView is ListView)
			{
				//cell.IsContextActionsLegacyModeEnabled = item.On<PlatformConfiguration.Android>().GetIsContextActionsLegacyModeEnabled();

				unevenRows = ListView.HasUnevenRowsProperty;
				rowHeight = ListView.RowHeightProperty;
			}

			if (cell.View == null)
				throw new InvalidOperationException($"ViewCell must have a {nameof(cell.View)}");

			var view = (IPlatformViewHandler)cell.View.ToHandler(cell.FindMauiContext());
			cell.View.IsPlatformEnabled = true;

			//ViewCellContainer c = view.PlatformView.GetParentOfType<ViewCellContainer>();

			//if (c != null)
			//	return c;

			var c = new ViewCellContainer((IPlatformViewHandler)cell.View.Handler, cell, ParentView, unevenRows, rowHeight);

			Performance.Stop(reference, "GetCellCore");

			return c;
		}

		protected override void DisconnectHandler(System.Object platformView)
		{
			base.DisconnectHandler(platformView);
			//ViewCellContainer c = platformView.GetParentOfType<ViewCellContainer>();
			//c?.DisconnectHandler();
		}

		internal class ViewCellContainer : System.Object, INativeElementView
		{
			readonly View _parent;
			readonly BindableProperty _rowHeight;
			readonly BindableProperty _unevenRows;
			IPlatformViewHandler _viewHandler;
			ViewCell _viewCell;
			//GestureDetector _tapGestureDetector;
			//GestureDetector _longPressGestureDetector;
			//ListViewRenderer _listViewRenderer;
			//bool _watchForLongPress;
			//MauiView _currentView;

			//ListViewRenderer ListViewRenderer
			//{
			//	get
			//	{
			//		if (_listViewRenderer != null)
			//		{
			//			return _listViewRenderer;
			//		}

			//		var listView = _parent as ListView;

			//		if (listView == null)
			//		{
			//			return null;
			//		}

			//		_listViewRenderer = listView.Handler as ListViewRenderer;

			//		return _listViewRenderer;
			//	}
			//}

			//GestureDetector TapGestureDetector
			//{
			//	get
			//	{
			//		if (_tapGestureDetector != null)
			//		{
			//			return _tapGestureDetector;
			//		}

			//		_tapGestureDetector = new GestureDetector(Context, new TapGestureListener(TriggerClick));
			//		return _tapGestureDetector;
			//	}
			//}

			//GestureDetector LongPressGestureDetector
			//{
			//	get
			//	{
			//		if (_longPressGestureDetector != null)
			//		{
			//			return _longPressGestureDetector;
			//		}

			//		_longPressGestureDetector = new GestureDetector(Context, new LongPressGestureListener(TriggerLongClick));
			//		return _longPressGestureDetector;
			//	}
			//}

			public ViewCellContainer()
			{
				// Added default constructor to prevent crash when accessing selected row in ListViewAdapter.Dispose
			}

			public ViewCellContainer(IPlatformViewHandler view, ViewCell viewCell, View parent,
				BindableProperty unevenRows, BindableProperty rowHeight)
			{
				_viewHandler = (IPlatformViewHandler)view;
				_parent = parent;
				_unevenRows = unevenRows;
				_rowHeight = rowHeight;
				_viewCell = viewCell;
				// Add(view.PlatformView);
				UpdateIsEnabled();
				UpdateWatchForLongPress();
			}

			protected bool ParentHasUnevenRows
			{
				get { return (bool)_parent.GetValue(_unevenRows); }
			}

			protected int ParentRowHeight
			{
				get { return (int)_parent.GetValue(_rowHeight); }
			}

			public Element Element
			{
				get { return _viewCell; }
			}

			//public override bool OnInterceptTouchEvent(MotionEvent ev)
			//{
			//	if (!Enabled)
			//		return true;

			//	return base.OnInterceptTouchEvent(ev);
			//}

			//public override bool DispatchTouchEvent(MotionEvent e)
			//{
			//	// Give the child controls a shot at the event (in case they've get Tap gestures and such
			//	var handled = base.DispatchTouchEvent(e);

			//	if (_watchForLongPress)
			//	{
			//		// Feed the gesture through the LongPress detector; for this to work we *must* return true 
			//		// afterward (or the LPGD goes nuts and immediately fires onLongPress)
			//		LongPressGestureDetector.OnTouchEvent(e);
			//		return true;
			//	}

			//	if (WatchForSwipeViewTap())
			//	{
			//		TapGestureDetector.OnTouchEvent(e);
			//		return true;
			//	}

			//	return handled;
			//}

			public void Update(ViewCell cell)
			{
				// This cell could have a handler that was used for the measure pass for the Listview height calculations
				//cell.View.Handler.DisconnectHandler();

				Performance.Start(out string reference);
				var viewHandlerType = _viewHandler.MauiContext.Handlers.GetHandlerType(cell.View.GetType());
				var reflectableType = _viewHandler as System.Reflection.IReflectableType;
				var rendererType = reflectableType != null ? reflectableType.GetTypeInfo().AsType() : (_viewHandler != null ? _viewHandler.GetType() : typeof(System.Object));
				if (_viewHandler != null && rendererType == viewHandlerType)
				{
					Performance.Start(reference, "Reuse");
					_viewCell = cell;

					Performance.Start(reference, "Reuse.SetElement");

					if (_viewHandler != cell.View.Handler)
					{
						if (cell.View.Handler?.PlatformView is MauiView oldCellView &&
							oldCellView.GetParentOfType<ViewCellContainer>() is ViewCellContainer vc)
						{
							vc.DisconnectHandler();
						}

						//var oldView = _currentView ?? _viewHandler.PlatformView;
						//if (oldView != null)
						//	Remove(oldView);

						cell.View.Handler?.DisconnectHandler();
						_viewHandler.SetVirtualView(cell.View);
						//Add(_viewHandler.PlatformView);
					}

					Performance.Stop(reference, "Reuse.SetElement");

					//Invalidate();

					Performance.Stop(reference, "Reuse");
					Performance.Stop(reference);
					return;
				}

				//Remove(_currentView ?? _viewHandler.PlatformView);
				_viewCell.View.Handler?.DisconnectHandler();
				_viewCell.View.IsPlatformEnabled = false;

				_viewHandler.DisconnectHandler();

				_viewCell = cell;

				//var platformView = _viewCell.View.ToPlatform(Element.FindMauiContext());
				//_viewHandler = (IPlatformViewHandler)_viewCell.View.Handler;
				//Add(platformView);

				UpdateIsEnabled();
				UpdateWatchForLongPress();

				Performance.Stop(reference);
			}

			public void UpdateIsEnabled()
			{
				//Enabled = _parent.IsEnabled && _viewCell.IsEnabled;
			}

			public void DisconnectHandler()
			{
				//var oldView = _currentView ?? _viewHandler.PlatformView;
				//if (oldView != null)
				//	Remove(oldView);

				_viewCell?.View?.Handler?.DisconnectHandler();

			}

			//public override void AddView(MauiView child)
			//{
			//	if (child.Parent is WrapperView wrapperView)
			//	{
			//		base.Add(wrapperView);
			//		_currentView = wrapperView;
			//	}
			//	else
			//	{
			//		base.AddView(child);
			//		_currentView = child;
			//	}
			//}

			//protected override void OnLayout(bool changed, int l, int t, int r, int b)
			//{
			//	if (_viewHandler.PlatformView == null || Context == null)
			//	{
			//		return;
			//	}

			//	_viewHandler.LayoutVirtualView(l, t, r, b);
			//}

			//protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
			//{
			//	Performance.Start(out string reference);

			//	int width = MeasureSpec.GetSize(widthMeasureSpec);
			//	int height;

			//	if (ParentHasUnevenRows)
			//	{
			//		if (_viewHandler.PlatformView == null)
			//		{
			//			SetMeasuredDimension(0, 0);
			//			return;
			//		}

			//		var size = _viewHandler.MeasureVirtualView(widthMeasureSpec, heightMeasureSpec);
			//		height = (int)size.Height;
			//	}
			//	else
			//	{
			//		height = (int)Context.ToPixels(ParentRowHeight == -1 ? BaseCellView.DefaultMinHeight : ParentRowHeight);
			//		var size = _viewHandler.MeasureVirtualView(widthMeasureSpec, MeasureSpec.MakeMeasureSpec(height, MeasureSpecMode.Exactly));
			//		width = (int)size.Width;
			//	}

			//	SetMeasuredDimension(width, height);

			//	Performance.Stop(reference);
			//}

			//bool WatchForSwipeViewTap()
			//{
			//	if (!(_viewHandler.VirtualView is SwipeView swipeView))
			//	{
			//		return false;
			//	}
			//	// If the cell contains a SwipeView, we will have conflicts capturing the touch.
			//	// So we need to watch locally for Tap and if we see it (and the SwipeView is open),
			//	// trigger the Click manually.
			//	if (!((ISwipeViewController)swipeView).IsOpen)
			//	{
			//		return true;
			//	}

			//	return false;
			//}

			void UpdateWatchForLongPress()
			{
				//var vw = _viewHandler.VirtualView as Microsoft.Maui.Controls.View;
				//if (vw == null)
				//{
				//	return;
				//}

				//// If the view cell has any context actions and the View itself has any Tap Gestures, they're going
				//// to conflict with one another - the Tap Gesture handling will prevent the ListViewAdapter's
				//// LongClick handling from happening. So we need to watch locally for LongPress and if we see it,
				//// trigger the LongClick manually.
				//_watchForLongPress = _viewCell.ContextActions.Count > 0
				//	&& HasTapGestureRecognizers(vw);
			}

			static bool HasTapGestureRecognizers(View view)
			{
				return view.GestureRecognizers.Any(t => t is TapGestureRecognizer)
					|| ((IElementController)view).LogicalChildren.OfType<View>().Any(HasTapGestureRecognizers);
			}

			void TriggerClick()
			{
				//ListViewRenderer?.ClickOn(this);
			}

			void TriggerLongClick()
			{
				//ListViewRenderer?.LongClickOn(this);
			}

			//internal class TapGestureListener : Java.Lang.Object, GestureDetector.IOnGestureListener
			//{
			//	readonly Action _onClick;

			//	internal TapGestureListener(Action onClick)
			//	{
			//		_onClick = onClick;
			//	}

			//	internal TapGestureListener(IntPtr handle, JniHandleOwnership ownership) : base(handle, ownership)
			//	{
			//	}

			//	public bool OnDown(MotionEvent e)
			//	{
			//		return true;
			//	}

			//	public bool OnFling(MotionEvent e1, MotionEvent e2, float velocityX, float velocityY)
			//	{
			//		return false;
			//	}

			//	public void OnLongPress(MotionEvent e)
			//	{

			//	}

			//	public bool OnScroll(MotionEvent e1, MotionEvent e2, float distanceX, float distanceY)
			//	{
			//		return false;
			//	}

			//	public void OnShowPress(MotionEvent e)
			//	{

			//	}

			//	public bool OnSingleTapUp(MotionEvent e)
			//	{
			//		_onClick();
			//		return false;
			//	}
			//}

			//internal class LongPressGestureListener : Java.Lang.Object, GestureDetector.IOnGestureListener
			//{
			//	readonly Action _onLongClick;

			//	internal LongPressGestureListener(Action onLongClick)
			//	{
			//		_onLongClick = onLongClick;
			//	}

			//	internal LongPressGestureListener(IntPtr handle, JniHandleOwnership ownership) : base(handle, ownership)
			//	{
			//	}

			//	public bool OnDown(MotionEvent e)
			//	{
			//		return true;
			//	}

			//	public bool OnFling(MotionEvent e1, MotionEvent e2, float velocityX, float velocityY)
			//	{
			//		return false;
			//	}

			//	public void OnLongPress(MotionEvent e)
			//	{
			//		_onLongClick();
			//	}

			//	public bool OnScroll(MotionEvent e1, MotionEvent e2, float distanceX, float distanceY)
			//	{
			//		return false;
			//	}

			//	public void OnShowPress(MotionEvent e)
			//	{

			//	}

			//	public bool OnSingleTapUp(MotionEvent e)
			//	{
			//		return false;
			//	}
			//}
		}
	}
}