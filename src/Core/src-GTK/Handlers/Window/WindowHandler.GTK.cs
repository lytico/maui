using System;
using Microsoft.Maui.Platform;

namespace Microsoft.Maui.Handlers
{
	public partial class WindowHandler : ElementHandler<IWindow, Gtk.Window>
	{
		public static void MapTitle(IWindowHandler handler, IWindow window) { }

		public static void MapContent(IWindowHandler handler, IWindow window)
		{
			_ = handler.MauiContext ?? throw new InvalidOperationException($"{nameof(MauiContext)} should have been set by base class.");

			_ = CreateRootViewFromContent(handler, window);
			//if (rootView != null) {
			//	handler.PlatformView = rootView;
			//}



			//if (window.VisualDiagnosticsOverlay != null && rootView is ViewGroup group)
			//	window.VisualDiagnosticsOverlay.Initialize();

			//// handler.PlatformView.SetContentView(rootView);
			//// handler.PlatformView = rootView;
			////if (window.VisualDiagnosticsOverlay != null && rootView is ViewGroup group)
			////	window.VisualDiagnosticsOverlay.Initialize();


			//_ = handler.MauiContext ?? throw new InvalidOperationException($"{nameof(MauiContext)} should have been set by base class.");

			//var windowManager = handler.MauiContext.GetNavigationRootManager();
			//var previousRootView = windowManager.RootView;

			//windowManager.Disconnect();
			//windowManager.Connect(handler.VirtualView.Content.ToPlatform(handler.MauiContext));

			//if (handler.PlatformView is MauiGTKWindow container)
			//{
			//	if (previousRootView != null && previousRootView != windowManager.RootView)
			//		container.RemovePage(previousRootView);

			//	container.AddPage(windowManager.RootView);
			//}

			//if (window.VisualDiagnosticsOverlay != null)
			//	window.VisualDiagnosticsOverlay.Initialize();
		}

		public static void MapToolbar(IWindowHandler handler, IWindow view)
		{
			if (view is IToolbarElement tb)
				ViewHandler.MapToolbar(handler, tb);
		}

		public static void MapRequestDisplayDensity(IWindowHandler handler, IWindow window, object? args)
		{
			//if (args is DisplayDensityRequest request)
			//	request.SetResult(handler.PlatformView.GetDisplayDensity());
		}

		internal static Gtk.Window? CreateRootViewFromContent(IWindowHandler handler, IWindow window)
		{
			_ = handler.MauiContext ?? throw new InvalidOperationException($"{nameof(MauiContext)} should have been set by base class.");
			var rootManager = handler.MauiContext.GetNavigationRootManager();
			var handlerWin = window.Content.ToHandler(handler.MauiContext);
			if (handler.PlatformView is not Gtk.Window result)
			{
				throw new InvalidOperationException($"Unable to convert {window} to {typeof(Gtk.Window)}");
			}
			rootManager.Connect(window);
			return rootManager.RootView;

			//var winuiWindow = new Gtk.Fixed();

			//foreach(var child in window.all)

			// return winuiWindow;
		}
	}
}