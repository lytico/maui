using System;
using System.IO;
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
#pragma warning disable RS0016

namespace BlazorWebKit;

public record BlazorWebViewOptions
{
    public Type? RootComponent { get; init; }
    public string HostPath { get; init; } = Path.Combine("wwwroot", "index.html");
    public string ContentRoot { get => Path.GetDirectoryName(Path.GetFullPath(HostPath))!; }
    public string RelativeHostPath { get => Path.GetRelativePath(ContentRoot, HostPath); }
}