#if __IOS__ || MACCATALYST
using PlatformView = UIKit.UIButton;
#elif MONOANDROID
using PlatformView = Google.Android.Material.Button.MaterialButton;
<<<<<<< HEAD
#elif WINDOWS
#if __GTK__
using PlatformView = Microsoft.Maui.Platform.GTK.MauiGTKButton;
#else
=======
#elif WINDOWS && __GTK__
using PlatformView = Gtk.Button;
#elif WINDOWS && !__GTK__
>>>>>>> 403b43973 (All native controls now, no drawn controls)
using PlatformView = Microsoft.UI.Xaml.Controls.Button;
#elif TIZEN
using PlatformView = Tizen.UIExtensions.NUI.Button;
#elif (NETSTANDARD || !PLATFORM) || (NET6_0_OR_GREATER && !IOS && !ANDROID && !TIZEN)
using PlatformView = System.Object;
#endif

namespace Microsoft.Maui.Handlers
{
	public partial interface IButtonHandler : IViewHandler
	{
		new IButton VirtualView { get; }
		new PlatformView PlatformView { get; }
		ImageSourcePartLoader ImageSourceLoader { get; }
	}
}
