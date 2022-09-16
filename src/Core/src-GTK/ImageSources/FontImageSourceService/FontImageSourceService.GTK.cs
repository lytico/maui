#nullable enable
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Media;

namespace Microsoft.Maui
{
	public partial class FontImageSourceService
	{
		public override Task<IImageSourceServiceResult<ImageSource>?> GetImageSourceAsync(IImageSource imageSource, float scale = 1, CancellationToken cancellationToken = default)
		{
			return Task.FromResult<IImageSourceServiceResult<ImageSource>?>(null);
		}

		public Task<IImageSourceServiceResult<Gtk.Image>?> GetImageAsync(IFontImageSource imageSource, Gtk.Image image, CancellationToken cancellationToken = default)
		{
			return Task.FromResult<IImageSourceServiceResult<Gtk.Image>?>(null);
		}
	}
}