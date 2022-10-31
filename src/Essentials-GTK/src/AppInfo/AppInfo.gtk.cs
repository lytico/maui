using System;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;

namespace Microsoft.Maui.ApplicationModel
{
	class AppInfoImplementation : IAppInfo
	{
		static readonly Assembly _launchingAssembly = Assembly.GetEntryAssembly();

		const string SettingsUri = "ms-settings:appsfeatures-app";

		public string PackageName => _launchingAssembly.GetAppInfoValue("PackageName");

		internal static string PublisherName => _launchingAssembly.GetAppInfoValue("PublisherName");

		public string Name => _launchingAssembly.GetAppInfoValue("Name");

		public Version Version => _launchingAssembly.GetAppInfoVersionValue("Version");

		public string VersionString => Version.ToString();

		public string BuildString => Version.Revision.ToString(CultureInfo.InvariantCulture);

		public void ShowSettingsUI()
		{
			Process.Start(new ProcessStartInfo { FileName = SettingsUri, UseShellExecute = true });
		}

		public AppTheme RequestedTheme => AppTheme.Unspecified;

		public AppPackagingModel PackagingModel => AppPackagingModel.Unpackaged;

		public LayoutDirection RequestedLayoutDirection => LayoutDirection.LeftToRight;
	}

	static class AppInfoUtils
	{
		static readonly Lazy<bool> _isPackagedAppLazy = new Lazy<bool>(() =>
		{
			return true;
		});

		public static bool IsPackagedApp => _isPackagedAppLazy.Value;

		public static Version GetAppInfoVersionValue(this Assembly assembly, string name)
		{
			if (assembly.GetAppInfoValue(name) is string value && !string.IsNullOrEmpty(value))
				return Version.Parse(value);

			return null;
		}

		public static string GetAppInfoValue(this Assembly assembly, string name) =>
			assembly.GetMetadataAttributeValue("Microsoft.Maui.ApplicationModel.AppInfo." + name);

		public static string GetMetadataAttributeValue(this Assembly assembly, string key)
		{
			foreach (var attr in assembly.GetCustomAttributes<AssemblyMetadataAttribute>())
			{
				if (attr.Key == key)
					return attr.Value;
			}

			return null;
		}
	}
}
