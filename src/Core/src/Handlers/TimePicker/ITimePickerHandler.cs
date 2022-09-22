#if __IOS__ && !MACCATALYST
using PlatformView = Microsoft.Maui.Platform.MauiTimePicker;
#elif MACCATALYST
using PlatformView = UIKit.UIDatePicker;
#elif MONOANDROID
using PlatformView = Microsoft.Maui.Platform.MauiTimePicker;
#elif WINDOWS
#if __GTK__
using PlatformView = Microsoft.Maui.Platform.TimePicker;
#else
using PlatformView = Microsoft.UI.Xaml.Controls.TimePicker;
#endif
#elif TIZEN
using PlatformView = Tizen.UIExtensions.ElmSharp.Entry;
#elif (NETSTANDARD || !PLATFORM) || (NET6_0_OR_GREATER && !IOS && !ANDROID && !TIZEN)
using PlatformView = System.Object;
#endif

namespace Microsoft.Maui.Handlers
{
#if __GTK__
	public partial interface ITimePickerHandler : IAltViewHandler
#else
	public partial interface ITimePickerHandler : IViewHandler
#endif
	{
		new ITimePicker VirtualView { get; }
		new PlatformView PlatformView { get; }
	}
}