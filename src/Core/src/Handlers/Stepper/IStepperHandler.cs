#if __IOS__ || MACCATALYST
using PlatformView = UIKit.UIStepper;
#elif MONOANDROID
using PlatformView = Microsoft.Maui.Platform.MauiStepper;
#elif WINDOWS
#if __GTK__
using PlatformView = Microsoft.Maui.Platform.MauiStepper;
#else
using PlatformView = Microsoft.Maui.Platform.MauiStepper;
#endif
#elif (NETSTANDARD || !PLATFORM) || (NET6_0_OR_GREATER && !IOS && !ANDROID)
using PlatformView = System.Object;
#endif

namespace Microsoft.Maui.Handlers
{
	public partial interface IStepperHandler : IViewHandler
	{
		new IStepper VirtualView { get; }
		new PlatformView PlatformView { get; }
	}
}