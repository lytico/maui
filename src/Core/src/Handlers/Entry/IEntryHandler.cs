#if __IOS__ || MACCATALYST
using PlatformView = Microsoft.Maui.Platform.MauiTextField;
#elif MONOANDROID
using PlatformView = AndroidX.AppCompat.Widget.AppCompatEditText;
#elif WINDOWS
#if __GTK__
using PlatformView = Microsoft.Maui.Platform.EntryWrapper;
#else
using PlatformView = Microsoft.UI.Xaml.Controls.TextBox;
#endif
#elif TIZEN
using PlatformView = Tizen.UIExtensions.ElmSharp.Entry;
#elif (NETSTANDARD || !PLATFORM) || (NET6_0_OR_GREATER && !IOS && !ANDROID && !TIZEN)
using PlatformView = System.Object;
#endif

namespace Microsoft.Maui.Handlers
{
#if __GTK__
	public partial interface IEntryHandler : IAltViewHandler
#else
	public partial interface IEntryHandler : IViewHandler
#endif
	{
		new IEntry VirtualView { get; }
		new PlatformView PlatformView { get; }
	}
}