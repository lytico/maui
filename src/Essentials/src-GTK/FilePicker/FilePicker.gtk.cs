using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Devices;

namespace Microsoft.Maui.Storage
{
	partial class FilePickerImplementation : IFilePicker
	{
		Task<IEnumerable<FileResult>> PlatformPickAsync(PickOptions options, bool allowMultiple = false)
		{
			List<FileResult> result = new List<FileResult>();
			Gtk.FileChooserDialog filechooser =
				new Gtk.FileChooserDialog("Choose the file to open",
					null,
					Gtk.FileChooserAction.Open,
					"Cancel", Gtk.ResponseType.Cancel,
					"Open", Gtk.ResponseType.Accept);

			if (filechooser.Run() == (int)Gtk.ResponseType.Accept)
			{
				System.IO.FileStream file = System.IO.File.OpenRead(filechooser.Filename);
				result.Add(new FileResult(filechooser.Filename));
				file.Close();
			}

			filechooser.Destroy();

			return Task.FromResult<IEnumerable<FileResult>>(result);
		}
	}

	public partial class FilePickerFileType
	{
		static FilePickerFileType PlatformImageFileType() =>
			new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
			{
				{ DevicePlatform.Unknown, FileExtensions.AllImage }
			});

		static FilePickerFileType PlatformPngFileType() =>
			new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
			{
				{ DevicePlatform.Unknown, new[] { FileExtensions.Png } }
			});

		static FilePickerFileType PlatformJpegFileType() =>
			new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
			{
				{ DevicePlatform.Unknown, FileExtensions.AllJpeg }
			});

		static FilePickerFileType PlatformVideoFileType() =>
			new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
			{
				{ DevicePlatform.Unknown, FileExtensions.AllVideo }
			});

		static FilePickerFileType PlatformPdfFileType() =>
			new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
			{
				{ DevicePlatform.Unknown, new[] { FileExtensions.Pdf } }
			});
	}
}
