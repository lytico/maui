#nullable enable
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Gdk;

namespace Microsoft.Maui
{
	public partial class StreamImageSourceService
	{
		public override Task<IImageSourceServiceResult<Gdk.Pixbuf>?> GetImageSourceAsync(IImageSource imageSource, float scale = 1, CancellationToken cancellationToken = default)
		{
			Pixbuf? image = null;
			var fileImageSource = (IFileImageSource)imageSource;

			if (fileImageSource != null)
			{
				var file = fileImageSource.File;
				if (!string.IsNullOrEmpty(file))
				{
					var imagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, file);

					if (File.Exists(imagePath))
					{
						GLib.Idle.Add(delegate
						{ { image = new Pixbuf(imagePath); }; return false; });
					}
				}
			}

			if (image == null)
				return null!;

			return Task.FromResult<IImageSourceServiceResult<Gdk.Pixbuf>?>(new ImageSourceServiceResult(image));
		}
	}
}