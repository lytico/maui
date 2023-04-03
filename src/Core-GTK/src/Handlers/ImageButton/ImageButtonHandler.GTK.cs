using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Gdk;
using Microsoft.Maui.Platform.GTK;

namespace Microsoft.Maui.Handlers
{
	public partial class ImageButtonHandler : ViewHandler<IImageButton, Gtk.Button>
	{
		protected override Gtk.Button CreatePlatformView(IView imageButton)
		{
			Gtk.Button platformView = new Gtk.Button();

			var _view = imageButton;
			if (_view is IImageButton virtualImageButton)
			{
				if (virtualImageButton.Source != null)
				{
					var fileImageSource = (IFileImageSource)virtualImageButton.Source;

					if (fileImageSource != null)
					{
						var imageElement = new Gtk.Image();
						imageElement.File = fileImageSource.File;

						var pixbufReturned = new Gdk.Pixbuf(fileImageSource.File);
						using (var drawableScaled = pixbufReturned.ScaleSimple((int)_view.Width, (int)_view.Height, InterpType.Bilinear)) {
							imageElement.Pixbuf = drawableScaled;
						}

						platformView.Image = imageElement;
					}
				}
			}

			if ((_view != null) && (_view.Width > 0))
			{
				platformView.WidthRequest = (int)_view.Width;
			}
			if ((_view != null) && (_view.Height > 0))
			{
				platformView.HeightRequest = (int)_view.Height;
			}

			Gtk.Widget widget = platformView;
			SetMargins(imageButton, ref widget);

			if (imageButton is IImageButton imageView)
			{
				if (imageView.Visibility == Visibility.Visible)
				{
					platformView.Show();
				}
			}

			return platformView;
		}

		void OnSetImageSource(Gdk.Pixbuf? obj)
		{
			Console.WriteLine("OnSetImageSource");
		}

		private protected override void OnConnectHandler(object platformView)
		{
			if (platformView is Gtk.Button imageButton)
			{
				imageButton.Clicked += ButtonWidget_Clicked;
			}

			base.OnConnectHandler(platformView);
		}

		private void ButtonWidget_Clicked(object? sender, System.EventArgs e)
		{
			VirtualView?.Clicked();
		}

		private protected override void OnDisconnectHandler(object platformView)
		{
			if (platformView is Gtk.Button button)
			{
				button.Clicked -= ButtonWidget_Clicked;
			}

			base.OnDisconnectHandler(platformView);
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

		public static void MapImageSource(IImageButtonHandler handler, IImageButton image)
		{
			if (image == null) {
				return;
			}

			if (image.Source != null)
			{
				var fileImageSource = (IFileImageSource)image.Source;

				if (fileImageSource != null)
				{
					var imageElement = new Gtk.Image();
					imageElement.File = fileImageSource.File;

					var pixbufReturned = new Gdk.Pixbuf(fileImageSource.File);
					using (var drawableScaled = pixbufReturned.ScaleSimple((int)image.Width, (int)image.Height, InterpType.Bilinear)) {
						imageElement.Pixbuf = drawableScaled;
					}

					handler.PlatformView.Image = imageElement;
				}
			}

			if ((image != null) && (image.Width > 0))
			{
				handler.PlatformView.WidthRequest = (int)image.Width;
			}
			if ((image != null) && (image.Height > 0))
			{
				handler.PlatformView.HeightRequest = (int)image.Height;
			}

			if (image != null) {
				Gtk.Widget widget = handler.PlatformView;
				((IPlatformViewHandler)handler).SetMargins((IView)image, ref widget);

				if (image.Visibility == Visibility.Visible)
				{
					handler.PlatformView.Show();
				}
			}


			//if (handler.PlatformView is Gtk.Button buttonHandler)
			//{
			//	if (image.Source == null)
			//	{
			//		buttonHandler.Image = null!;
			//	}
			//	else
			//	{
			//		var fileImageSource = (IFileImageSource)image.Source;

			//		if (fileImageSource != null)
			//		{
			//			buttonHandler.Image = new Gtk.Image(fileImageSource.File);
			//		}
			//	}
			//}
		}

		public override void PlatformArrange(Graphics.Rect frame)
		{
			var platformImageElement = this.PlatformView.Image as Gtk.Image;
			if (platformImageElement == null) {
				platformImageElement = new Gtk.Image();
			}

			if (platformImageElement != null) {
				if (!string.IsNullOrEmpty(platformImageElement.File)) {
					var pixbufReturned = new Gdk.Pixbuf(platformImageElement.File);
					using (var drawableScaled = pixbufReturned.ScaleSimple((int)frame.Width, (int)frame.Height, InterpType.Bilinear)) {
						platformImageElement.Pixbuf = drawableScaled;
					}
				}

				if ((frame != null) && (frame.Width > 0))
				{
					this.PlatformView.WidthRequest = (int)frame.Width;
				}
				if ((frame != null) && (frame.Height > 0))
				{
					this.PlatformView.HeightRequest = (int)frame.Height;
				}

				Gtk.Widget widget = this.PlatformView;
				((IPlatformViewHandler)this).SetMargins(this.VirtualView, ref widget);

				if (this.VirtualView.Visibility == Visibility.Visible)
				{
					this.PlatformView.Show();
				}
			}

			base.PlatformArrange(frame);
		}
	}
}