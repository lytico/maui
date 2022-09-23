﻿#if __IOS__ || MACCATALYST
using PlatformView = Microsoft.Maui.Platform.ContentView;
#elif MONOANDROID
using PlatformView = Android.Views.View;
#elif WINDOWS
#if __GTK__
using PlatformView = Microsoft.Maui.Platform.RadioButton;
#else
using PlatformView = Microsoft.UI.Xaml.Controls.RadioButton;
#endif
#elif TIZEN
using PlatformView = Microsoft.Maui.Platform.MauiRadioButton;
#elif (NETSTANDARD || !PLATFORM) || (NET6_0_OR_GREATER && !IOS && !ANDROID && !TIZEN)
using PlatformView = System.Object;
#endif

namespace Microsoft.Maui.Handlers
{
	public partial interface IRadioButtonHandler : IViewHandler
	{
		new IRadioButton VirtualView { get; }
		new PlatformView PlatformView { get; }
	}
}