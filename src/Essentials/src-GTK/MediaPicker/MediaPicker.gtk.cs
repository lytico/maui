using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Storage;

namespace Microsoft.Maui.Media
{
	partial class MediaPickerImplementation : IMediaPicker
	{
		public bool IsCaptureSupported
			=> true;

		public Task<FileResult> PickPhotoAsync(MediaPickerOptions options)
			=> PickAsync(options, true);

		public Task<FileResult> PickVideoAsync(MediaPickerOptions options)
			=> PickAsync(options, false);

		public Task<FileResult> PickAsync(MediaPickerOptions options, bool photo)
		{
			FileResult result = null;
			Gtk.FileChooserDialog filechooser =
				new Gtk.FileChooserDialog("Choose the file to open",
					null,
					Gtk.FileChooserAction.Open,
					"Cancel", Gtk.ResponseType.Cancel,
					"Open", Gtk.ResponseType.Accept);

			if (filechooser.Run() == (int)Gtk.ResponseType.Accept)
			{
				System.IO.FileStream file = System.IO.File.OpenRead(filechooser.Filename);
				result = new FileResult(filechooser.Filename);
				file.Close();
			}

			filechooser.Destroy();

			return Task.FromResult<FileResult>(result);
		}

		public Task<FileResult> CapturePhotoAsync(MediaPickerOptions options)
			=> CaptureAsync(options, true);

		public Task<FileResult> CaptureVideoAsync(MediaPickerOptions options)
			=> CaptureAsync(options, false);

		public Task<FileResult> CaptureAsync(MediaPickerOptions options, bool photo)
		{
			return Task.FromResult<FileResult>(null);
		}
	}
}
