using System;
using System.Collections.Generic;
using Microsoft.Maui.Hosting;
using Microsoft.Maui.LifecycleEvents;

namespace Microsoft.Maui
{
	public abstract class MauiGTKApplication : IPlatformApplication
	{
		protected abstract MauiApp CreateMauiApp();

		public void Main(string[] args)
		{
			//// Windows running on a different thread will "launch" the app again
			//if (Application != null)
			//{
			//	Services.InvokeLifecycleEvents<WindowsLifecycle.OnLaunching>(del => del(this, args));
			//	Services.InvokeLifecycleEvents<WindowsLifecycle.OnLaunched>(del => del(this, args));
			//	return;
			//}

			//Current = new IApplication();
			Gtk.Application.Init();

			//IPlatformApplication.Current = this;
			var mauiApp = CreateMauiApp();

			var rootContext = new MauiContext(mauiApp.Services);

			var applicationContext = rootContext.MakeApplicationScope(null!);

			Services = applicationContext.Services;

			// Services.InvokeLifecycleEvents<WindowsLifecycle.OnLaunching>(del => del(this, args));

			// Application = Services.GetRequiredService<IApplication>();

			// this.SetApplicationHandler(Application, applicationContext);

			this.CreatePlatformWindow();

			//Services.InvokeLifecycleEvents<WindowsLifecycle.OnLaunched>(del => del(this, args));
		}

		// private static MauiGTKApplication _instance = new MauiGTKApplication();

		//protected override void OnLaunched(UI.Xaml.LaunchActivatedEventArgs args)
		//{
		//	// Windows running on a different thread will "launch" the app again
		//	if (Application != null)
		//	{
		//		Services.InvokeLifecycleEvents<WindowsLifecycle.OnLaunching>(del => del(this, args));
		//		Services.InvokeLifecycleEvents<WindowsLifecycle.OnLaunched>(del => del(this, args));
		//		return;
		//	}

		//	IPlatformApplication.Current = this;
		//	var mauiApp = CreateMauiApp();

		//	var rootContext = new MauiContext(mauiApp.Services);

		//	var applicationContext = rootContext.MakeApplicationScope(this);

		//	Services = applicationContext.Services;

		//	Services.InvokeLifecycleEvents<WindowsLifecycle.OnLaunching>(del => del(this, args));

		//	Application = Services.GetRequiredService<IApplication>();

		//	this.SetApplicationHandler(Application, applicationContext);

		//	this.CreatePlatformWindow(Application, args);

		//	Services.InvokeLifecycleEvents<WindowsLifecycle.OnLaunched>(del => del(this, args));
		//}

		public static IApplication Current { get; set; } = null!;

		// public UI.Xaml.LaunchActivatedEventArgs LaunchActivatedEventArgs { get; protected set; } = null!;

		public IServiceProvider Services { get; protected set; } = null!;

		public IApplication Application { get; protected set; } = null!;
	}

	//class AppTemplate : IApplication
	//{
	//	public IReadOnlyList<IWindow> Windows => throw new NotImplementedException();

	//	public IElementHandler? Handler { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

	//	public IElement? Parent => throw new NotImplementedException();

	//	public void CloseWindow(IWindow window)
	//	{
	//		throw new NotImplementedException();
	//	}

	//	public IWindow CreateWindow(IActivationState? activationState)
	//	{
	//		throw new NotImplementedException();
	//	}

	//	public void OpenWindow(IWindow window)
	//	{
	//		throw new NotImplementedException();
	//	}

	//	public void ThemeChanged()
	//	{
	//		throw new NotImplementedException();
	//	}
	//}
}