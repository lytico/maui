#if __IOS__ || MACCATALYST
using PlatformView = Microsoft.Maui.Platform.MauiPageControl;
#elif MONOANDROID
using PlatformView = Microsoft.Maui.Platform.MauiPageControl;
#elif WINDOWS
using PlatformView = Microsoft.Maui.Platform.MauiPageControl;
#elif TIZEN
using PlatformView = Tizen.UIExtensions.ElmSharp.IndicatorView;
#elif (NETSTANDARD || !PLATFORM) || (NET6_0_OR_GREATER && !IOS && !ANDROID && !TIZEN)
using PlatformView = System.Object;
#endif

namespace Microsoft.Maui.Handlers
{
#if __GTK__
	public partial interface IIndicatorViewHandler : IAltViewHandler
#else
	public partial interface IIndicatorViewHandler : IViewHandler
#endif
	{
		new IIndicatorView VirtualView { get; }
		new PlatformView PlatformView { get; }
	}
}