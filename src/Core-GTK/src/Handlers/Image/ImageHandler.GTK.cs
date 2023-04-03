using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Gdk;

namespace Microsoft.Maui.Handlers
{
	public partial class ImageHandler : ViewHandler<IImage, Gtk.Image>
	{
		protected override Gtk.Image CreatePlatformView(IView image)
		{
			Gtk.Image imageElement = null!;

			var _view = image;
			//if ((_view is ITextButton virtualTextButton) && (_view is IImageButton virtualImageButton))
			if (_view is IImage virtualImageButton)
			{
				//_fontSize = Convert.ToInt32(virtualTextButton.Font.Size);
				//if (virtualTextButton.TextColor != null)
				//{
				//	var red = Convert.ToByte(virtualTextButton.TextColor.Red * 255);
				//	var green = Convert.ToByte(virtualTextButton.TextColor.Green * 255);
				//	var blue = Convert.ToByte(virtualTextButton.TextColor.Blue * 255);
				//	FontColor = new Gdk.Color(red, green, blue);
				//}
				//else if (virtualTextButton.StrokeColor != null)
				//{
				//	var red = Convert.ToByte(virtualTextButton.StrokeColor.Red * 255);
				//	var green = Convert.ToByte(virtualTextButton.StrokeColor.Green * 255);
				//	var blue = Convert.ToByte(virtualTextButton.StrokeColor.Blue * 255);
				//	FontColor = new Gdk.Color(red, green, blue);
				//}

				if (virtualImageButton.Source != null)
				{
					var fileImageSource = (IFileImageSource)virtualImageButton.Source;

					if (fileImageSource != null)
					{
						imageElement = new Gtk.Image(fileImageSource.File);
						// Console.WriteLine("Image: " + fileImageSource.File);
						//if (string.IsNullOrEmpty(virtualTextButton.Text))
						//{
						//	imageElement = new Gtk.Image(fileImageSource.File);
						//	//Initialize(string.Empty, fileImageSource.File, string.Empty, name);
						//}
						//else
						//{
						//	Initialize(virtualTextButton.Text, fileImageSource.File, string.Empty, name);
						//}
						//return;
					}
				}
				//Initialize(virtualTextButton.Text, string.Empty, string.Empty, name);

				//return;
			}
			//else if (!(_view is ITextButton) && (_view is IImageButton virtualImageButtonButton))
			//{
			//	if (virtualImageButtonButton.Source != null)
			//	{
			//		var fileImageSource = (IFileImageSource)virtualImageButtonButton.Source;

			//		if (fileImageSource != null)
			//		{
			//			// Console.WriteLine("Image: " + fileImageSource.File);
			//			imageElement = new Gtk.Image(fileImageSource.File);
			//			// Initialize(string.Empty, fileImageSource.File, string.Empty, name);

			//			// return;
			//		}
			//	}
			//}

			if (imageElement == null!) {
				imageElement = new Gtk.Image();
			}

			if ((_view != null) && (_view.Width > 0))
			{
				imageElement.WidthRequest = (int)_view.Width;
			}
			if ((_view != null) && (_view.Height > 0))
			{
				imageElement.HeightRequest = (int)_view.Height;
			}

			Gtk.Widget widget = imageElement;
			SetMargins(image, ref widget);

			if (image is IImage imageView)
			{
				if (imageView.Visibility == Visibility.Visible)
				{
					imageElement.Show();
				}
			}

			return imageElement;
		}

		protected override void DisconnectHandler(Gtk.Image platformView)
		{
			base.DisconnectHandler(platformView);
			SourceLoader.Reset();
		}

		public override bool NeedsContainer =>
			VirtualView?.Background != null ||
			base.NeedsContainer;

		public static void MapBackground(IImageHandler handler, IImage image)
		{
			handler.UpdateValue(nameof(IViewHandler.ContainerView));

			//handler.ToPlatform().UpdateBackground(image);
			//handler.ToPlatform().UpdateOpacity(image);
		}

		public static void MapAspect(IImageHandler handler, IImage image) { }
			// handler.PlatformView?.UpdateAspect(image);

		public static void MapIsAnimationPlaying(IImageHandler handler, IImage image) { }
			// handler.PlatformView?.UpdateIsAnimationPlaying(image);

