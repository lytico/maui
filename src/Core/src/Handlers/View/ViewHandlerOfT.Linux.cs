﻿using System;
using Gdk;
using Microsoft.Maui.Graphics.Native.Gtk;
using Rectangle = Microsoft.Maui.Graphics.Rectangle;
using Size = Microsoft.Maui.Graphics.Size;

namespace Microsoft.Maui.Handlers
{
	public partial class ViewHandler<TVirtualView, TNativeView> : INativeViewHandler
	{
		Gtk.Widget? INativeViewHandler.NativeView => (Gtk.Widget?)base.NativeView;

		public override void SetFrame(Rectangle rect)
		{
			var nativeView = NativeView;

			if (nativeView == null)
				return;

			if (rect.Width < 0 || rect.Height < 0)
				return;

			var allocation = rect.ToNative();
			nativeView.WidthRequest = allocation.Width;
			nativeView.HeightRequest = allocation.Height;
			nativeView.QueueResize();
			
			//nativeView.Arrange(new Windows.Foundation.Rect(rect.X, rect.Y, rect.Width, rect.Height));
		}

		public override Size GetDesiredSize(double widthConstraint, double heightConstraint)
		{
			return new(widthConstraint, heightConstraint);
		}

		protected override void SetupContainer()
		{

		}

		protected override void RemoveContainer()
		{

		}

		protected void InvokeEvent(Action action)
		{
			MauiGtkApplication.Invoke(action);
		}

	}
}