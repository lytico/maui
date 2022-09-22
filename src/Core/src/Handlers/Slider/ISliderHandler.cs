#if __IOS__ || MACCATALYST
using PlatformView = UIKit.UISlider;
#elif MONOANDROID
using PlatformView = Android.Widget.SeekBar;
#elif WINDOWS
#if __GTK__
using PlatformView = Microsoft.Maui.Platform.CustomSlider;
#else
using PlatformView = Microsoft.UI.Xaml.Controls.Slider;
#endif
#elif TIZEN
using PlatformView = ElmSharp.Slider;
#elif (NETSTANDARD || !PLATFORM) || (NET6_0_OR_GREATER && !IOS && !ANDROID && !TIZEN)
using PlatformView = System.Object;
#endif

namespace Microsoft.Maui.Handlers
{
#if __GTK__
	public partial interface ISliderHandler : IAltViewHandler
#else
	public partial interface ISliderHandler : IViewHandler
#endif
	{
		new ISlider VirtualView { get; }
		new PlatformView PlatformView { get; }
	}
}