		public static void MapSource(IImageHandler handler, IImage image) {
			var _view = image;
			//if ((_view is ITextButton virtualTextButton) && (_view is IImageButton virtualImageButton))
			if (_view is IImage virtualImageButton)
			{
				//_fontSize = Convert.ToInt32(virtualTextButton.Font.Size);
				//if (virtualTextButton.TextColor != null)
				//{
				//	var red = Convert.ToByte(virtualTextButton.TextColor.Red * 255);
				//	var green = Convert.ToByte(virtualTextButton.TextColor.Green * 255);
				//	var blue = Convert.ToByte(virtualTextButton.TextColor.Blue * 255);
				//	FontColor = new Gdk.Color(red, green, blue);
				//}
				//else if (virtualTextButton.StrokeColor != null)
				//{
				//	var red = Convert.ToByte(virtualTextButton.StrokeColor.Red * 255);
				//	var green = Convert.ToByte(virtualTextButton.StrokeColor.Green * 255);
				//	var blue = Convert.ToByte(virtualTextButton.StrokeColor.Blue * 255);
				//	FontColor = new Gdk.Color(red, green, blue);
				//}

				if (virtualImageButton.Source != null)
				{
					var fileImageSource = (IFileImageSource)virtualImageButton.Source;

					if (fileImageSource != null)
					{
						// this.image1.Pixbuf = new Gdk.Pixbuf ("/home/whoami/Pictures/1.png");
						// widget.WidthRequest = (int)view.Width;
						handler.PlatformView.File = fileImageSource.File;

						var pixbufReturned = new Gdk.Pixbuf(fileImageSource.File);
						using (var drawableScaled = pixbufReturned.ScaleSimple((int)_view.Width, (int)_view.Height, InterpType.Bilinear)) {
							// only set if we are still on the same image
							handler.PlatformView.Pixbuf = drawableScaled;
						}

						// handler.PlatformView.File = fileImageSource.File;
						//imageElement = new Gtk.Image(fileImageSource.File);
						// Console.WriteLine("Image: " + fileImageSource.File);
						//if (string.IsNullOrEmpty(virtualTextButton.Text))
						//{
						//	imageElement = new Gtk.Image(fileImageSource.File);
						//	//Initialize(string.Empty, fileImageSource.File, string.Empty, name);
						//}
						//else
						//{
						//	Initialize(virtualTextButton.Text, fileImageSource.File, string.Empty, name);
						//}
						//return;
					}
				}
				//Initialize(virtualTextButton.Text, string.Empty, string.Empty, name);

				//return;
			}
			//else if (!(_view is ITextButton) && (_view is IImageButton virtualImageButtonButton))
			//{
			//	if (virtualImageButtonButton.Source != null)
			//	{
			//		var fileImageSource = (IFileImageSource)virtualImageButtonButton.Source;

			//		if (fileImageSource != null)
			//		{
			//			// Console.WriteLine("Image: " + fileImageSource.File);
			//			imageElement = new Gtk.Image(fileImageSource.File);
			//			// Initialize(string.Empty, fileImageSource.File, string.Empty, name);

			//			// return;
			//		}
			//	}
			//}

			//if (imageElement == null!) {
			//	imageElement = new Gtk.Image();
			//}

			if ((_view != null) && (_view.Width > 0))
			{
				handler.PlatformView.WidthRequest = (int)_view.Width;
			}
			if ((_view != null) && (_view.Height > 0))
			{
				handler.PlatformView.HeightRequest = (int)_view.Height;
			}

			Gtk.Widget widget = handler.PlatformView;
			((IPlatformViewHandler)handler).SetMargins(image, ref widget);

			if (image is IImage imageView)
			{
				if (imageView.Visibility == Visibility.Visible)
				{
					handler.PlatformView.Show();
				}
			}
		}

		void OnSetImageSource(Gdk.Pixbuf? obj)
		{
			this.PlatformView.Pixbuf = obj;
		}

		public override void PlatformArrange(Graphics.Rect frame)
		{
			//if (PlatformView.GetScaleType() == ImageView.ScaleType.CenterCrop)
			//{
			//	// If the image is center cropped (AspectFill), then the size of the image likely exceeds
			//	// the view size in some dimension. So we need to clip to the view's bounds.

			//	var (left, top, right, bottom) = PlatformView.Context!.ToPixels(frame);
			//	var clipRect = new Android.Graphics.Rect(0, 0, right - left, bottom - top);
			//	PlatformView.ClipBounds = clipRect;
			//}
			//else
			//{
			//	PlatformView.ClipBounds = null;
			//}

			base.PlatformArrange(frame);
		}
	}
}