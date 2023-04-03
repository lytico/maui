using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Gdk;
using Gtk;

namespace Microsoft.Maui.Handlers
{
	public partial class ImageHandler : ViewHandler<IImage, Gtk.Image>
	{
		protected override Gtk.Image CreatePlatformView(IView image)
		{
			Gtk.Image imageElement = null!;

			var _view = image;
			if (_view is IImage virtualImageButton)
			{
				if (virtualImageButton.Source != null)
				{
					var fileImageSource = (IFileImageSource)virtualImageButton.Source;

					if (fileImageSource != null)
					{
						imageElement = new Gtk.Image();
						imageElement.File = fileImageSource.File;

						var pixbufReturned = new Gdk.Pixbuf(fileImageSource.File);
						using (var drawableScaled = pixbufReturned.ScaleSimple((int)_view.Width, (int)_view.Height, InterpType.Bilinear)) {
							imageElement.Pixbuf = drawableScaled;
						}
					}
				}
			}

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
			if (_view is IImage virtualImageButton)
			{
				if (virtualImageButton.Source != null)
				{
					var fileImageSource = (IFileImageSource)virtualImageButton.Source;

					if (fileImageSource != null)
					{
						handler.PlatformView.File = fileImageSource.File;

						var pixbufReturned = new Gdk.Pixbuf(fileImageSource.File);
						using (var drawableScaled = pixbufReturned.ScaleSimple((int)_view.Width, (int)_view.Height, InterpType.Bilinear)) {
							handler.PlatformView.Pixbuf = drawableScaled;
						}
					}
				}
			}

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

		public static void MapWidth(IImageHandler handler, IImage image) {
			HandleMapSizes(handler, image);
		}

		public static void MapHeight(IImageHandler handler, IImage image) {
			HandleMapSizes(handler, image);
		}

		private static void HandleMapSizes(IImageHandler handler, IImage image) {
			if (!string.IsNullOrEmpty(handler.PlatformView.File)) {
				var pixbufReturned = new Gdk.Pixbuf(handler.PlatformView.File);
				using (var drawableScaled = pixbufReturned.ScaleSimple((int)image.Width, (int)image.Height, InterpType.Bilinear)) {
					handler.PlatformView.Pixbuf = drawableScaled;
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

			Gtk.Widget widget = handler.PlatformView;
			((IPlatformViewHandler)handler).SetMargins(handler.VirtualView, ref widget);

			if (handler.VirtualView.Visibility == Visibility.Visible)
			{
				handler.PlatformView.Show();
			}
		}
	}
}