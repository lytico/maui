#nullable enable
#if __IOS__ || MACCATALYST
using PlatformView = Microsoft.Maui.Platform.MauiSearchBar;
using QueryEditor = UIKit.UITextField;
#elif MONOANDROID
using PlatformView = AndroidX.AppCompat.Widget.SearchView;
using QueryEditor = Android.Widget.EditText;
#elif WINDOWS
#if __GTK__
using PlatformView = Microsoft.Maui.Platform.SearchEntry;
using QueryEditor = Gtk.Entry;
#else
using PlatformView = Microsoft.UI.Xaml.Controls.AutoSuggestBox;
using QueryEditor = Microsoft.UI.Xaml.Controls.AutoSuggestBox;
#endif
#elif TIZEN
using PlatformView = Tizen.UIExtensions.ElmSharp.SearchBar;
using QueryEditor = Tizen.UIExtensions.ElmSharp.EditfieldEntry;
#elif (NETSTANDARD || !PLATFORM) || (NET6_0_OR_GREATER && !IOS && !ANDROID && !TIZEN)
using PlatformView = System.Object;
using QueryEditor = System.Object;
#endif

namespace Microsoft.Maui.Handlers
{
#if __GTK__
	public partial interface ISearchBarHandler : IAltViewHandler
#else
	public partial interface ISearchBarHandler : IViewHandler
#endif
	{
		new ISearchBar VirtualView { get; }
		new PlatformView PlatformView { get; }
		QueryEditor? QueryEditor { get; }
	}
}