#nullable enable
#if __IOS__ || MACCATALYST
using PlatformView = Microsoft.Maui.Platform.ContentView;
#elif __ANDROID__
using PlatformView = Microsoft.Maui.Platform.ContentViewGroup;
#elif WINDOWS
#if __GTK__
using PlatformView = Microsoft.Maui.Platform.MauiBorder;
#else
using PlatformView = Microsoft.Maui.Platform.ContentViewGroup;
#endif
#elif TIZEN
using PlatformView = Microsoft.Maui.Platform.BorderView;
#elif (NETSTANDARD || !PLATFORM)
using PlatformView = System.Object;
#endif

namespace Microsoft.Maui.Handlers
{
	public partial interface IBorderHandler : IViewHandler
	{
		new IBorderView VirtualView { get; }
		new PlatformView PlatformView { get; }
	}
}