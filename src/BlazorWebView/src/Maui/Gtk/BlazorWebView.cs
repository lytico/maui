using System;
using System.Diagnostics.CodeAnalysis;
using WebKit;
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace BlazorWebKit;

[SuppressMessage("ApiDesign", "RS0016:Öffentliche Typen und Member der deklarierten API hinzufügen")]
public class BlazorWebView : WebView
{
  
    public BlazorWebView(IServiceProvider serviceProvider)
        : base ()
    {
        _manager = new GtkSharpWebViewManager(this, serviceProvider);
    }

    public BlazorWebView(IntPtr raw, IServiceProvider serviceProvider)
        : base (raw)
    {
        _manager = new GtkSharpWebViewManager(this, serviceProvider);
    }

    public BlazorWebView(WebContext context, IServiceProvider serviceProvider)
        : base (context)
    {
        _manager = new GtkSharpWebViewManager(this, serviceProvider);
    }

    public BlazorWebView(WebView web_view, IServiceProvider serviceProvider)
        : base (web_view)
    {
        _manager = new GtkSharpWebViewManager(this, serviceProvider);
    }

    public BlazorWebView(Settings settings, IServiceProvider serviceProvider)
        : base (settings)
    {
        _manager = new GtkSharpWebViewManager(this, serviceProvider);
    }

    public BlazorWebView(UserContentManager user_content_manager, IServiceProvider serviceProvider)
        : base (user_content_manager)
    {
        _manager = new GtkSharpWebViewManager(this, serviceProvider);
    }

    readonly GtkSharpWebViewManager _manager;
}