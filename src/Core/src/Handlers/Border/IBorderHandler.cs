#nullable enable
#if __IOS__ || MACCATALYST
using PlatformView = Microsoft.Maui.Platform.ContentView;
#elif __ANDROID__
using PlatformView = Microsoft.Maui.Platform.ContentViewGroup;
#elif WINDOWS
#if __GTK__
using PlatformView = Microsoft.Maui.Platform.CustomBorder;
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
#if __GTK__
	public partial interface IBorderHandler : IAltViewHandler
#else
	public partial interface IBorderHandler : IViewHandler
#endif
	{
		new IBorderView VirtualView { get; }
		new PlatformView PlatformView { get; }
	}
}