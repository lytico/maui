﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Devices;
using UIKit;

namespace Microsoft.Maui.Platform
{
	public static partial class WindowExtensions
	{
		internal static void UpdateTitle(this UIWindow platformWindow, IWindow window)
		{
			// If you set the title to null the app will crash
			// If you set it to an empty string the title reverts back to the 
			// default app title.
			if (OperatingSystem.IsIOSVersionAtLeast(13) && platformWindow.WindowScene is not null)
			{
				platformWindow.WindowScene.Title = window.Title ?? String.Empty;
			}
		}

		internal static void UpdateX(this UIWindow platformWindow, IWindow window) =>
			platformWindow.UpdateUnsupportedCoordinate(window);

		internal static void UpdateY(this UIWindow platformWindow, IWindow window) =>
			platformWindow.UpdateUnsupportedCoordinate(window);

		internal static void UpdateWidth(this UIWindow platformWindow, IWindow window) =>
			platformWindow.UpdateUnsupportedCoordinate(window);

		internal static void UpdateHeight(this UIWindow platformWindow, IWindow window) =>
			platformWindow.UpdateUnsupportedCoordinate(window);

		internal static void UpdateUnsupportedCoordinate(this UIWindow platformWindow, IWindow window) =>
			window.FrameChanged(platformWindow.Bounds.ToRectangle());

		public static void UpdateMaximumWidth(this UIWindow platformWindow, IWindow window) =>
			platformWindow.UpdateMaximumSize(window.MaximumWidth, window.MaximumHeight);

		public static void UpdateMaximumHeight(this UIWindow platformWindow, IWindow window) =>
			platformWindow.UpdateMaximumSize(window.MaximumWidth, window.MaximumHeight);

		public static void UpdateMaximumSize(this UIWindow platformWindow, IWindow window) =>
			platformWindow.UpdateMaximumSize(window.MaximumWidth, window.MaximumHeight);

		internal static void UpdateMaximumSize(this UIWindow platformWindow, double width, double height)
		{
			if (!OperatingSystem.IsIOSVersionAtLeast(13))
			{
				return;
			}

			var restrictions = platformWindow.WindowScene?.SizeRestrictions;
			if (restrictions is null)
			{
				return;
			}

			if (!Primitives.Dimension.IsExplicitSet(width) || !Primitives.Dimension.IsMaximumSet(width))
			{
				width = double.MaxValue;
			}

			if (!Primitives.Dimension.IsExplicitSet(height) || !Primitives.Dimension.IsMaximumSet(height))
			{
				height = double.MaxValue;
			}

			restrictions.MaximumSize = new CoreGraphics.CGSize(width, height);
		}

		public static void UpdateMinimumWidth(this UIWindow platformWindow, IWindow window) =>
			platformWindow.UpdateMinimumSize(window.MinimumWidth, window.MinimumHeight);

		public static void UpdateMinimumHeight(this UIWindow platformWindow, IWindow window) =>
			platformWindow.UpdateMinimumSize(window.MinimumWidth, window.MinimumHeight);

		public static void UpdateMinimumSize(this UIWindow platformWindow, IWindow window) =>
			platformWindow.UpdateMinimumSize(window.MinimumWidth, window.MinimumHeight);

		internal static void UpdateMinimumSize(this UIWindow platformWindow, double width, double height)
		{
			if (!OperatingSystem.IsIOSVersionAtLeast(13))
			{
				return;
			}

			var restrictions = platformWindow.WindowScene?.SizeRestrictions;
			if (restrictions is null)
			{
				return;
			}

			if (!Primitives.Dimension.IsExplicitSet(width) || !Primitives.Dimension.IsMinimumSet(width))
			{
				width = 0;
			}

			if (!Primitives.Dimension.IsExplicitSet(height) || !Primitives.Dimension.IsMinimumSet(height))
			{
				height = 0;
			}

			restrictions.MinimumSize = new CoreGraphics.CGSize(width, height);
		}

		internal static IWindow? GetHostedWindow(this UIWindow? uiWindow)
		{
			if (uiWindow is null)

/* Unmerged change from project 'Core(net8.0-maccatalyst)'
Before:
				return null;

			var windows = WindowExtensions.GetWindows();
			foreach (var window in windows)
			{

				if (window.Handler?.PlatformView is UIWindow win)
				{
					if (win == uiWindow)
						return window;
After:
			{
				return null;
			}

			var windows = WindowExtensions.GetWindows();
			foreach (var window in windows)
			{

				if (window.Handler?.PlatformView is UIWindow win)
				{
					if (win == uiWindow)
					{
						return window;
					}
*/
			{
				return null;
			}

			var windows = WindowExtensions.GetWindows();
			foreach (var window in windows)
			{

				if (window.Handler?.PlatformView is UIWindow win)
				{
					if (win == uiWindow)
					{
						return window;
					}
				}
			}

			return null;
		}

		public static float GetDisplayDensity(this UIWindow uiWindow) =>
			(float)(uiWindow.Screen?.Scale ?? new nfloat(1.0f));

		internal static DisplayOrientation GetOrientation(this IWindow? window) =>
			DeviceDisplay.Current.MainDisplayInfo.Orientation;
	}
}
