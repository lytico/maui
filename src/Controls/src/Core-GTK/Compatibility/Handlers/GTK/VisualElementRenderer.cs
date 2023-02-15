﻿#nullable enable
using System;
using System.ComponentModel;
using Microsoft.Maui.Controls.Platform;
using Microsoft.Maui.Graphics;

namespace Microsoft.Maui.Controls.Handlers.Compatibility
{
	public abstract partial class VisualElementRenderer<TElement> : System.Object, IPlatformViewHandler
		where TElement : Element, IView
	{
		// private IMauiContext? _mauiContext;
		public System.Object? PlatformView => new System.Object();

		public System.Object? ContainerView => new System.Object();

		public bool HasContainer { get; set; }

		public IView? VirtualView => new View();

		// public IMauiContext? MauiContext => _mauiContext;

		object? IElementHandler.PlatformView
		{
			get => this;
		}

		//object? IViewHandler.ContainerView => new MauiView();

		//IElement? IElementHandler.VirtualView => Element;

		// ViewHandlerDelegator<Frame> _viewHandlerWrapper = null!;

		//protected Frame? Element
		//{
		//	get { return _viewHandlerWrapper.Element; }
		//	set
		//	{
		//		if (value != null)
		//			(this as IPlatformViewHandler).SetVirtualView(value);
		//	}
		//}

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
		}

		static partial void ProcessAutoPackage(Maui.IElement element)
		{
			//if (element?.Handler?.PlatformView is not MauiView viewGroup)
			//	return;

			//viewGroup.RemoveAllViews();

			//if (element is not IVisualTreeElement vte)
			//	return;

			//var mauiContext = element?.Handler?.MauiContext;
			//if (mauiContext == null)
			//	return;

			//foreach (var child in vte.GetVisualChildren())
			//{
			//	if (child is Maui.IElement childElement)
			//		viewGroup.Add(childElement.ToPlatform(mauiContext));
			//}
		}

		public void DisconnectHandler()
		{
			//throw new NotImplementedException();
		}

		//public Size GetDesiredSize(double widthConstraint, double heightConstraint)
		//{
		//	//throw new NotImplementedException();
		//	return new Size(widthConstraint, heightConstraint);
		//}

		public void Invoke(string command, object? args = null)
		{
			//throw new NotImplementedException();
		}

		public void PlatformArrange(Rect frame)
		{
			//throw new NotImplementedException();
		}

		public void SetMauiContext(IMauiContext mauiContext)
		{
			_mauiContext = mauiContext;
		}

		public void SetVirtualView(IElement view)
		{
			//throw new NotImplementedException();
		}

		public void UpdateLayout()
		{
			//if (Element != null)
			//	this.InvalidateMeasure(Element);
		}

		public void UpdateValue(string property)
		{
			//throw new NotImplementedException();
		}

		//protected override void OnLayout(bool changed, int l, int t, int r, int b)
		//{
		//	if (ChildCount > 0)
		//	{
		//		var platformView = GetChildAt(0);
		//		if (platformView != null)
		//		{
		//			platformView.Layout(0, 0, r - l, b - t);
		//		}
		//	}
		//}

		//protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
		//{
		//	if (ChildCount > 0)
		//	{
		//		var platformView = GetChildAt(0);
		//		if (platformView != null)
		//		{
		//			platformView.Measure(widthMeasureSpec, heightMeasureSpec);
		//			SetMeasuredDimension(platformView.MeasuredWidth, platformView.MeasuredHeight);
		//			return;
		//		}
		//	}

		//	SetMeasuredDimension(0, 0);
		//}
	}
}