using System;
using System.IO;

namespace GtkSharp.BlazorWebKit;

public class BlazorWebViewOptions
{

	public Type RootComponent { get; init; }

	public string HostPath { get; init; } = Path.Combine("wwwroot", "index.html");

}