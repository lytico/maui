﻿#nullable enable
#if __IOS__ || MACCATALYST
using PlatformView = Microsoft.Maui.Platform.MauiCheckBox;
#elif __ANDROID__
using PlatformView = AndroidX.AppCompat.Widget.AppCompatCheckBox;
#elif WINDOWS
#if __GTK__
using PlatformView = Microsoft.Maui.Platform.MauiView;
#else
using PlatformView = Microsoft.UI.Xaml.Controls.CheckBox;
#endif
#elif TIZEN
using PlatformView = Tizen.UIExtensions.NUI.GraphicsView.CheckBox;
#elif (NETSTANDARD || !PLATFORM)
using PlatformView = System.Object;
#endif

namespace Microsoft.Maui.Handlers
{
	public partial interface ICheckBoxHandler : IViewHandler
	{
		new ICheckBox VirtualView { get; }
		new PlatformView PlatformView { get; }
	}
}