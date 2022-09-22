#if __IOS__ || MACCATALYST
using PlatformView = UIKit.UIImageView;
#elif MONOANDROID
using PlatformView = Android.Widget.ImageView;
#elif WINDOWS
#if __GTK__
using PlatformView = Microsoft.Maui.Platform.ImageControl;
#else
using PlatformView = Microsoft.UI.Xaml.Controls.Image;
#endif
#elif TIZEN
using PlatformView = Tizen.UIExtensions.ElmSharp.Image;
#elif (NETSTANDARD || !PLATFORM) || (NET6_0_OR_GREATER && !IOS && !ANDROID && !TIZEN)
using PlatformView = System.Object;
#endif

namespace Microsoft.Maui.Handlers
{
#if __GTK__
	public partial interface IImageHandler : IAltViewHandler
#else
	public partial interface IImageHandler : IViewHandler
#endif
	{
		new IImage VirtualView { get; }
		ImageSourcePartLoader SourceLoader { get; }
		new PlatformView PlatformView { get; }
	}
}