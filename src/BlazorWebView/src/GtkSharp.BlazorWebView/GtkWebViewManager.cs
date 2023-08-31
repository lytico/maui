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

public partial class GtkWebViewManager : Microsoft.AspNetCore.Components.WebView.WebViewManager
{

	public delegate void WebMessageHandler(IntPtr contentManager, IntPtr jsResult, IntPtr arg);

	public WebView WebView { get; protected set; }

	protected ILogger<GtkWebViewManager>? _logger;

	public GtkWebViewManager(
		WebView webView, Type rootComponent, string scheme,
		IServiceProvider provider, Dispatcher dispatcher, Uri appBaseUri,
		IFileProvider fileProvider,
		JSComponentConfigurationStore jsComponents, string hostPageRelativePath) : base(provider, dispatcher, appBaseUri, fileProvider, jsComponents, hostPageRelativePath)

	{
		_logger = provider.GetService<ILogger<GtkWebViewManager>>();

		WebView = webView;

		// This is necessary to automatically serve the files in the `_framework` virtual folder.
		// Using `file://` will cause the webview to look for the `_framework` files on the file system,
		// and it won't find them.
		WebView.Context.RegisterUriScheme(scheme, request =>
		{
			if (request.Scheme != scheme)
			{
				throw new Exception($"Invalid scheme \"{request.Scheme}\"");
			}

			var uri = request.Uri;

			if (request.Path == "/")
			{
				uri += hostPageRelativePath;
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
		});

		Dispatcher.InvokeAsync(async () =>
		{
			await AddRootComponentAsync(rootComponent, "#app", ParameterView.Empty);
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

		WebView.UserContentManager.SignalConnectData<WebMessageHandler>("script-message-received::webview",
			(contentManager, jsResultHandle, arg) =>
			{

				var jsResult = new global::WebKit.Upstream.JavascriptResult(jsResultHandle);

				var jsValue = jsResult.JsValue;

				if (!jsValue.IsString) return;

				var s = jsValue.ToString();

				if (s is not null) {
					_logger?.LogDebug($"Received message `{s}`");

					try {
						MessageReceived(appBaseUri, s);
					} finally { }
				}
			},
			IntPtr.Zero, IntPtr.Zero, (global::GLib.ConnectFlags)0);

		WebView.UserContentManager.RegisterScriptMessageHandler("webview");

		Navigate("/");
	}

	public static GtkWebViewManager NewForWebView(WebView webView, IServiceProvider serviceProvider)
	{
		var options = serviceProvider.GetRequiredService<BlazorWebViewOptions>();
		var hostPath = options.HostPath;
		var rootComponent = options.RootComponent;
		var contentRoot = Path.GetDirectoryName(Path.GetFullPath(hostPath))!;
		const string scheme = "app";
		var baseUri = new Uri($"{scheme}://localhost/");

		var relativePath = Path.GetRelativePath(contentRoot, hostPath);

		var webViewManager = new GtkWebViewManager(webView,
			rootComponent,
			scheme,
			serviceProvider,
			Dispatcher.CreateDefault(),
			baseUri,
			new PhysicalFileProvider(contentRoot),
			new(),
			relativePath);

		return webViewManager;
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

}