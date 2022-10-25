using System;
using Microsoft.Extensions.DependencyInjection;
#if __IOS__ || MACCATALYST
using PlatformView = UIKit.UIWindow;
#elif MONOANDROID
using PlatformView = Android.App.Activity;
#elif WINDOWS && __GTK__
using PlatformView = Gtk.Window;
#elif WINDOWS && !__GTK__
using PlatformView = Microsoft.UI.Xaml.Window;
#elif TIZEN      
using PlatformView = Tizen.NUI.Window;
#endif

namespace Microsoft.Maui.Handlers
{
	public partial class WindowHandler : IWindowHandler
	{
		public static IPropertyMapper<IWindow, IWindowHandler> Mapper = new PropertyMapper<IWindow, IWindowHandler>(ElementHandler.ElementMapper)
		{
			[nameof(IWindow.Title)] = MapTitle,
			[nameof(IWindow.Content)] = MapContent,
#if !__GTK__
			[nameof(IWindow.X)] = MapX,
			[nameof(IWindow.Y)] = MapY,
			[nameof(IWindow.Width)] = MapWidth,
			[nameof(IWindow.Height)] = MapHeight,
#endif
#if (WINDOWS && !__GTK__) || MACCATALYST
			[nameof(IWindow.MaximumWidth)] = MapMaximumWidth,
			[nameof(IWindow.MaximumHeight)] = MapMaximumHeight,
			[nameof(IWindow.MinimumWidth)] = MapMinimumWidth,
			[nameof(IWindow.MinimumHeight)] = MapMinimumHeight,
#endif
#if __GTK__
#else
#if ANDROID || WINDOWS || TIZEN
			[nameof(IToolbarElement.Toolbar)] = MapToolbar,
#endif
#if WINDOWS || IOS
			[nameof(IMenuBarElement.MenuBar)] = MapMenuBar,
#endif
#if WINDOWS
			[nameof(IWindow.FlowDirection)] = MapFlowDirection,
#endif
#endif
		};

		public static CommandMapper<IWindow, IWindowHandler> CommandMapper = new(ElementCommandMapper)
		{
			[nameof(IWindow.RequestDisplayDensity)] = MapRequestDisplayDensity,
		};

		public WindowHandler()
			: base(Mapper, CommandMapper)
		{
		}

		public WindowHandler(IPropertyMapper? mapper)
			: base(mapper ?? Mapper, CommandMapper)
		{
		}

		public WindowHandler(IPropertyMapper? mapper, CommandMapper? commandMapper)
			: base(mapper ?? Mapper, commandMapper ?? CommandMapper)
		{
		}

		protected override PlatformView CreatePlatformElement()
		{
			var plat = MauiContext?.Services.GetService<PlatformView>();
			if (plat != null)
			{
				return plat;
			}
//			else
//			{
//#if !(NETSTANDARD || !PLATFORM)
//				var rawPlat = IPlatformApplication.Current;
//				if (rawPlat != null)
//				{
//					return (PlatformView)rawPlat;
//				}
//#else
//				var rawPlat = IWindow
//				if (rawPlat != null)
//				{
//					return (PlatformView)rawPlat;
//				}
//#endif
//			}
			throw new InvalidOperationException($"MauiContext did not have a valid window.");
		}


//#if !(NETSTANDARD || !PLATFORM) || __GTK__
//		protected override PlatformView CreatePlatformElement() =>
//			MauiContext?.Services.GetService<PlatformView>() ?? throw new InvalidOperationException($"MauiContext did not have a valid window.");
//#endif
	}
}