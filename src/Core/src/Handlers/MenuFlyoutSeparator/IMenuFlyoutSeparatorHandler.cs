#if IOS || MACCATALYST
using PlatformView = UIKit.UIMenu;
#elif WINDOWS && !__GTK__
using PlatformView = Microsoft.UI.Xaml.Controls.MenuFlyoutSeparator;
#elif (NETSTANDARD || !PLATFORM) || (NET6_0_OR_GREATER && !IOS) || __GTK__
using PlatformView = System.Object;
#endif

namespace Microsoft.Maui.Handlers
{
	public interface IMenuFlyoutSeparatorHandler : IElementHandler
	{
		new PlatformView PlatformView { get; }
		new IMenuFlyoutSeparator VirtualView { get; }
	}
}
