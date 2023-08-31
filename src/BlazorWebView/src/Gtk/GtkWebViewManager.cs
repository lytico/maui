using System;
using GtkSharp.BlazorWebKit;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.FileProviders;

namespace Microsoft.AspNetCore.Components.WebView.Gtk;
#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value

public class GtkWebViewManager : GtkSharp.BlazorWebKit.GtkWebViewManager
{

	#region CopiedFromWebView2WebViewManager

	protected readonly Action<UrlLoadingEventArgs>? _urlLoading;
	protected readonly Action<BlazorWebViewInitializingEventArgs>? _blazorWebViewInitializing;
	protected readonly Action<BlazorWebViewInitializedEventArgs>? _blazorWebViewInitialized;

	internal readonly BlazorWebViewDeveloperTools? _developerTools;

	internal void ApplyDefaultWebViewSettings(BlazorWebViewDeveloperTools devTools)
	{
		WebView.Settings.EnableDeveloperExtras = devTools.Enabled;

	}

	#endregion

	public GtkWebViewManager(WebKit.WebView webView, Type rootComponent, string scheme, IServiceProvider provider, Dispatcher dispatcher, Uri appBaseUri, IFileProvider fileProvider, JSComponentConfigurationStore jsComponents, string hostPageRelativePath) : 
		base(webView, rootComponent, scheme, provider, dispatcher, appBaseUri, fileProvider, jsComponents, hostPageRelativePath) { }

}