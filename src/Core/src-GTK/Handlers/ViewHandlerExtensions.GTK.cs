using System;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Platform;
using PlatformView = Microsoft.Maui.Platform.MauiView;

namespace Microsoft.Maui
{
	internal static partial class ViewHandlerExtensions
	{
		// TODO: Possibly reconcile this code with LayoutViewGroup.OnLayout
		// If you make changes here please review if those changes should also
		// apply to LayoutViewGroup.OnLayout
		internal static Size LayoutVirtualView(
			this IPlatformViewHandler viewHandler,
			int l, int t, int r, int b,
			Func<Rect, Size>? arrangeFunc = null)
		{
			var virtualView = viewHandler.VirtualView;
			var platformView = viewHandler.PlatformView;

			if (virtualView == null || platformView == null)
			{
				return Size.Zero;
			}

			var destination = Rect.FromLTRB(0, 0, r - l, b - t);

			// var destination = new Rect(l, t, r, b); // context.ToCrossPlatformRectInReferenceFrame(l, t, r, b);
			arrangeFunc ??= virtualView.Arrange;
			return arrangeFunc(destination);
		}

		// TODO: Possibly reconcile this code with LayoutViewGroup.OnMeasure
		// If you make changes here please review if those changes should also
		// apply to LayoutViewGroup.OnMeasure
		internal static Size MeasureVirtualView(
			this IPlatformViewHandler viewHandler,
			int platformWidthConstraint,
			int platformHeightConstraint,
			Func<double, double, Size>? measureFunc = null)
		{
			var virtualView = viewHandler.VirtualView;
			var platformView = viewHandler.PlatformView;

			if (virtualView == null || platformView == null)
			{
				return Size.Zero;
			}

			//var deviceIndependentWidth = platformWidthConstraint.ToDouble(context);
			//var deviceIndependentHeight = platformHeightConstraint.ToDouble(context);

			//var widthMode = MeasureSpec.GetMode(platformWidthConstraint);
			//var heightMode = MeasureSpec.GetMode(platformHeightConstraint);

			measureFunc ??= virtualView.Measure;
			var measure = measureFunc(platformWidthConstraint, platformHeightConstraint);

			//// If the measure spec was exact, we should return the explicit size value, even if the content
			//// measure came out to a different size
			//var width = widthMode == MeasureSpecMode.Exactly ? deviceIndependentWidth : measure.Width;
			//var height = heightMode == MeasureSpecMode.Exactly ? deviceIndependentHeight : measure.Height;

			//var platformWidth = context.ToPixels(width);
			//var platformHeight = context.ToPixels(height);

			//// Minimum values win over everything
			//platformWidth = Math.Max(platformView.MinimumWidth, platformWidth);
			//platformHeight = Math.Max(platformView.MinimumHeight, platformHeight);

			return new Size(measure.Width, measure.Height);
		}

		internal static Size GetDesiredSizeFromHandler(this IViewHandler viewHandler, double widthConstraint, double heightConstraint)
		{
			var platformView = viewHandler.ToPlatform();
			var virtualView = viewHandler.VirtualView;

			if (platformView == null || virtualView == null)
			{
				return Size.Zero;
			}

			//// Create a spec to handle the native measure
			//var widthSpec = Context.CreateMeasureSpec(widthConstraint, virtualView.Width, virtualView.MaximumWidth);
			//var heightSpec = Context.CreateMeasureSpec(heightConstraint, virtualView.Height, virtualView.MaximumHeight);

			//var packed = PlatformInterop.MeasureAndGetWidthAndHeight(platformView, widthSpec, heightSpec);
			//var measuredWidth = (int)(packed >> 32);
			//var measuredHeight = (int)(packed & 0xffffffffL);

			// Convert back to xplat sizes for the return value
			// return Context.FromPixels(measuredWidth, measuredHeight);
			return new Size(widthConstraint, heightConstraint);
		}

		internal static void PlatformArrangeHandler(this IViewHandler viewHandler, Rect frame)
		{
			var platformView = viewHandler.ToPlatform();

			var MauiContext = viewHandler.MauiContext;

			if (platformView == null || MauiContext == null)
			{
				return;
			}

			if (frame.Width < 0 || frame.Height < 0)
			{
				// This is a legacy layout value from Controls, nothing is actually laying out yet so we just ignore it
				return;
			}

			//var left = Context.ToPixels(frame.Left);
			//var top = Context.ToPixels(frame.Top);
			//var bottom = Context.ToPixels(frame.Bottom);
			//var right = Context.ToPixels(frame.Right);

			//platformView.Layout((int)left, (int)top, (int)right, (int)bottom);

			viewHandler.Invoke(nameof(IView.Frame), frame);
		}
	}
}
