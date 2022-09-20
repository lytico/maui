#nullable enable
using System;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using Gdk;
using Microsoft.UI.Xaml.Media;

namespace Microsoft.Maui
{
	public partial class FileImageSourceService
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