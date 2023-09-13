using System.IO;
using BlazorGtkApp;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Gtk;
using GtkSharp.BlazorWebKit;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebView.Gtk;
using WebViewAppShared;
using GtkWebViewManager = Microsoft.AspNetCore.Components.WebView.Gtk.GtkWebViewManager;

AppState _appState = new();

Application.Init();

// Create the parent window
var window = new Window(WindowType.Toplevel);
window.DefaultSize = new Gdk.Size(1024, 768);

window.DeleteEvent += (o, e) =>
{
	Application.Quit();
};

// Add the BlazorWebViews
var services1 = new ServiceCollection();
services1.AddGtkBlazorWebView();
#if DEBUG
services1.AddBlazorWebViewDeveloperTools();
#endif
services1.AddSingleton<AppState>(_appState);

var services2 = new ServiceCollection();
services2.AddGtkBlazorWebView();
#if DEBUG
services2.AddBlazorWebViewDeveloperTools();
#endif

services2.AddSingleton<AppState>(_appState);

var blazorWebView1 = new BlazorWebView();
blazorWebView1.HostPage = Path.Combine("wwwroot","index.html");
blazorWebView1.Services = services1.BuildServiceProvider();
blazorWebView1.RootComponents.Add<BlazorGtkApp.Main>("#app");
blazorWebView1.RootComponents.RegisterForJavaScript<MyDynamicComponent>("my-dynamic-root-component");


var customFilesBlazorWebView = new BlazorWebView();
customFilesBlazorWebView.HostPage = Path.Combine("wwwroot","customindex.html");
customFilesBlazorWebView.Services = services2.BuildServiceProvider();
customFilesBlazorWebView.RootComponents.Add<BlazorGtkApp.Main>("#app");

var nb = new Gtk.Notebook();
var tab1 = nb.AppendPage(blazorWebView1, new Label(nameof(blazorWebView1)));
var tab2 = nb.AppendPage(customFilesBlazorWebView, new Label(nameof(customFilesBlazorWebView)));


// this is the old stuff
var serviceProvider = new ServiceCollection()
	.AddBlazorWebViewOptions(new BlazorWebViewOptions() 
	{ 
		RootComponent = typeof(BlazorGtkApp.Main),
		HostPath = "wwwroot/index.html"
	})
	.AddLogging((lb) =>
	{
		lb.AddSimpleConsole(options =>
			{
				//options.ColorBehavior = Microsoft.Extensions.Logging.Console.LoggerColorBehavior.Disabled;
				//options.IncludeScopes = false;
				//options.SingleLine = true;
				options.TimestampFormat = "hh:mm:ss ";
			})
			.SetMinimumLevel(LogLevel.Information);
	})
	.BuildServiceProvider();

var webViewRaw = new WebKit.WebView();
var manager = GtkWebViewManager.NewForWebView(webViewRaw, serviceProvider);
var tab3 = nb.AppendPage(webViewRaw, new Label(nameof(webViewRaw)));


window.Add(nb);
window.ShowAll();

Application.Run();