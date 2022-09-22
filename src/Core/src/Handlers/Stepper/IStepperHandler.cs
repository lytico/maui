#if __IOS__ || MACCATALYST
using PlatformView = UIKit.UIStepper;
#elif MONOANDROID
using PlatformView = Microsoft.Maui.Platform.MauiStepper;
#elif WINDOWS
#if __GTK__
using PlatformView = Microsoft.Maui.Platform.StepperControl;
#else
using PlatformView = Microsoft.Maui.Platform.MauiStepper;
#endif
#elif (NETSTANDARD || !PLATFORM) || (NET6_0_OR_GREATER && !IOS && !ANDROID)
using PlatformView = System.Object;
#endif

namespace Microsoft.Maui.Handlers
{
#if __GTK__
	public partial interface IStepperHandler : IAltViewHandler
#else
	public partial interface IStepperHandler : IViewHandler
#endif
	{
		new IStepper VirtualView { get; }
		new PlatformView PlatformView { get; }
	}
}