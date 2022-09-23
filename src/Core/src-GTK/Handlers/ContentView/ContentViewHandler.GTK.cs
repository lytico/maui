using System;
using System.Collections.Generic;

namespace Microsoft.Maui.Handlers
{
	public partial class ContentViewHandler : ViewHandler<IContentView, ContentViewGroup>
	{
		protected override ContentViewGroup CreatePlatformView()
		{
			if (VirtualView == null)
			{
				throw new InvalidOperationException($"{nameof(VirtualView)} must be set to create a ContentViewGroup");
			}

			var viewGroup = new ContentViewGroup();

			//viewGroup.SetClipChildren(false);

			return viewGroup;
		}

		public override void SetVirtualView(IView view)
		{
			base.SetVirtualView(view);
			_ = VirtualView ?? throw new InvalidOperationException($"{nameof(VirtualView)} should have been set by base class.");
			_ = PlatformView ?? throw new InvalidOperationException($"{nameof(PlatformView)} should have been set by base class.");

			//PlatformView.CrossPlatformMeasure = VirtualView.CrossPlatformMeasure;
			//PlatformView.CrossPlatformArrange = VirtualView.CrossPlatformArrange;
		}

		static void UpdateContent(IContentViewHandler handler)
		{
			//_ = handler.PlatformView ?? throw new InvalidOperationException($"{nameof(PlatformView)} should have been set by base class.");
			//_ = handler.MauiContext ?? throw new InvalidOperationException($"{nameof(MauiContext)} should have been set by base class.");
			//_ = handler.VirtualView ?? throw new InvalidOperationException($"{nameof(VirtualView)} should have been set by base class.");

			//handler.PlatformView.RemoveAllViews();

			//if (handler.VirtualView.PresentedContent is IView view)
			//	handler.PlatformView.AddView(view.ToPlatform(handler.MauiContext));
		}

		public static void MapContent(IContentViewHandler handler, IContentView page)
		{
			UpdateContent(handler);
		}

		protected override void DisconnectHandler(ContentViewGroup platformView)
		{
			// If we're being disconnected from the xplat element, then we should no longer be managing its chidren
			List<Gtk.Widget> childList = new List<Gtk.Widget>();
			foreach (var child in platformView.Children)
			{
				childList.Add(child);
			}
			foreach (var child in childList)
			{
				platformView.Children.Remove(child);
			}

			base.DisconnectHandler(platformView);
		}
	}
}
