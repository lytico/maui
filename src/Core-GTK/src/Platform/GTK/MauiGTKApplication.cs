using System;
using System.Collections.Generic;
using Gtk;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.Hosting;
using Microsoft.Maui.LifecycleEvents;
using Microsoft.Maui.Platform;

namespace Microsoft.Maui
{
	public abstract class MauiGTKApplication : IPlatformApplication
	{
		protected abstract MauiApp CreateMauiApp();

#if NETSTANDARD2_0     
		/// <summary>                  
		/// Gets the current IPlatformApplication.
		/// This must be set in each implementation manually, as we can't
		/// have a true static be used in the implementation.
		/// </summary>
		public static MauiGTKApplication? CurrentApp { get; set; }
#endif

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

#if NETSTANDARD2_0
			CurrentApp = this;
#else
			IPlatformApplication.Current = this;
#endif

			var mauiApp = CreateMauiApp();

			var rootContext = new MauiContext(mauiApp.Services);

			var applicationContext = rootContext.MakeApplicationScope(null!);

			Services = applicationContext.Services;

			Services.InvokeLifecycleEvents<GTKLifecycle.OnLaunching>(del => del(this));

			if (Services.GetRequiredService<IApplication>() != null)
			{
				Application = Services.GetRequiredService<IApplication>();
			}

			// Application = Services.GetRequiredService<IApplication>();

			this.SetApplicationHandler(Application, applicationContext);

			this.CreatePlatformWindow(Application);
			// this.CreatePlatformWindow(this);

			Services.InvokeLifecycleEvents<GTKLifecycle.OnLaunched>(del => del(this));

			Gtk.Application.Run();

			// System.Windows.Forms.Application.Exit();
			// Services.InvokeLifecycleEvents<GTKLifecycle.OnClosed>(del => del(this));
			// this.Quit();
			// MauiGTKApplication.Current.Quit();
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

		public string? WindowCssFileName {  get; set; } = null;
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