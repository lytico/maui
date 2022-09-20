#nullable enable
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Gdk;

namespace Microsoft.Maui
{
	public partial class UriImageSourceService
	{
		public override Pixbuf? GetImageSourceAsync(IImageSource imageSource, float scale = 1, CancellationToken cancellationToken = default)
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

			return image;
		}
	}
}