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

	public static GtkWebViewManager NewForWebView(WebView webView, IServiceProvider serviceProvider)
	{
		var options = serviceProvider.GetRequiredService<BlazorWebViewOptions>();
		var hostPath = options.HostPath;
		var rootComponent = options.RootComponent;
		var contentRoot = Path.GetDirectoryName(Path.GetFullPath(hostPath))!;
		const string scheme = "app";
		var baseUri = new Uri($"{scheme}://localhost/");

		var relativePath = Path.GetRelativePath(contentRoot, hostPath);
		var dispatcher = Microsoft.AspNetCore.Components.Dispatcher.CreateDefault();

		var webViewManager = new GtkWebViewManager(webView,
			scheme,
			serviceProvider,
			dispatcher,
			baseUri,
			new PhysicalFileProvider(contentRoot),
			new(),
			relativePath);

		webViewManager.Attach();

		dispatcher.InvokeAsync(async () =>
		{
			await webViewManager.AddRootComponentAsync(rootComponent, "#app", Microsoft.AspNetCore.Components.ParameterView.Empty);
		});

		webViewManager.Navigate("/");

		return webViewManager;
	}

	#region CopiedFromWebView2WebViewManager

	// Using an IP address means that WebView doesn't wait for any DNS resolution,
	// making it substantially faster. Note that this isn't real HTTP traffic, since
	// we intercept all the requests within this origin.
	protected static readonly string AppHostAddress = "localhost";

	/// <summary>
	/// Gets the application's base URI. Defaults to <c>https://0.0.0.0/</c>
	/// </summary>
	protected static readonly string AppOrigin = $"https://{AppHostAddress}/";

	protected static readonly Uri AppOriginUri = new(AppOrigin);

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