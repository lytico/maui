using System;
using System.IO;

namespace GtkSharp.BlazorWebKit;

public class BlazorWebViewOptions
{

	static string _scheme = "app";
	static Uri _baseUri = new Uri($"{_scheme}://localhost/");

	public Uri BaseUri { get; set; } = _baseUri;

	public Type? RootComponent { get; set; }

	public string HostPath { get; set; } = Path.Combine("wwwroot", "index.html");

	public string ContentRoot { get => Path.GetDirectoryName(Path.GetFullPath(HostPath))!; }

	public string RelativeHostPath { get => Path.GetRelativePath(ContentRoot, HostPath); }

}