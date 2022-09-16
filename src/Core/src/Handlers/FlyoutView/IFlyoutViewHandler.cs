#if __IOS__ || MACCATALYST
using PlatformView = UIKit.UIView;
#elif MONOANDROID
using PlatformView = Android.Views.View;
#elif WINDOWS
#if __GTK__
using PlatformView = Gtk.Fixed;
#else
using PlatformView = Microsoft.Maui.Platform.RootNavigationView;
#endif
#elif (NETSTANDARD || !PLATFORM) || (NET6_0_OR_GREATER && !IOS && !ANDROID)
using PlatformView = System.Object;
#endif

namespace Microsoft.Maui.Handlers
{
	public partial interface IFlyoutViewHandler : IViewHandler
	{
		new IFlyoutView VirtualView { get; }
		new PlatformView PlatformView { get; }
	}
}