#if __IOS__ || MACCATALYST
using PlatformView = UIKit.UIScrollView;
#elif MONOANDROID
using PlatformView = Microsoft.Maui.Platform.MauiScrollView;
#elif WINDOWS
#if __GTK__
using PlatformView = Gtk.ScrolledWindow;
#else
using PlatformView = Microsoft.UI.Xaml.Controls.ScrollViewer;
#endif
#elif TIZEN
using PlatformView = Tizen.UIExtensions.ElmSharp.ScrollView;
#elif (NETSTANDARD || !PLATFORM) || (NET6_0_OR_GREATER && !IOS && !ANDROID && !TIZEN)
using PlatformView = System.Object;
#endif

namespace Microsoft.Maui.Handlers
{
	public partial interface IScrollViewHandler : IViewHandler
	{
		new IScrollView VirtualView { get; }
		new PlatformView PlatformView { get; }
	}
}