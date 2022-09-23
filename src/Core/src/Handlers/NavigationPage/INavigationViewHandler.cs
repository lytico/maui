﻿#if __IOS__ || MACCATALYST
using PlatformView = UIKit.UIView;
#elif MONOANDROID
using PlatformView = Android.Views.View;
#elif WINDOWS
#if __GTK__
using PlatformView = Microsoft.Maui.Platform.CustomView;
#else
using PlatformView = Microsoft.UI.Xaml.Controls.Frame;
#endif
#elif TIZEN
using PlatformView = ElmSharp.Naviframe;
#elif (NETSTANDARD || !PLATFORM) || (NET6_0_OR_GREATER && !IOS && !ANDROID && !TIZEN)
using PlatformView = System.Object;
#endif

namespace Microsoft.Maui.Handlers
{
	public partial interface INavigationViewHandler : IViewHandler
	{
		new IStackNavigationView VirtualView { get; }
		new PlatformView PlatformView { get; }
	}
}