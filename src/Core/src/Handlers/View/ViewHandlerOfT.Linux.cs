using System;
using Gdk;
using Rectangle = Microsoft.Maui.Graphics.Rectangle;
using Size = Microsoft.Maui.Graphics.Size;

namespace Microsoft.Maui.Handlers
{
	public partial class ViewHandler<TVirtualView, TNativeView> : INativeViewHandler
	{
		Gtk.Widget? INativeViewHandler.NativeView => (Gtk.Widget?)base.NativeView;

		public override void SetFrame(Rectangle rect)
		{

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