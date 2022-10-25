using Microsoft.Maui.Graphics;
using System.Threading.Tasks;
#if !__GTK__
using Microsoft.Maui.Media;
#endif
using System.IO;
#if (NETSTANDARD || !PLATFORM) || (NET6_0_OR_GREATER && !IOS && !ANDROID && !TIZEN)
using IPlatformViewHandler = Microsoft.Maui.IViewHandler;
#endif
#if IOS || MACCATALYST
using PlatformView = UIKit.UIView;
using ParentView = UIKit.UIView;
#elif ANDROID
using PlatformView = Android.Views.View;
using ParentView = Android.Views.IViewParent;
#elif WINDOWS
#if __GTK__
using PlatformView = Microsoft.Maui.Platform.MauiView;
using ParentView = Microsoft.Maui.Platform.MauiView;
#else
using PlatformView = Microsoft.UI.Xaml.FrameworkElement;
using ParentView = Microsoft.UI.Xaml.DependencyObject;
#endif
#elif TIZEN
using PlatformView = Tizen.NUI.BaseComponents.View;
using ParentView = Tizen.NUI.BaseComponents.View;
#else
using PlatformView = System.Object;
using ParentView = System.Object;
using System;
#endif

namespace Microsoft.Maui
{
	public static partial class ViewExtensions
	{
#if !__GTK__
		public static Task<IScreenshotResult?> CaptureAsync(this IView view)
		{
#if PLATFORM
			if (view?.ToPlatform() is not PlatformView platformView)
				return Task.FromResult<IScreenshotResult?>(null);

			if (!Screenshot.Default.IsCaptureSupported)
				return Task.FromResult<IScreenshotResult?>(null);

			return CaptureAsync(platformView);
#else
			return Task.FromResult<IScreenshotResult?>(null);
#endif
		}
#endif


#if PLATFORM && !__GTK__
		async static Task<IScreenshotResult?> CaptureAsync(PlatformView window) =>
			await Screenshot.Default.CaptureAsync(window);
#endif
	}
}
