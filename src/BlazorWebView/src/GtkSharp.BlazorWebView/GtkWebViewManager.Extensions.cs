using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using WebKit;

namespace GtkSharp.BlazorWebKit;

public partial class GtkWebViewManager
{
	public static GtkWebViewManager NewForWebView(WebView webView, BlazorWebViewOptions options, IServiceProvider serviceProvider)
	{
		var hostPath = options.HostPath;
		var rootComponent = options.RootComponent;
		var contentRoot = Path.GetDirectoryName(Path.GetFullPath(hostPath))!;
		const string scheme = "app";
		var appBaseUri = new Uri($"{scheme}://localhost/");

		var relativePath = Path.GetRelativePath(contentRoot, hostPath);
		var dispatcher = Microsoft.AspNetCore.Components.Dispatcher.CreateDefault();

		var webViewManager = new GtkWebViewManager(webView,
			scheme,
			serviceProvider,
			dispatcher,
			appBaseUri,
			new PhysicalFileProvider(contentRoot),
			new(),
			relativePath);

		webViewManager.Attach();

		if (rootComponent != null)
			dispatcher.InvokeAsync(async () =>
			{
				await webViewManager.AddRootComponentAsync(rootComponent, "#app", Microsoft.AspNetCore.Components.ParameterView.Empty);
			});

		webViewManager.Navigate("/");

		return webViewManager;
	}

	public static GtkWebViewManager NewForWebView(WebView webView, IServiceProvider serviceProvider)
	{
		var options = serviceProvider.GetRequiredService<BlazorWebViewOptions>();
		return NewForWebView(webView, options, serviceProvider);
	}

	#region CopiedFromWebView2WebViewManager

	protected const string AppHostAddress = "localhost";

	protected static readonly string AppHostScheme = "app";

	/// <summary>
	/// Gets the application's base URI. Defaults to <c>app://localhost/</c>
	/// </summary>
	protected static string AppOrigin(string appHostScheme, string appHostAddress = AppHostAddress) => $"{appHostScheme}://{appHostAddress}/";

	protected static readonly Uri AppOriginUri = new(AppOrigin(AppHostScheme, AppHostAddress));

	protected Task<bool> _webviewReadyTask;
	protected string _contentRootRelativeToAppRoot;

	protected static string GetHeaderString(IDictionary<string, string> headers) =>
		string.Join(Environment.NewLine, headers.Select(kvp => $"{kvp.Key}: {kvp.Value}"));

	protected static string? GetWebView2UserDataFolder()
	{
		if (Assembly.GetEntryAssembly() is { } mainAssembly)
		{
			// In case the application is running from a non-writable location (e.g., program files if you're not running
			// elevated), use our own convention of %LocalAppData%\YourApplicationName.WebView2.
			// We may be able to remove this if https://github.com/MicrosoftEdge/WebView2Feedback/issues/297 is fixed.
			var applicationName = mainAssembly.GetName().Name;

			var result = Path.Combine(
				Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
				$"{applicationName}.WebView2");

			return result;
		}

		return null;
	}

	protected static void LaunchUriInExternalBrowser(Uri uri)
	{
		using var launchBrowser = new Process();

		launchBrowser.StartInfo.UseShellExecute = true;
		launchBrowser.StartInfo.FileName = uri.ToString();
		launchBrowser.Start();
	}

	#endregion
}