﻿using System;
using Microsoft.Maui.Graphics;

namespace Microsoft.Maui
{

	internal static partial class ViewHandlerExtensions
	{

		internal static Size LayoutVirtualView(
			this IPlatformViewHandler viewHandler, Rect frame,
			Func<Rect, Size>? arrangeFunc = null)
		{
			var virtualView = viewHandler.VirtualView;
			var platformView = viewHandler.PlatformView;

			if (virtualView == null || platformView == null)
			{
				return Size.Zero;
			}

			arrangeFunc ??= virtualView.Arrange;

			return arrangeFunc(frame);
		}

		internal static Size MeasureVirtualView(
			this IPlatformViewHandler viewHandler,
			double widthConstraint,
			double heightConstraint,
			Func<double, double, Size>? measureFunc = null)
		{
			var virtualView = viewHandler.VirtualView;
			var platformView = viewHandler.PlatformView;

			if (virtualView == null || platformView == null)
			{
				return Size.Zero;
			}

			measureFunc ??= virtualView.Measure;
			var measure = measureFunc(widthConstraint, heightConstraint);

			return measure;
		}

		internal static Size GetDesiredSizeFromHandler(this IViewHandler viewHandler, double widthConstraint, double heightConstraint)
		{
			var platformView = viewHandler.ToPlatform();
			var virtualView = viewHandler.VirtualView;

			if (platformView == null || virtualView == null)
			{
				return virtualView == null || double.IsNaN(virtualView.Width) || double.IsNaN(virtualView.Height) ? Size.Zero : new Size(virtualView.Width, virtualView.Height);
			}

			double? explicitWidth = (virtualView.Width >= 0) ? virtualView.Width : null;
			double? explicitHeight = (virtualView.Height >= 0) ? virtualView.Height : null;

			Size measured = platformView.GetDesiredSize(explicitWidth ?? widthConstraint, explicitHeight ?? heightConstraint).Minimum;

			return new Size(measured.Width, measured.Height);
		}

		internal static void PlatformArrangeHandler(this IViewHandler viewHandler, Rect frame)
		{
			var platformView = viewHandler.ToPlatform();

			if (platformView == null)
				return;

			if (frame.Width < 0 || frame.Height < 0)
			{
				// This is just some initial Forms value nonsense, nothing is actually laying out yet
				return;
			}

			viewHandler.Invoke(nameof(IView.Frame), frame);
		}

	}

}