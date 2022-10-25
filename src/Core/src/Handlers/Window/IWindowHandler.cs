#if __IOS__ || MACCATALYST
using PlatformView = UIKit.UIWindow;
#elif MONOANDROID
using PlatformView = Android.App.Activity;
#elif WINDOWS && __GTK__
using PlatformView = Gtk.Window;
#elif WINDOWS && !__GTK__
using PlatformView = Microsoft.UI.Xaml.Window;
#elif TIZEN
using PlatformView = Tizen.NUI.Window;
#elif (NETSTANDARD || !PLATFORM) || (NET6_0_OR_GREATER && !IOS && !ANDROID && !TIZEN)
using PlatformView = System.Object;
#endif

namespace Microsoft.Maui.Handlers
{
	public partial interface IWindowHandler : IElementHandler
	{
		new IWindow VirtualView { get; }
#if WINDOWS && __GTK__
		new PlatformView PlatformView { get; set; }
#else
		new PlatformView PlatformView { get; }
#endif
	}
}
