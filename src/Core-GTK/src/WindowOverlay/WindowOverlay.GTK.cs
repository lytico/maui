using System.Linq;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Graphics.Platform;
using Microsoft.Maui.Graphics.Platforms.GTK;
using Microsoft.Maui.Handlers;

namespace Microsoft.Maui
{
	public partial class WindowOverlay
	{
		PlatformGraphicsView? _graphicsView;
		MauiGTKWindow? _nativeLayer;

		public virtual bool Initialize()
		{
			if (IsPlatformViewInitialized)
				return true;

			if (Window == null)
				return false;

			var platformWindow = Window?.Content?.ToPlatform();
			if (platformWindow == null)
				return false;

			var handler = Window?.Handler as WindowHandler;
			if (handler?.MauiContext == null)
				return false;
			var rootManager = handler.MauiContext.GetNavigationRootManager();
			if (rootManager == null)
				return false;


			//if (handler.PlatformView is not Gtk.EventBox activity)
			//	return false;

			//_nativeActivity = activity;
			_nativeLayer = rootManager.RootView;

			//if (_nativeLayer?.Context == null)
			//	return false;

			//if (_nativeActivity?.WindowManager?.DefaultDisplay == null)
			//	return false;

			// var measuredHeight = _nativeLayer.MeasuredHeight;

			//if (_nativeActivity.Window != null)
			//	_nativeActivity.Window.DecorView.LayoutChange += DecorViewLayoutChange;

			_graphicsView = new PlatformGraphicsView();
			if (_graphicsView == null)
				return false;

			//_graphicsView.Touch += TouchLayerTouch;
			//_nativeLayer.AddView(_graphicsView, 0, new CoordinatorLayout.LayoutParams(CoordinatorLayout.LayoutParams.MatchParent, CoordinatorLayout.LayoutParams.MatchParent));
			//_graphicsView.BringToFront();

			IsPlatformViewInitialized = true;
			return IsPlatformViewInitialized;
		}

		/// <inheritdoc/>
		public void Invalidate()
		{
			//_graphicsView?.Invalidate();
		}

		/// <summary>
		/// Deinitializes the native event hooks and handlers used to drive the overlay.
		/// </summary>
		void DeinitializePlatformDependencies()
		{
			//if (_nativeActivity?.Window != null)
			//	_nativeActivity.Window.DecorView.LayoutChange -= DecorViewLayoutChange;

			_nativeLayer?.Remove(_graphicsView);

			_graphicsView = null;
			IsPlatformViewInitialized = false;
		}

		//void TouchLayerTouch(object? sender, View.TouchEventArgs e)
		//{
		//	if (e?.Event == null)
		//		return;

		//	if (e.Event.Action != MotionEventActions.Down && e.Event.ButtonState != MotionEventButtonState.Primary)
		//		return;

		//	var x = this._nativeLayer?.Context.FromPixels(e.Event.RawX) ?? 0;
		//	var y = this._nativeLayer?.Context.FromPixels(e.Event.RawY) ?? 0;

		//	var point = new Point(x, y);

		//	e.Handled = false;
		//	if (DisableUITouchEventPassthrough)
		//		e.Handled = true;
		//	else if (EnableDrawableTouchHandling)
		//		e.Handled = _windowElements.Any(n => n.Contains(point));

		//	OnTappedInternal(point);
		//}

		//void DecorViewLayoutChange(object? sender, View.LayoutChangeEventArgs e)
		//{
		//	HandleUIChange();
		//	Invalidate();
		//}
	}
}