using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using GtkSharpUpstream;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebView.Maui;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using WebKit;
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
#pragma warning disable RS0016

namespace BlazorWebKit;

public class GtkSharpWebViewManager : Microsoft.AspNetCore.Components.WebView.WebViewManager
{

    delegate void WebMessageHandler(IntPtr contentManager, IntPtr jsResult, IntPtr arg);

    const string _scheme = "app";
    readonly static Uri _baseUri = new Uri($"{_scheme}://localhost/");

    public GtkSharpWebViewManager(WebView webView, IServiceProvider serviceProvider)
        : base(serviceProvider, Dispatcher.CreateDefault(), _baseUri
            , new PhysicalFileProvider(serviceProvider.GetRequiredService<BlazorWebViewOptions>().ContentRoot)
            , new()
            , serviceProvider.GetRequiredService<BlazorWebViewOptions>().RelativeHostPath)
    {
        var options = serviceProvider.GetRequiredService<BlazorWebViewOptions>();
        _relativeHostPath = options.RelativeHostPath;
        _rootComponent = options.RootComponent;
        _logger = serviceProvider.GetService<ILogger<BlazorWebView>>();

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
            IntPtr.Zero,IntPtr.Zero, (global::GLib.ConnectFlags)0);

        WebView.UserContentManager.RegisterScriptMessageHandler("webview");

        Navigate("/");
    }

    public WebView WebView { get; init; }
    readonly WebMessageHandler HandleWebMessageDelegate;
    readonly string _relativeHostPath;
    readonly Type _rootComponent;
    readonly ILogger<BlazorWebView>? _logger;

    void HandleUriScheme(URISchemeRequest request)
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

    void HandleWebMessage(IntPtr contentManager, IntPtr jsResultHandle, IntPtr arg) {
            
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
                {
                }
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
}