using System;
using System.Diagnostics;
using Microsoft.Maui.Platform.GTK;

namespace Microsoft.Maui.Handlers
{
	public partial class ImageButtonHandler : ViewHandler<IImageButton, MauiGTKButton>
	{
		protected override MauiGTKButton CreatePlatformView(IView imageButton)
		{
			var platformView = new MauiGTKButton(imageButton);

			Gtk.Widget widget = platformView;
			SetMargins(imageButton, ref widget);

			platformView.Show();

			return platformView;
		}

		void OnSetImageSource(Gdk.Pixbuf? obj)
		{
			Console.WriteLine("OnSetImageSource");
		}

		//protected override void DisconnectHandler(MauiGTKButton platformView)
		//{
		//	platformView.ButtonWidget.Clicked -= PlatformView_Clicked;

		//	base.DisconnectHandler(platformView);

		//	SourceLoader.Reset();
		//}

		//protected override void ConnectHandler(MauiGTKButton platformView)
		//{
		//	platformView.ButtonWidget.Clicked += PlatformView_Clicked;

		//	base.ConnectHandler(platformView);
		//}

		private protected override void OnConnectHandler(object platformView)
		{
			if (platformView is MauiGTKButton imageButton)
			{
				//imageButton.ButtonWidget.Clicked += ButtonWidget_Clicked;
				imageButton.Clicked += ButtonWidget_Clicked;
			}

			base.OnConnectHandler(platformView);
		}

		private void ButtonWidget_Clicked(object? sender, System.EventArgs e)
		{
			Debug.WriteLine("Clicked");
			VirtualView?.Clicked();
		}

		private protected override void OnDisconnectHandler(object platformView)
		{
			if (platformView is MauiGTKButton button)
			{
				button.Clicked -= ButtonWidget_Clicked;
			}

			base.OnDisconnectHandler(platformView);
		}

		//private void PlatformView_Clicked(object? sender, EventArgs e)
		//{
		//	VirtualView?.Clicked();
		//}

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

		public static void MapImageSource(IImageButtonHandler handler, IImageButton image)
		{
			if (handler.PlatformView is MauiGTKButton buttonHandler)
			{
				// buttonHandler.Image = new Gtk.Image(source);
				if (image.Source == null)
				{
					buttonHandler.Image = null!;
				}
				else
				{
					var fileImageSource = (IFileImageSource)image.Source;

					if (fileImageSource != null)
					{
						// Console.WriteLine("MapImageSource Image: " + fileImageSource.File);
						buttonHandler.Image = new Gtk.Image(fileImageSource.File);
					}
				}
			}
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