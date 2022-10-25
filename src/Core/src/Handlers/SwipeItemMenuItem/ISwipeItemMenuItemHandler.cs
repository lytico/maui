﻿#if __IOS__ || MACCATALYST
using PlatformView = UIKit.UIButton;
#elif MONOANDROID
using PlatformView = Android.Views.View;
#elif WINDOWS && !__GTK__
using PlatformView = Microsoft.UI.Xaml.Controls.SwipeItem;
#elif TIZEN
using PlatformView = Tizen.UIExtensions.NUI.Button;
#elif (NETSTANDARD || !PLATFORM) || (NET6_0_OR_GREATER && !IOS && !ANDROID && !TIZEN) || __GTK__
using PlatformView = System.Object;
#endif

namespace Microsoft.Maui.Handlers
{
	public partial interface ISwipeItemMenuItemHandler : IElementHandler
	{
		new ISwipeItemMenuItem VirtualView { get; }
		new PlatformView PlatformView { get; }
	}
}