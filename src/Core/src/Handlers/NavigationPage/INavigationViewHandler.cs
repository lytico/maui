#if __IOS__ || MACCATALYST
using PlatformView = UIKit.UIView;
#elif MONOANDROID
using PlatformView = Android.Views.View;
#elif WINDOWS
#if __GTK__
using PlatformView = Microsoft.Maui.Platform.CustomAltView;
#else
using PlatformView = Microsoft.UI.Xaml.Controls.Frame;
#endif
#elif TIZEN
using PlatformView = ElmSharp.Naviframe;
#elif (NETSTANDARD || !PLATFORM) || (NET6_0_OR_GREATER && !IOS && !ANDROID && !TIZEN)
using PlatformView = System.Object;
#endif

namespace Microsoft.Maui.Handlers
{
#if __GTK__
	public partial interface INavigationViewHandler : IAltViewHandler
#else
	public partial interface INavigationViewHandler : IViewHandler
#endif
	{
		new IStackNavigationView VirtualView { get; }
		new PlatformView PlatformView { get; }
	}
}