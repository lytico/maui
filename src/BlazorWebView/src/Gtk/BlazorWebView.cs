using System;
using WebKit;

namespace Microsoft.AspNetCore.Components.WebView.Gtk;

/// <inheritdoc />
public class BlazorWebView : GtkSharp.BlazorWebKit.BlazorWebView
{

	public BlazorWebView(IServiceProvider serviceProvider) : base(serviceProvider) { }

	public BlazorWebView(IntPtr raw, IServiceProvider serviceProvider) : base(raw, serviceProvider) { }

	public BlazorWebView(WebContext context, IServiceProvider serviceProvider) : base(context, serviceProvider) { }

	public BlazorWebView(WebKit.WebView web_view, IServiceProvider serviceProvider) : base(web_view, serviceProvider) { }

	public BlazorWebView(Settings settings, IServiceProvider serviceProvider) : base(settings, serviceProvider) { }

	public BlazorWebView(UserContentManager user_content_manager, IServiceProvider serviceProvider) : base(user_content_manager, serviceProvider) { }

}