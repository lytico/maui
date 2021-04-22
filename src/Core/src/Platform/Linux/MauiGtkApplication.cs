using System;
using GLib;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Maui.Hosting;
using Microsoft.Maui.LifecycleEvents;
using Gtk;
using Application = GLib.Application;

namespace Microsoft.Maui
{

	public class MauiGtkApplication<TStartup> : MauiGtkApplication
		where TStartup : IStartup, new()
	{

		public void Run()
		{
			Launch(new EventArgs());
		}

		protected void RegisterLifecycleEvents(Gtk.Application app)
		{

			app.Startup += OnStartup;
			app.Shutdown += OnShutdown;
			app.Opened += OnOpened;
			app.WindowAdded += OnWindowAdded;
			app.Activated += OnActivated;
			app.WindowRemoved += OnWindowRemoved;
			app.CommandLine += OnCommandLine;

		}

		void OnStartup(object sender, EventArgs args)
		{
			Services?.InvokeLifecycleEvents<LinuxLifecycle.OnStartup>(del => del(CurrentGtkApplication, args));
		}

		void OnOpened(object o, OpenedArgs args)
		{
			Services?.InvokeLifecycleEvents<LinuxLifecycle.OnOpened>(del => del(CurrentGtkApplication, args));
		}

		void OnActivated(object sender, EventArgs args)
		{
			Services?.InvokeLifecycleEvents<LinuxLifecycle.OnApplicationActivated>(del => del(CurrentGtkApplication, args));
		}

		void OnShutdown(object sender, EventArgs args)
		{
			Services?.InvokeLifecycleEvents<LinuxLifecycle.OnShutdown>(del => del(CurrentGtkApplication, args));
		}

		void OnCommandLine(object o, CommandLineArgs args)
		{
			;
		}

		void OnWindowRemoved(object o, WindowRemovedArgs args)
		{
			;
		}

		void OnWindowAdded(object o, WindowAddedArgs args)
		{
			;
		}

		protected void Launch(EventArgs args)
		{

			Gtk.Application.Init();
			CurrentGtkApplication = new Gtk.Application(Name ?? string.Empty, ApplicationFlags.None);
			CurrentGtkApplication.Register(GLib.Cancellable.Current);
			RegisterLifecycleEvents(CurrentGtkApplication);
			
			Current = this;

			MainWindow = new MauiGtkMainWindow();

			var startup = new TStartup();

			var host = startup
			   .CreateAppHostBuilder()
			   .ConfigureServices(ConfigureNativeServices)
			   .ConfigureUsing(startup)
			   .Build();

			Services = host.Services;
			Application = Services.GetRequiredService<IApplication>();

			var mauiContext = new MauiContext(Services);

			var activationState = new ActivationState(mauiContext);
			var window = Application.CreateWindow(activationState);
			window.MauiContext = mauiContext;

			var content = (window.Page as IView) ?? window.Page.View;

			var canvas = CreateRootContainer();

			canvas.Child = content.ToNative(window.MauiContext);

			MainWindow.Child = canvas;

			CurrentGtkApplication.AddWindow(MainWindow);

			MainWindow.ShowAll();

			
			
			Gtk.Application.Run();

			Services?.InvokeLifecycleEvents<LinuxLifecycle.OnLaunched>(del => del(CurrentGtkApplication, args));

		}

		Container CreateRootContainer()
		{
			var b = new Box(Orientation.Horizontal, 0);

			return b;
		}

		void ConfigureNativeServices(HostBuilderContext ctx, IServiceCollection services)
		{ }

	}

	public abstract class MauiGtkApplication
	{

		protected MauiGtkApplication()
		{ }

		public string? Name { get; set; }

		public static Gtk.Application CurrentGtkApplication { get; internal set; } = null!;

		public static MauiGtkApplication Current { get; internal set; } = null!;

		public MauiGtkMainWindow MainWindow { get; protected set; } = null!;

		public IServiceProvider Services { get; protected set; } = null!;

		public IApplication Application { get; protected set; } = null!;

	}

}