#if __IOS__ || MACCATALYST
using PlatformView = Microsoft.Maui.Platform.ContentView;
#elif MONOANDROID
using PlatformView = Microsoft.Maui.Platform.ContentViewGroup;
#elif WINDOWS
#if __GTK__
using PlatformView = Microsoft.Maui.Platform.ContentViewGroup;
#else
using PlatformView = Microsoft.Maui.Platform.ContentPanel;
#endif
#elif TIZEN
using PlatformView = Microsoft.Maui.Platform.ContentCanvas;
#elif (NETSTANDARD || !PLATFORM) || (NET6_0_OR_GREATER && !IOS && !ANDROID && !TIZEN)
using PlatformView = System.Object;
#endif

namespace Microsoft.Maui.Handlers
{
#if __GTK__
	public partial interface IContentViewHandler : IAltViewHandler
#else
	public partial interface IContentViewHandler : IViewHandler
#endif
	{
		new IContentView VirtualView { get; }
		new PlatformView PlatformView { get; }
	}
}