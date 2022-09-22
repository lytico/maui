#if __IOS__ || MACCATALYST
using PlatformView = UIKit.UISwitch;
#elif MONOANDROID
using PlatformView = AndroidX.AppCompat.Widget.SwitchCompat;
#elif WINDOWS
#if __GTK__
using PlatformView = Microsoft.Maui.Platform.SwitchControl;
#else
using PlatformView = Microsoft.UI.Xaml.Controls.ToggleSwitch;
#endif
#elif TIZEN
using PlatformView = ElmSharp.Check;
#elif (NETSTANDARD || !PLATFORM) || (NET6_0_OR_GREATER && !IOS && !ANDROID && !TIZEN)
using PlatformView = System.Object;
#endif

namespace Microsoft.Maui.Handlers
{
#if __GTK__
	public partial interface ISwitchHandler : IAltViewHandler
#else
	public partial interface ISwitchHandler : IViewHandler
#endif
	{
		new ISwitch VirtualView { get; }
		new PlatformView PlatformView { get; }
	}
}