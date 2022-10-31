using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Maui.ApplicationModel;

namespace Microsoft.Maui.Storage
{
	partial class FileSystemImplementation : IFileSystem
	{
		static string CleanPath(string path) =>
			string.Join("_", path.Split(Path.GetInvalidFileNameChars()));

		static string AppSpecificPath =>
			Path.Combine(CleanPath(AppInfoImplementation.PublisherName), CleanPath(AppInfo.PackageName));

		string PlatformCacheDirectory
			=> Path.Combine("~/Public", AppSpecificPath, "Cache");

		string PlatformAppDataDirectory
			=> Path.Combine("~/Public", AppSpecificPath, "Data");

		Task<Stream> PlatformOpenAppPackageFileAsync(string filename)
		{
			if (filename == null)
				throw new ArgumentNullException(nameof(filename));

			var file = FileSystemUtils.PlatformGetFullAppPackageFilePath(filename);
			return Task.FromResult((Stream)File.OpenRead(file));
		}

		Task<bool> PlatformAppPackageFileExistsAsync(string filename)
		{
			var file = FileSystemUtils.PlatformGetFullAppPackageFilePath(filename);
			return Task.FromResult(File.Exists(file));
		}
	}

	static partial class FileSystemUtils
	{
		public static bool AppPackageFileExists(string filename)
		{
			var file = PlatformGetFullAppPackageFilePath(filename);
			return File.Exists(file);
		}

		public static string PlatformGetFullAppPackageFilePath(string filename)
		{
			if (filename == null)
				throw new ArgumentNullException(nameof(filename));

			filename = NormalizePath(filename);

			string root = AppContext.BaseDirectory;

			return Path.Combine(root, filename);
		}
	}
}
