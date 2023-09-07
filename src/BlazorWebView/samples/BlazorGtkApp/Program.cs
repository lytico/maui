using BlazorGtkApp;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Gtk;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebView.Gtk;
using WebViewAppShared;

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
blazorWebView1.HostPage = @"wwwroot\index.html";
blazorWebView1.Services = services1.BuildServiceProvider();
blazorWebView1.RootComponents.Add<BlazorGtkApp.Main>("#app");
blazorWebView1.RootComponents.RegisterForJavaScript<MyDynamicComponent>("my-dynamic-root-component");

var customFilesBlazorWebView = new BlazorWebView();
customFilesBlazorWebView.HostPage = @"wwwroot\customindex.html";
customFilesBlazorWebView.Services = services2.BuildServiceProvider();
customFilesBlazorWebView.RootComponents.Add<BlazorGtkApp.Main>("#app");

window.Add(blazorWebView1);


window.ShowAll();

Application.Run();