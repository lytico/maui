using System;
using System.IO;

namespace GtkSharp.BlazorWebKit;

public record BlazorWebViewOptions
{

	public Type? RootComponent { get; init; }

	public string HostPath { get; init; } = Path.Combine("wwwroot", "index.html");

	public string ContentRoot { get => Path.GetDirectoryName(Path.GetFullPath(HostPath))!; }

	public string RelativeHostPath { get => Path.GetRelativePath(ContentRoot, HostPath); }

}