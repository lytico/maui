using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;
using Gtk;
using GtkSharpUpstream;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using WebKit;
using UserScript = WebKit.Upstream.UserScript;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

#pragma warning disable CS8601 // Possible null reference assignment.
#pragma warning disable CS8604 // Possible null reference argument.

namespace GtkSharp.BlazorWebKit;

[SuppressMessage("ApiDesign", "RS0016:Öffentliche Typen und Member der deklarierten API hinzufügen")]
public partial class GtkWebViewManager : Microsoft.AspNetCore.Components.WebView.WebViewManager
{

	protected const string AppHostAddress = "localhost";

	protected static readonly string AppHostScheme = "app";

	/// <summary>
	/// Gets the application's base URI. Defaults to <c>app://localhost/</c>
	/// </summary>
	protected static string AppOrigin(string appHostScheme, string appHostAddress = AppHostAddress) => $"{appHostScheme}://{appHostAddress}/";

	protected static readonly Uri AppOriginUri = new(AppOrigin(AppHostScheme, AppHostAddress));

	protected Task<bool>? WebviewReadyTask;

	protected string MessageQueueId = "webview";

	string _hostPageRelativePath;
	Uri _appBaseUri;

	UserScript? _script;
	
	public delegate void WebMessageHandler(IntPtr contentManager, IntPtr jsResult, IntPtr arg);

	public WebView? WebView { get; protected set; }

	protected ILogger<GtkWebViewManager>? Logger;

	protected GtkWebViewManager(IServiceProvider provider, Dispatcher dispatcher, Uri appBaseUri, IFileProvider fileProvider, JSComponentConfigurationStore jsComponents, string hostPageRelativePath) :
		base(provider, dispatcher, appBaseUri, fileProvider, jsComponents, hostPageRelativePath)
	{
		_appBaseUri = appBaseUri;
		_hostPageRelativePath = hostPageRelativePath;
	}

	delegate bool TryGetResponseContentHandler(string uri, bool allowFallbackOnHostPage, out int statusCode, out string statusMessage, out Stream content, out IDictionary<string, string> headers);

	static readonly Dictionary<IntPtr, (string _hostPageRelativePath, TryGetResponseContentHandler tryGetResponseContent)> UriSchemeRequestHandlers = new();

	static bool HandleUriSchemeRequestIsRegistered = false;

	/// <summary>
	/// RegisterUriScheme can only called once per scheme
	/// so it's needed to have a list of all WebViews registered
	/// </summary>
	/// <param name="request"></param>
	/// <exception cref="Exception"></exception>
	static void HandleUriSchemeRequest(URISchemeRequest request)
	{
		if (!UriSchemeRequestHandlers.TryGetValue(request.WebView.Handle, out var uriSchemeHandler))
		{
			throw new Exception($"Invalid scheme \"{request.Scheme}\"");
		}

		var uri = request.Uri;

		if (request.Path == "/")
		{
			uri += uriSchemeHandler._hostPageRelativePath;
		}

		if (uriSchemeHandler.tryGetResponseContent(uri, false, out int statusCode, out string statusMessage, out Stream content, out IDictionary<string, string> headers))
		{
			using var inputStream = content.AsInputStream();
			request.Finish(inputStream, content.Length, headers["Content-Type"]);
		}
		else
		{
			throw new Exception($"Failed to serve \"{uri}\". {statusCode} - {statusMessage}");
		}
	}

	void RegisterUriSchemeRequestHandler()
	{
		if (WebView is not { })
			return;

		if (!UriSchemeRequestHandlers.TryGetValue(WebView.Handle, out var uriSchemeHandler))
		{
			UriSchemeRequestHandlers.Add(WebView.Handle, (_hostPageRelativePath, TryGetResponseContent));
		}
	}

	protected override void NavigateCore(Uri absoluteUri)
	{
		if (WebView is not { })
			return;

		Logger?.LogInformation($"Navigating to \"{absoluteUri}\"");
		var loadUri = absoluteUri.ToString();

		WebView.LoadUri(loadUri);
	}

	void SignalHandler(IntPtr contentManager, IntPtr jsResultHandle, IntPtr arg)
	{
		var jsResult = new WebKit.Upstream.JavascriptResult(jsResultHandle);

		var jsValue = jsResult.JsValue;

		if (!jsValue.IsString) return;

		var s = jsValue.ToString();

		if (s is not null)
		{
			Logger?.LogDebug($"Received message `{s}`");

			try
			{
				MessageReceived(_appBaseUri, s);
			}
			finally
			{ }
		}
	}

	public string JsScript(string messageQueueId) =>
		"""
		window.__receiveMessageCallbacks = [];

		window.__dispatchMessageCallback = function(message) {
		   window.__receiveMessageCallbacks.forEach(function(callback) { callback(message); });
		};

		window.external = {
		   sendMessage: function(message) {
		"""
		+
		$"""
		        window.webkit.messageHandlers.{MessageQueueId}.postMessage(message);
		 """
		+
		"""
		   },
		   receiveMessage: function(callback) {
		       window.__receiveMessageCallbacks.push(callback);
		   }
		};
		""";

	protected virtual void Attach()
	{
		if (WebView is not { })
			throw new ArgumentException();

		if (!HandleUriSchemeRequestIsRegistered)
		{
			WebView.Context.RegisterUriScheme(AppHostScheme, HandleUriSchemeRequest);
			HandleUriSchemeRequestIsRegistered = true;
		}

		RegisterUriSchemeRequestHandler();

		var jsScript = JsScript(MessageQueueId);

		_script = new WebKit.Upstream.UserScript(
			jsScript,
			UserContentInjectedFrames.AllFrames,
			UserScriptInjectionTime.Start,
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
			null, null);
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.

		WebView.UserContentManager.AddScript(_script);

		WebView.UserContentManager.SignalConnectData<WebMessageHandler>($"script-message-received::{MessageQueueId}",
			SignalHandler,
			IntPtr.Zero, IntPtr.Zero, (global::GLib.ConnectFlags)0);

		WebView.UserContentManager.RegisterScriptMessageHandler(MessageQueueId);
	}

	protected virtual void Detach()
	{
		if (WebView is not { })
			return;

		WebView.Context.RemoveSignalHandler($"script-message-received::{MessageQueueId}", SignalHandler);
		WebView.UserContentManager.UnregisterScriptMessageHandler(MessageQueueId);
		WebView.UserContentManager.RemoveScript(_script);
		UriSchemeRequestHandlers.Remove(WebView.Handle);
	}

	protected override void SendMessage(string message)
	{
		if (WebView is not { })
			return;

		Logger?.LogDebug($"Dispatching `{message}`");

		var script = $"__dispatchMessageCallback(\"{HttpUtility.JavaScriptStringEncode(message)}\")";

		WebView.RunJavascript(script);
	}

	protected override async ValueTask DisposeAsyncCore()
	{
		Detach();
		await base.DisposeAsyncCore();
	}

	protected static string GetHeaderString(IDictionary<string, string> headers) =>
		string.Join(Environment.NewLine, headers.Select(kvp => $"{kvp.Key}: {kvp.Value}"));

	protected static string? GetWebView2UserDataFolder()
	{
		if (Assembly.GetEntryAssembly() is { } mainAssembly)
		{
			// In case the application is running from a non-writable location (e.g., program files if you're not running
			// elevated), use our own convention of %LocalAppData%\YourApplicationName.WebView.
			var applicationName = mainAssembly.GetName().Name;

			var result = Path.Combine(
				Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
				$"{applicationName}.{nameof(WebView)}");

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

}