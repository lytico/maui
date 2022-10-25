#if __IOS__ || MACCATALYST
using PlatformView = Microsoft.Maui.Platform.MauiTextView;
#elif MONOANDROID
using PlatformView = AndroidX.AppCompat.Widget.AppCompatEditText;
#elif WINDOWS
#if __GTK__
using PlatformView = Microsoft.Maui.Platform.ScrolledTextView;
#else
using PlatformView = Microsoft.UI.Xaml.Controls.TextBox;
#endif
#elif TIZEN
using PlatformView = Tizen.UIExtensions.NUI.Editor;
#elif (NETSTANDARD || !PLATFORM) || (NET6_0_OR_GREATER && !IOS && !ANDROID && !TIZEN)
using PlatformView = System.Object;
#endif

namespace Microsoft.Maui.Handlers
{
	public partial interface IEditorHandler : IViewHandler
	{
		new IEditor VirtualView { get; }
		new PlatformView PlatformView { get; }
	}
}