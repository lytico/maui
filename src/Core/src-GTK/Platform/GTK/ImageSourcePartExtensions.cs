using System;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Maui.Platform
{
	internal static class ImageSourcePartExtensions
	{
		public static async Task<IImageSourceServiceResult?> UpdateSourceAsync(
			this IImageSourcePart image,
			Gtk.EventBox destinationContext,
			IImageSourceServiceProvider services,
			Action<Gdk.Pixbuf?> setImage,
			CancellationToken cancellationToken = default)
		{
			image.UpdateIsLoading(false);

			var destinationImageView = destinationContext as MauiImage;

			if (destinationImageView is null && setImage is null)
				return null;

			var imageSource = image.Source;
			if (imageSource == null)
				return null;

			var events = image as IImageSourcePartEvents;

			events?.LoadingStarted();
			image.UpdateIsLoading(true);

			try
			{
				var service = services.GetRequiredImageSourceService(imageSource);

				var applied = !cancellationToken.IsCancellationRequested && imageSource == image.Source;

				IImageSourceServiceResult? result = null;

				if (applied)
				{
					result = await service.GetImageSourceAsync(imageSource, 1, cancellationToken);

					if (result is null)
						throw new InvalidOperationException("Glide failed to load image");
				}

				events?.LoadingCompleted(applied);
				return result;
			}
			catch (OperationCanceledException)
			{
				// no-op
				events?.LoadingCompleted(false);
			}
			catch (Exception ex)
			{
				events?.LoadingFailed(ex);
			}
			finally
			{
				// only mark as finished if we are still working on the same image
				if (imageSource == image.Source)
				{
					image.UpdateIsLoading(false);
				}
			}

			return null;
		}
	}
}