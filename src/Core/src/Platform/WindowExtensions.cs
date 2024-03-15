using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Maui.Media;

#if __IOS__ || MACCATALYST
using PlatformView = UIKit.UIWindow;
#elif MONOANDROID
using PlatformView = Android.App.Activity;
#elif WINDOWS
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

/* Unmerged change from project 'Core(net8.0)'
Before:
				return new List<IWindow>();

			if (platformApplication.Application is not IApplication application)
After:
			{
*/

/* Unmerged change from project 'Core(net8.0-maccatalyst)'
Before:
				return new List<IWindow>();

			if (platformApplication.Application is not IApplication application)
After:
			{
*/

/* Unmerged change from project 'Core(net8.0-android)'
Before:
				return new List<IWindow>();

			if (platformApplication.Application is not IApplication application)
After:
			{
*/

/* Unmerged change from project 'Core(net8.0-windows10.0.19041.0)'
Before:
				return new List<IWindow>();

			if (platformApplication.Application is not IApplication application)
After:
			{
*/

/* Unmerged change from project 'Core(net8.0-windows10.0.20348.0)'
Before:
				return new List<IWindow>();

			if (platformApplication.Application is not IApplication application)
After:
			{
*/
			{
				return new List<IWindow>();
			}

			if (platformApplication.Application is not IApplication application)
			{
				return new List<IWindow>();
			}
			}

			if (platformApplication.Application is not IApplication application)
			{
				return new List<IWindow>();
			}

			return application.Windows;
		}
#endif
	}
}
