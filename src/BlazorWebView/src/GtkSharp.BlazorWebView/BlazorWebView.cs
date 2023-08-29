using System;
using WebKit;

namespace GtkSharp.BlazorWebKit;

public class BlazorWebView : WebView
{

	public BlazorWebView(GtkSharpWebViewManager manager) : base()
	{
		_manager = manager;
	}

	public BlazorWebView(IServiceProvider serviceProvider) : base()
	{
		_manager = new GtkSharpWebViewManager(this, serviceProvider);
	}

	public BlazorWebView(IntPtr raw, IServiceProvider serviceProvider) : base(raw)
	{
		_manager = new GtkSharpWebViewManager(this, serviceProvider);
	}

	public BlazorWebView(WebContext context, IServiceProvider serviceProvider) : base(context)
	{
		_manager = new GtkSharpWebViewManager(this, serviceProvider);
	}

	public BlazorWebView(WebView web_view, IServiceProvider serviceProvider) : base(web_view)
	{
		_manager = new GtkSharpWebViewManager(this, serviceProvider);
	}

	public BlazorWebView(Settings settings, IServiceProvider serviceProvider) : base(settings)
	{
		_manager = new GtkSharpWebViewManager(this, serviceProvider);
	}

	public BlazorWebView(UserContentManager user_content_manager, IServiceProvider serviceProvider)
		: base(user_content_manager)
	{
		_manager = new GtkSharpWebViewManager(this, serviceProvider);
	}

	readonly GtkSharpWebViewManager _manager;

}