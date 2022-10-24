using System;
using Gtk;

namespace Microsoft.Maui.Handlers
{
	public partial class LayoutHandler : ViewHandler<ILayout, Gtk.VBox>
	{
		protected override Gtk.VBox CreatePlatformView()
		{
			if (VirtualView == null)
			{
				throw new InvalidOperationException($"{nameof(VirtualView)} must be set to create a LayoutViewGroup");
			}

			// var viewGroup = new LayoutViewGroup();
			var view = new Gtk.VBox();

			return view;
		}

		public override void SetVirtualView(IView view)
		{
			base.SetVirtualView(view);

			_ = PlatformView ?? throw new InvalidOperationException($"{nameof(PlatformView)} should have been set by base class.");
			_ = VirtualView ?? throw new InvalidOperationException($"{nameof(VirtualView)} should have been set by base class.");
			_ = MauiContext ?? throw new InvalidOperationException($"{nameof(MauiContext)} should have been set by base class.");

			// PlatformView.RemoveAllViews();

			foreach (var child in VirtualView.OrderByZIndex())
			{
				PlatformView.PackStart((Gtk.Widget)child.ToPlatform(MauiContext), true, false, 20);
			}
		}

		public void Add(IView child)
		{
			_ = PlatformView ?? throw new InvalidOperationException($"{nameof(PlatformView)} should have been set by base class.");
			_ = VirtualView ?? throw new InvalidOperationException($"{nameof(VirtualView)} should have been set by base class.");
			_ = MauiContext ?? throw new InvalidOperationException($"{nameof(MauiContext)} should have been set by base class.");

			//var targetIndex = VirtualView.GetLayoutHandlerIndex(child);
			//PlatformView.Add(child.ToPlatform(MauiContext), targetIndex);
			//PlatformView.PackStart(child.ToPlatform(MauiContext));
		}

		public void Remove(IView child)
		{
			_ = PlatformView ?? throw new InvalidOperationException($"{nameof(PlatformView)} should have been set by base class.");
			_ = VirtualView ?? throw new InvalidOperationException($"{nameof(VirtualView)} should have been set by base class.");

			if (child?.ToPlatform() is MauiView view)
			{
				PlatformView.Remove(view);
			}
		}

		void Clear(Gtk.VBox platformView)
		{
			//platformView.RemoveAllViews();
		}

		public void Clear()
		{
			//Clear(PlatformView);
		}

		public void Insert(int index, IView child)
		{
			_ = PlatformView ?? throw new InvalidOperationException($"{nameof(PlatformView)} should have been set by base class.");
			_ = VirtualView ?? throw new InvalidOperationException($"{nameof(VirtualView)} should have been set by base class.");
			_ = MauiContext ?? throw new InvalidOperationException($"{nameof(MauiContext)} should have been set by base class.");

			//var targetIndex = VirtualView.GetLayoutHandlerIndex(child);
			//PlatformView.AddView(child.ToPlatform(MauiContext), targetIndex);
			//PlatformView.PackStart(child.ToPlatform(MauiContext));
		}

		public void Update(int index, IView child)
		{
			_ = PlatformView ?? throw new InvalidOperationException($"{nameof(PlatformView)} should have been set by base class.");
			_ = VirtualView ?? throw new InvalidOperationException($"{nameof(VirtualView)} should have been set by base class.");
			_ = MauiContext ?? throw new InvalidOperationException($"{nameof(MauiContext)} should have been set by base class.");

			//PlatformView.RemoveViewAt(index);
			//var targetIndex = VirtualView.GetLayoutHandlerIndex(child);
			//PlatformView.AddView(child.ToPlatform(MauiContext), targetIndex);
		}

		public void UpdateZIndex(IView child)
		{
			_ = PlatformView ?? throw new InvalidOperationException($"{nameof(PlatformView)} should have been set by base class.");
			_ = VirtualView ?? throw new InvalidOperationException($"{nameof(VirtualView)} should have been set by base class.");
			_ = MauiContext ?? throw new InvalidOperationException($"{nameof(MauiContext)} should have been set by base class.");

			EnsureZIndexOrder(child);
		}

		protected override void DisconnectHandler(Gtk.VBox platformView)
		{
			// If we're being disconnected from the xplat element, then we should no longer be managing its chidren
			Clear(platformView);
			base.DisconnectHandler(platformView);
		}

		void EnsureZIndexOrder(IView child)
		{
			//if (PlatformView.ChildCount == 0)
			//{
			//	return;
			//}

			//AView platformChildView = child.ToPlatform(MauiContext!);
			//var currentIndex = IndexOf(PlatformView, platformChildView);

			//if (currentIndex == -1)
			//{
			//	return;
			//}

			//var targetIndex = VirtualView.GetLayoutHandlerIndex(child);

			//if (currentIndex != targetIndex)
			//{
			//	PlatformView.RemoveViewAt(currentIndex);
			//	PlatformView.AddView(platformChildView, targetIndex);
			//}
		}

		//static int IndexOf(ViewGroup viewGroup, AView view)
		//{
		//	for (int n = 0; n < viewGroup.ChildCount; n++)
		//	{
		//		if (viewGroup.GetChildAt(n) == view)
		//		{
		//			return n;
		//		}
		//	}

		//	return -1;
		//}

		//static void MapInputTransparent(ILayoutHandler handler, ILayout layout)
		//{
		//	if (handler.PlatformView is LayoutViewGroup layoutViewGroup)
		//	{
		//		layoutViewGroup.InputTransparent = layout.InputTransparent;
		//	}
		//}
	}
}
