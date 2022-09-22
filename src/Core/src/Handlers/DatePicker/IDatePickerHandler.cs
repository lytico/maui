#if IOS && !MACCATALYST
using PlatformView = Microsoft.Maui.Platform.MauiDatePicker;
#elif MACCATALYST
using PlatformView = UIKit.UIDatePicker;
#elif MONOANDROID
using PlatformView = Microsoft.Maui.Platform.MauiDatePicker;
#elif WINDOWS
#if __GTK__
using PlatformView = Microsoft.Maui.Platform.DatePicker;
#else
using PlatformView = Microsoft.UI.Xaml.Controls.CalendarDatePicker;
#endif
#elif TIZEN
using PlatformView = Tizen.UIExtensions.ElmSharp.Entry;
#elif (NETSTANDARD || !PLATFORM) || (NET6_0_OR_GREATER && !IOS && !ANDROID && !TIZEN)
using PlatformView = System.Object;
#endif

namespace Microsoft.Maui.Handlers
{
#if __GTK__
	public partial interface IDatePickerHandler : IAltViewHandler
#else
	public partial interface IDatePickerHandler : IViewHandler
#endif
	{
		new IDatePicker VirtualView { get; }
		new PlatformView PlatformView { get; }
	}
}