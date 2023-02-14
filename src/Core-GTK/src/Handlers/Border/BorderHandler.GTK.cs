using System;
using System.Runtime.InteropServices;

namespace Microsoft.Maui.Handlers
{
	public partial class BorderHandler : ViewHandler<IBorderView, MauiBorder>
	{
		protected override MauiBorder CreatePlatformView(IView border)
		{
			if (VirtualView == null)
			{
				throw new InvalidOperationException($"{nameof(VirtualView)} must be set to create a Border");
			}

			var viewGroup = new MauiBorder();
			//{
			//	CrossPlatformMeasure = VirtualView.CrossPlatformMeasure,
			//	CrossPlatformArrange = VirtualView.CrossPlatformArrange
			//};

			// We only want to use a hardware layer for the entering view because its quite likely
			// the view will invalidate several times the Drawable (Draw).
			// viewGroup.SetLayerType(Android.Views.LayerType.Hardware, null);

			return viewGroup;
		}

		public override void SetVirtualView(IView view)
		{
			base.SetVirtualView(view);
			_ = VirtualView ?? throw new InvalidOperationException($"{nameof(VirtualView)} should have been set by base class.");
			_ = PlatformView ?? throw new InvalidOperationException($"{nameof(PlatformView)} should have been set by base class.");
			_ = MauiContext ?? throw new InvalidOperationException($"{nameof(MauiContext)} should have been set by base class.");

			//PlatformView.CrossPlatformMeasure = VirtualView.CrossPlatformMeasure;
			//PlatformView.CrossPlatformArrange = VirtualView.CrossPlatformArrange;

			foreach (var child in PlatformView.Children)
			{
				PlatformView.Remove(child);
			}

			if (VirtualView.PresentedContent is IView contentView)
				PlatformView.Add((Gtk.Widget)contentView.ToPlatform(MauiContext));
		}

		static void UpdateContent(IBorderHandler handler)
		{
			_ = handler.PlatformView ?? throw new InvalidOperationException($"{nameof(PlatformView)} should have been set by base class.");
			_ = handler.MauiContext ?? throw new InvalidOperationException($"{nameof(MauiContext)} should have been set by base class.");
			_ = handler.VirtualView ?? throw new InvalidOperationException($"{nameof(VirtualView)} should have been set by base class.");

			foreach (var child in handler.PlatformView.Children)
			{
				handler.PlatformView.Remove(child);
			}

			if (handler.VirtualView.PresentedContent is IView view)
				handler.PlatformView.Add((Gtk.Widget)view.ToPlatform(handler.MauiContext));
		}

		public static void MapHeight(IBorderHandler handler, IBorderView border)
		{
			if (handler.PlatformView != null)
			{
				handler.PlatformView.HeightRequest = (int)border.Height;
			}
			//handler.PlatformView?.UpdateHeight(border);
			//handler.PlatformView?.InvalidateBorderStrokeBounds();
		}

		public static void MapWidth(IBorderHandler handler, IBorderView border)
		{
			if (handler.PlatformView != null)
			{
				handler.PlatformView.WidthRequest = (int)border.Width;
			}
			//handler.PlatformView?.UpdateWidth(border);
			//handler.PlatformView?.InvalidateBorderStrokeBounds();
		}

		public static void MapContent(IBorderHandler handler, IBorderView border)
		{
			UpdateContent(handler);
		}

		protected override void DisconnectHandler(MauiBorder platformView)
		{
			// If we're being disconnected from the xplat element, then we should no longer be managing its chidren
			foreach (var child in PlatformView.Children)
			{
				PlatformView.Remove(child);
			}

			base.DisconnectHandler(platformView);
		}
	}
}