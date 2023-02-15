using System;
using System.Runtime.CompilerServices;
using Gtk;
using Microsoft.Maui.Graphics;
using static Microsoft.Maui.Primitives.Dimension;

namespace Microsoft.Maui.Handlers
{
	public partial class ViewHandler<TVirtualView, TPlatformView> : IPlatformViewHandler
	{
		public override void PlatformArrange(Rect frame) =>
			this.PlatformArrangeHandler(frame);

		public override Size GetDesiredSize(double widthConstraint, double heightConstraint) =>
			this.GetDesiredSizeFromHandler(widthConstraint, heightConstraint);

		public void SetMargins(IView view, ref Gtk.Widget widget)
		{
			if (view.Margin.Left > 0)
			{
				widget.MarginStart = (int)view.Margin.Left;
			}
			if (view.Margin.Top > 0)
			{
				widget.MarginTop = (int)view.Margin.Top;
			}
			if (view.Margin.Right > 0)
			{
				widget.MarginEnd = (int)view.Margin.Right;
			}
			if (view.Margin.Bottom > 0)
			{
				widget.MarginBottom = (int)view.Margin.Bottom;
			}

			if (view.Width > 0)
			{
				widget.WidthRequest = (int)view.Width;
			}

			if (view.Height > 0)
			{
				widget.HeightRequest = (int)view.Height;
			}
		}

		public void SetAlignment(IView view, ref Gtk.Widget widget)
		{
			if (view is ILabel virtualLabel)
			{
				if (virtualLabel.HorizontalTextAlignment == TextAlignment.Start)
				{
					if (widget is Gtk.Label widgetLabel)
					{
						// widgetLabel.Halign = Align.Start;
						widgetLabel.Xalign = 0.0f;
						widgetLabel.Justify = Justification.Left;
					}
				}
				else if (virtualLabel.HorizontalTextAlignment == TextAlignment.Center)
				{
					if (widget is Gtk.Label widgetLabel)
					{
						//widgetLabel.Halign = Align.Center;
						widgetLabel.Xalign = 0.5f;
						widgetLabel.Justify = Justification.Center;
					}
				}
				else if (virtualLabel.HorizontalTextAlignment == TextAlignment.End)
				{
					if (widget is Gtk.Label widgetLabel)
					{
						//widgetLabel.Halign = Align.End;
						widgetLabel.Xalign = 1.0f;
						widgetLabel.Justify = Justification.Right;
					}
				}
			}
		}

		protected override void SetupContainer()
		{
		//	if (PlatformView == null || ContainerView != null)
		//		return;

		//	var oldParent = (Gtk.Widget?)PlatformView.Parent;

		//	var oldIndex = oldParent?.IndexOfChild(PlatformView);
		//	oldParent?.RemoveView(PlatformView);

		//	ContainerView ??= new WrapperView(Context);
		//	((ViewGroup)ContainerView).AddView(PlatformView);

		//	if (oldIndex is int idx && idx >= 0)
		//		oldParent?.AddView(ContainerView, idx);
		//	else
		//		oldParent?.AddView(ContainerView);
		}

		protected override void RemoveContainer()
		{
		//	if (Context == null || PlatformView == null || ContainerView == null || PlatformView.Parent != ContainerView)
		//		return;

		//	var oldParent = (ViewGroup?)ContainerView.Parent;

		//	var oldIndex = oldParent?.IndexOfChild(ContainerView);
		//	oldParent?.RemoveView(ContainerView);

		//	((ViewGroup)ContainerView).RemoveAllViews();
		//	ContainerView = null;

		//	if (oldIndex is int idx && idx >= 0)
		//		oldParent?.AddView(PlatformView, idx);
		//	else
		//		oldParent?.AddView(PlatformView);
		}
	}
}
