using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Gtk;

namespace Microsoft.Maui.Handlers
{
	public partial class ContentViewHandler : ViewHandler<IContentView, ContentViewGroup>
	{
		protected override ContentViewGroup CreatePlatformView(IView viewGroup)
		{
			if (VirtualView == null)
			{
				throw new InvalidOperationException($"{nameof(VirtualView)} must be set to create a ContentViewGroup");
			}

			var contentViewGroup = new ContentViewGroup();

			//viewGroup.SetClipChildren(false);

			return contentViewGroup;
		}

		public override void SetVirtualView(IView view)
		{
			base.SetVirtualView(view);
			_ = VirtualView ?? throw new InvalidOperationException($"{nameof(VirtualView)} should have been set by base class.");
			_ = PlatformView ?? throw new InvalidOperationException($"{nameof(PlatformView)} should have been set by base class.");

			//PlatformView.CrossPlatformMeasure = VirtualView.CrossPlatformMeasure;
			//PlatformView.CrossPlatformArrange = VirtualView.CrossPlatformArrange;

			if (VirtualView.PresentedContent is IView viewContent)
			{
				if (MauiContext != null)
				{
					var platformChild = viewContent.ToPlatform(MauiContext);
					if (platformChild is Gtk.Widget widget)
					{
						PlatformView.AddChild(widget);
						widget.ShowAll();
					}
					else if (platformChild is Gtk.ScrolledWindow window)
					{
						PlatformView.AddChild(window);
						window.ShowAll();
					}
					else if (platformChild is ContentViewGroup viewGroup)
					{
						if (viewGroup != null)
						{
							var viewGroupChild = viewGroup.GetChild();
							if (PlatformView.GetChild() != viewGroupChild)
							{

								if (viewGroupChild != null)
								{
									PlatformView.AddChild((Gtk.Widget)viewGroupChild);
									viewGroupChild.ShowAll();
								}
							}
							//if (viewGroup.RemoveChildOnly(widge))
							//{
							//	PlatformView.AddChild(viewGroupChild);
							//}
							//PlatformView.AddChild(widge);
						}
					}
				}
			}
		}

		static void UpdateContent(IContentViewHandler handler)
		{
			_ = handler.PlatformView ?? throw new InvalidOperationException($"{nameof(PlatformView)} should have been set by base class.");
			_ = handler.MauiContext ?? throw new InvalidOperationException($"{nameof(MauiContext)} should have been set by base class.");
			_ = handler.VirtualView ?? throw new InvalidOperationException($"{nameof(VirtualView)} should have been set by base class.");

			// handler.PlatformView.RemoveAllViews();

			//if (handler.VirtualView.PresentedContent is IView view)
			//{
			//	var platformChild = view.ToPlatform(handler.MauiContext);
			//	handler.PlatformView.AddChild(platformChild);
			//}
			// handler.PlatformView.Remove();

			//if (handler.PlatformView is Gtk.Container container)
			//{
			//	foreach (var child in container.Children)
			//	{
			//		container.Remove(child);
			//	}
			//}

			//	handler.PlatformView.AddView(view.ToPlatform(handler.MauiContext));
			if (handler.VirtualView.PresentedContent is IView viewContent)
			{
				if (handler.MauiContext != null)
				{
					var platformChild = viewContent.ToPlatform(handler.MauiContext);
					if (platformChild is Gtk.Widget widget)
					{
						handler.PlatformView.AddChild(widget);
						widget.ShowAll();
					}
					else if (platformChild is Gtk.ScrolledWindow window)
					{
						handler.PlatformView.AddChild(window);
						window.ShowAll();
					}
					else if (platformChild is ContentViewGroup viewGroup)
					{
						if (viewGroup != null)
						{
							var viewGroupChild = viewGroup.GetChild();
							if (handler.PlatformView.GetChild() != viewGroupChild)
							{
								if (viewGroupChild != null)
								{
									handler.PlatformView.AddChild((Gtk.Widget)viewGroupChild);
									viewGroupChild.ShowAll();
								}
							}
							//if (viewGroup.RemoveChildOnly(widge))
							//{
							//	PlatformView.AddChild(viewGroupChild);
							//}
							//PlatformView.AddChild(widge);
						}
						//	if (viewGroup.GetChild() != null)
						//{
						//	var widge = viewGroup.GetChild();
						//	var viewGroupChild = viewGroup.GetChild();
						//	if (widge != null && viewGroupChild != null)
						//	{
						//		if (viewGroup.RemoveChildOnly(widge))
						//		{
						//			handler.PlatformView.AddChild(viewGroupChild);
						//		}
						//		handler.PlatformView.AddChild(widge);
						//		widge.ShowAll();
						//	}
						//}
					}
				}
			}
		}


		public static void MapContent(IContentViewHandler handler, IContentView page)
		{
			UpdateContent(handler);
		}

		//protected override void DisconnectHandler(ContentViewGroup platformView)
		//{
		//	// If we're being disconnected from the xplat element, then we should no longer be managing its chidren
		//	List<Gtk.Widget> childList = new List<Gtk.Widget>();
		//	foreach (var child in platformView.Children)
		//	{
		//		childList.Add(child);
		//	}
		//	foreach (var child in childList)
		//	{
		//		platformView.Children.Remove(child);
		//	}

		//	base.DisconnectHandler(platformView);
		//}
	}
}
