﻿using System.IO;
using System.Threading.Tasks;
#if !__GTK__
using Microsoft.Maui.Media;
#endif

#if __IOS__ || MACCATALYST
using PlatformView = UIKit.UIWindow;
#elif MONOANDROID
using PlatformView = Android.App.Activity;
#elif WINDOWS && __GTK__
using PlatformView = Gdk.Window;
#elif WINDOWS && !__GTK__
using PlatformView = Microsoft.UI.Xaml.Window;
#elif TIZEN
using PlatformView =  Tizen.NUI.Window;
#endif

namespace Microsoft.Maui
{
	public static partial class WindowExtensions
	{
#if !__GTK__
		public static Task<IScreenshotResult?> CaptureAsync(this IWindow window)
		{
#if PLATFORM
			if (window?.Handler?.PlatformView is not PlatformView platformView)
				return Task.FromResult<IScreenshotResult?>(null);

			if (!Screenshot.Default.IsCaptureSupported)
				return Task.FromResult<IScreenshotResult?>(null);

			return CaptureAsync(platformView);
#else
			return Task.FromResult<IScreenshotResult?>(null);
#endif
		}


#if PLATFORM
		async static Task<IScreenshotResult?> CaptureAsync(PlatformView window) =>
			await Screenshot.Default.CaptureAsync(window);
#endif
#endif
	}
}
