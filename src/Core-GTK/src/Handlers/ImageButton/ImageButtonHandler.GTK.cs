using System;

namespace Microsoft.Maui.Handlers
{
	public partial class ImageButtonHandler : ViewHandler<IImageButton, MauiImageButton>
	{
		protected override MauiImageButton CreatePlatformView()
		{
			var platformView = new MauiImageButton();

			return platformView;
		}

		void OnSetImageSource(Gdk.Pixbuf? obj)
		{
		}

		protected override void DisconnectHandler(MauiImageButton platformView)
		{
			platformView.ButtonWidget.Clicked -= PlatformView_Clicked;

			base.DisconnectHandler(platformView);

			SourceLoader.Reset();
		}

		protected override void ConnectHandler(MauiImageButton platformView)
		{
			platformView.ButtonWidget.Clicked += PlatformView_Clicked;

			base.ConnectHandler(platformView);
		}

		private void PlatformView_Clicked(object? sender, EventArgs e)
		{
			VirtualView?.Clicked();
		}

		// TODO: NET7 make this public
		internal static void MapBackground(IImageButtonHandler handler, IImageButton imageButton)
		{
			//(handler.PlatformView as ShapeableImageView)?.UpdateBackground(imageButton);
		}

		public static void MapStrokeColor(IImageButtonHandler handler, IButtonStroke buttonStroke)
		{
			//(handler.PlatformView as ShapeableImageView)?.UpdateStrokeColor(buttonStroke);
		}

		public static void MapStrokeThickness(IImageButtonHandler handler, IButtonStroke buttonStroke)
		{
			//(handler.PlatformView as ShapeableImageView)?.UpdateStrokeThickness(buttonStroke);
		}

		public static void MapCornerRadius(IImageButtonHandler handler, IButtonStroke buttonStroke)
		{
			//(handler.PlatformView as ShapeableImageView)?.UpdateCornerRadius(buttonStroke);
		}

		public static void MapPadding(IImageButtonHandler handler, IImageButton imageButton)
		{
			//(handler.PlatformView as ShapeableImageView)?.UpdatePadding(imageButton);
		}

		//void OnFocusChange(object? sender, View.FocusChangeEventArgs e)
		//{
		//	if (VirtualView != null)
		//		VirtualView.IsFocused = e.HasFocus;
		//}

		//void OnTouch(object? sender, View.TouchEventArgs e)
		//{
		//	var motionEvent = e.Event;
		//	switch (motionEvent?.ActionMasked)
		//	{
		//		case MotionEventActions.Down:
		//			VirtualView?.Pressed();
		//			break;
		//		case MotionEventActions.Up:
		//			VirtualView?.Released();
		//			break;
		//	}

		//	e.Handled = false;
		//}

		//void OnClick(object? sender, EventArgs e)
		//{
		//	VirtualView?.Clicked();
		//}
	}
}