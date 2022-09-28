using System;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Microsoft.Maui.ApplicationModel;

namespace Microsoft.Maui.Media
{
	partial class ScreenshotImplementation : IScreenshot
	{
		public bool IsCaptureSupported =>
			true;

		public Task<IScreenshotResult> CaptureAsync()
		{
			return (Task<IScreenshotResult>)Task.CompletedTask;
		}
	}

	partial class ScreenshotResult
	{
		readonly byte[] bytes;

		public ScreenshotResult(int width, int height, Gdk.Pixbuf pixels)
		{
		}

		Task<Stream> PlatformOpenReadAsync(ScreenshotFormat format, int quality)
		{
			return (Task<Stream>)Task.CompletedTask;
		}

		Task PlatformCopyToAsync(Stream destination, ScreenshotFormat format, int quality)
		{
			return Task.CompletedTask;
		}
	}
}
