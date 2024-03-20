#nullable enable
using System;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Xaml;

namespace Microsoft.Maui.TestUtils.DeviceTests.Runners.VisualRunner
{
	[ContentProperty(nameof(Name))]
	class EmbeddedResourceExtension : IMarkupExtension
	{
		public string? Name { get; set; }

		public virtual object? ProvideValue(IServiceProvider serviceProvider)
		{
			if (Name == null)

/* Unmerged change from project 'TestUtils.DeviceTests.Runners(net8.0-maccatalyst)'
Before:
				return null;

			var resourceName = "." + Name.Trim().Replace('/', '.').Replace('\\', '.');

			var assembly = typeof(MauiVisualRunnerApp).Assembly;
			foreach (var name in assembly.GetManifestResourceNames())
			{
				if (name.EndsWith(resourceName, StringComparison.InvariantCultureIgnoreCase))
					return assembly.GetManifestResourceStream(name);
After:
			{
				return null;
			}

			var resourceName = "." + Name.Trim().Replace('/', '.').Replace('\\', '.');

			var assembly = typeof(MauiVisualRunnerApp).Assembly;
			foreach (var name in assembly.GetManifestResourceNames())
			{
				if (name.EndsWith(resourceName, StringComparison.InvariantCultureIgnoreCase))
				{
					return assembly.GetManifestResourceStream(name);
				}
*/

/* Unmerged change from project 'TestUtils.DeviceTests.Runners(net8.0-windows10.0.19041.0)'
Before:
				return null;

			var resourceName = "." + Name.Trim().Replace('/', '.').Replace('\\', '.');

			var assembly = typeof(MauiVisualRunnerApp).Assembly;
			foreach (var name in assembly.GetManifestResourceNames())
			{
				if (name.EndsWith(resourceName, StringComparison.InvariantCultureIgnoreCase))
					return assembly.GetManifestResourceStream(name);
After:
			{
				return null;
			}

			var resourceName = "." + Name.Trim().Replace('/', '.').Replace('\\', '.');

			var assembly = typeof(MauiVisualRunnerApp).Assembly;
			foreach (var name in assembly.GetManifestResourceNames())
			{
				if (name.EndsWith(resourceName, StringComparison.InvariantCultureIgnoreCase))
				{
					return assembly.GetManifestResourceStream(name);
				}
*/

/* Unmerged change from project 'TestUtils.DeviceTests.Runners(net8.0-windows10.0.20348.0)'
Before:
				return null;

			var resourceName = "." + Name.Trim().Replace('/', '.').Replace('\\', '.');

			var assembly = typeof(MauiVisualRunnerApp).Assembly;
			foreach (var name in assembly.GetManifestResourceNames())
			{
				if (name.EndsWith(resourceName, StringComparison.InvariantCultureIgnoreCase))
					return assembly.GetManifestResourceStream(name);
After:
			{
				return null;
			}

			var resourceName = "." + Name.Trim().Replace('/', '.').Replace('\\', '.');

			var assembly = typeof(MauiVisualRunnerApp).Assembly;
			foreach (var name in assembly.GetManifestResourceNames())
			{
				if (name.EndsWith(resourceName, StringComparison.InvariantCultureIgnoreCase))
				{
					return assembly.GetManifestResourceStream(name);
				}
*/
			{
				return null;
			}

			var resourceName = "." + Name.Trim().Replace('/', '.').Replace('\\', '.');

			var assembly = typeof(MauiVisualRunnerApp).Assembly;
			foreach (var name in assembly.GetManifestResourceNames())
			{
				if (name.EndsWith(resourceName, StringComparison.InvariantCultureIgnoreCase))
				{
					return assembly.GetManifestResourceStream(name);
				}
			}

			return null;
		}
	}
}