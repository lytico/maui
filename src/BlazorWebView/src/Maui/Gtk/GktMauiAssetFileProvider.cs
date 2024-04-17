using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;

namespace Microsoft.AspNetCore.Components.WebView.Maui
{
	/// <summary>
	/// A minimal implementation of an IFileProvider to be used by the BlazorWebView and WebViewManager types.
	/// </summary>
	internal sealed class GtkMauiAssetFileProvider : IFileProvider
	{
		private readonly string _resDir;

		public GtkMauiAssetFileProvider(string contentRootDir)
		{
			_resDir = Path.Combine(Assembly.GetEntryAssembly()?.Location ?? Directory.GetCurrentDirectory(), contentRootDir);
		}

		public IDirectoryContents GetDirectoryContents(string subpath)
			=> new GtkMauiAssetDirectoryContents(Path.Combine(_resDir, subpath));

		public IFileInfo GetFileInfo(string subpath)
			=> new GtkMauiAssetFileInfo(Path.Combine(_resDir, subpath));

		public IChangeToken Watch(string filter)
			=> NullChangeToken.Singleton;

		private sealed class GtkMauiAssetFileInfo : IFileInfo
		{
			private readonly string _filePath;

			public GtkMauiAssetFileInfo(string filePath)
			{
				_filePath = filePath;

				Name = Path.GetFileName(_filePath);

				var fileInfo = new FileInfo(_filePath);
				Exists = fileInfo.Exists;
				Length = Exists ? fileInfo.Length : -1;
			}

			public bool Exists { get; }
			public long Length { get; }
			public string PhysicalPath { get; } = null!;
			public string Name { get; }
			public DateTimeOffset LastModified { get; } = DateTimeOffset.FromUnixTimeSeconds(0);
			public bool IsDirectory => false;

			public Stream CreateReadStream()
				=> File.OpenRead(_filePath);
		}

		// This is never used by BlazorWebView or WebViewManager
		private sealed class GtkMauiAssetDirectoryContents : IDirectoryContents
		{
			public GtkMauiAssetDirectoryContents(string filePath) { }

			public bool Exists => false;

			public IEnumerator<IFileInfo> GetEnumerator()
				=> throw new NotImplementedException();

			IEnumerator IEnumerable.GetEnumerator()
				=> throw new NotImplementedException();
		}
	}
}