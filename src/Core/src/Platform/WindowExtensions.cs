using System.Collections.Generic;
using System.IO;
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
#endif

namespace Microsoft.Maui.Platform
{
	public static partial class WindowExtensions
	{
#if !NETSTANDARD2_0
		internal static IReadOnlyList<IWindow> GetWindows()
		{
			if (IPlatformApplication.Current is not IPlatformApplication platformApplication)
				return new List<IWindow>();

			if (platformApplication.Application is not IApplication application)
				return new List<IWindow>();

			return application.Windows;
		}
#endif
	}
}
