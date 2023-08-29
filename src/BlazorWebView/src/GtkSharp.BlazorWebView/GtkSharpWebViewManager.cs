using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;
using GtkSharpUpstream;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using WebKit;

#pragma warning disable CS8601 // Possible null reference assignment.
#pragma warning disable CS8604 // Possible null reference argument.

namespace GtkSharp.BlazorWebKit;

public class GtkSharpWebViewManager : Microsoft.AspNetCore.Components.WebView.WebViewManager
{

	public delegate void WebMessageHandler(IntPtr contentManager, IntPtr jsResult, IntPtr arg);

	protected string _scheme = "app";

	protected Uri _baseUri => new Uri($"{_scheme}://localhost/");

	protected void Initialize(WebView webView, BlazorWebViewOptions options, ILogger<GtkSharpWebViewManager>? logger)
	{
		_relativeHostPath = options.RelativeHostPath;

		_rootComponent = options.RootComponent;
		_logger = logger;

		WebView = webView;
		HandleWebMessageDelegate = HandleWebMessage;

		// This is necessary to automatically serve the files in the `_framework` virtual folder.
		// Using `file://` will cause the webview to look for the `_framework` files on the file system,
		// and it won't find them.
		WebView.Context.RegisterUriScheme(_scheme, HandleUriScheme);

		Dispatcher.InvokeAsync(async () =>
		{
			await AddRootComponentAsync(_rootComponent, "#app", ParameterView.Empty);
		});

		var script = new global::WebKit.Upstream.UserScript(
			"""
			window.__receiveMessageCallbacks = [];

			window.__dispatchMessageCallback = function(message) {
			    window.__receiveMessageCallbacks.forEach(function(callback) { callback(message); });
			};

			window.external = {
			    sendMessage: function(message) {
			        window.webkit.messageHandlers.webview.postMessage(message);
			    },
			    receiveMessage: function(callback) {
			        window.__receiveMessageCallbacks.push(callback);
			    }
			};
			""",
			UserContentInjectedFrames.AllFrames,
			UserScriptInjectionTime.Start,
			null, null);

		WebView.UserContentManager.AddScript(script);

		WebView.UserContentManager.SignalConnectData("script-message-received::webview",
			HandleWebMessageDelegate,
			IntPtr.Zero, IntPtr.Zero, (global::GLib.ConnectFlags)0);

		WebView.UserContentManager.RegisterScriptMessageHandler("webview");

		Navigate("/");
	}

	public GtkSharpWebViewManager(WebView webView, IServiceProvider serviceProvider)
		: base(serviceProvider, Dispatcher.CreateDefault(),
			serviceProvider.GetRequiredService<BlazorWebViewOptions>().BaseUri,
			new PhysicalFileProvider(serviceProvider.GetRequiredService<BlazorWebViewOptions>().ContentRoot),
			new(),
			serviceProvider.GetRequiredService<BlazorWebViewOptions>().RelativeHostPath)
	{
		var options = serviceProvider.GetRequiredService<BlazorWebViewOptions>();
		var logger = serviceProvider.GetService<ILogger<GtkSharpWebViewManager>>();

		Initialize(webView, options, logger);
	}

	public GtkSharpWebViewManager(WebView webView, BlazorWebViewOptions options,
		IServiceProvider provider, Dispatcher dispatcher, Uri appBaseUri, IFileProvider fileProvider, JSComponentConfigurationStore jsComponents, string hostPageRelativePath)
		: base(provider, dispatcher, appBaseUri, fileProvider, jsComponents, hostPageRelativePath)
	{

		Initialize(webView, options, default);
	}

	public WebView WebView { get; protected set; }

	protected WebMessageHandler HandleWebMessageDelegate;
	protected string _relativeHostPath;
	protected Type _rootComponent;
	protected ILogger<GtkSharpWebViewManager>? _logger;

	protected void HandleUriScheme(URISchemeRequest request)
	{
		if (request.Scheme != _scheme)
		{
			throw new Exception($"Invalid scheme \"{request.Scheme}\"");
		}

		var uri = request.Uri;

		if (request.Path == "/")
		{
			uri += _relativeHostPath;
		}

		_logger?.LogInformation($"Fetching \"{uri}\"");

		if (TryGetResponseContent(uri, false, out int statusCode, out string statusMessage, out Stream content, out IDictionary<string, string> headers))
		{

			using var inputStream = content.AsInputStream();
			request.Finish(inputStream, content.Length, headers["Content-Type"]);

		}
		else
		{
			throw new Exception($"Failed to serve \"{uri}\". {statusCode} - {statusMessage}");
		}
	}

	protected void HandleWebMessage(IntPtr contentManager, IntPtr jsResultHandle, IntPtr arg)
	{

		var jsResult = new global::WebKit.Upstream.JavascriptResult(jsResultHandle);

		var jsValue = jsResult.JsValue;

		if (jsValue.IsString)
		{

			var s = jsValue.ToString();

			if (s is not null)
			{
				_logger?.LogDebug($"Received message `{s}`");

				try
				{
					MessageReceived(_baseUri, s);
				}
				finally
				{ }
			}
		}

	}

	protected override void NavigateCore(Uri absoluteUri)
	{
		_logger?.LogInformation($"Navigating to \"{absoluteUri}\"");

		WebView.LoadUri(absoluteUri.ToString());
	}

	protected override void SendMessage(string message)
	{
		_logger?.LogDebug($"Dispatching `{message}`");

		var script = $"__dispatchMessageCallback(\"{HttpUtility.JavaScriptStringEncode(message)}\")";

		WebView.RunJavascript(script);
	}

	#region CopiedFromWebView2WebViewManager

	// Using an IP address means that WebView doesn't wait for any DNS resolution,
	// making it substantially faster. Note that this isn't real HTTP traffic, since
	// we intercept all the requests within this origin.
	protected static readonly string AppHostAddress = "0.0.0.0";

	/// <summary>
	/// Gets the application's base URI. Defaults to <c>https://0.0.0.0/</c>
	/// </summary>
	protected static readonly string AppOrigin = $"https://{AppHostAddress}/";

	protected static readonly Uri AppOriginUri = new(AppOrigin);

	protected readonly Task<bool> _webviewReadyTask;
	protected readonly string _contentRootRelativeToAppRoot;

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

	protected void LaunchUriInExternalBrowser(Uri uri)
	{
		using var launchBrowser = new Process();

		launchBrowser.StartInfo.UseShellExecute = true;
		launchBrowser.StartInfo.FileName = uri.ToString();
		launchBrowser.Start();

	}

	#endregion

}