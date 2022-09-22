#nullable enable
#if __IOS__ || MACCATALYST
using PlatformView = Microsoft.Maui.Platform.MauiCheckBox;
#elif __ANDROID__
using PlatformView = AndroidX.AppCompat.Widget.AppCompatCheckBox;
#elif WINDOWS
#if __GTK__
using PlatformView = Microsoft.Maui.Platform.CustomAltView;
#else
using PlatformView = Microsoft.UI.Xaml.Controls.CheckBox;
#endif
#elif TIZEN
using PlatformView = ElmSharp.Check;
#elif (NETSTANDARD || !PLATFORM)
using PlatformView = System.Object;
#endif

namespace Microsoft.Maui.Handlers
{
#if __GTK__
	public partial interface ICheckBoxHandler : IAltViewHandler
#else
	public partial interface ICheckBoxHandler : IViewHandler
#endif
	{
		new ICheckBox VirtualView { get; }
		new PlatformView PlatformView { get; }
	}
}