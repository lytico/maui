#if __IOS__ || MACCATALYST || MONOANDROID || WINDOWS || TIZEN
#if __GTK__
using PlatformView = Microsoft.Maui.Platform.CustomView;
#else
using PlatformView = Microsoft.Maui.Platform.PlatformTouchGraphicsView;
#endif
#else
using PlatformView = System.Object;
#endif

namespace Microsoft.Maui.Handlers
{
	public partial interface IGraphicsViewHandler : IViewHandler
	{
		new IGraphicsView VirtualView { get; }
		new PlatformView PlatformView { get; }
	}
}