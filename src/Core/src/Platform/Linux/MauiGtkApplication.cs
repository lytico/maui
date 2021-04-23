using System;
using System.Diagnostics;
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
			isfired = false;

			app.Startup += OnStartup!;
			app.Shutdown += OnShutdown!;
			app.Opened += OnOpened;
			app.WindowAdded += OnWindowAdded;
			app.Activated += OnActivated!;
			app.WindowRemoved += OnWindowRemoved!;
			app.CommandLine += OnCommandLine!;

			if (false)
#pragma warning disable 162
			{
				// this has to be after all event registration
				app.Register(GLib.Cancellable.Current);
			}
#pragma warning restore 162
		}

#if DEBUG
		static bool isfired = false;
#endif
		protected void OnStartup(object sender, EventArgs args)
		{

#if DEBUG
			Trace.WriteLine($"{nameof(OnStartup)}");
			isfired = true;
#endif
			Services.InvokeLifecycleEvents<LinuxLifecycle.OnStartup>(del => del(CurrentGtkApplication, args));
		}

		protected void OnOpened(object o, OpenedArgs args)
		{
			Services?.InvokeLifecycleEvents<LinuxLifecycle.OnOpened>(del => del(CurrentGtkApplication, args));
		}

		protected void OnActivated(object sender, EventArgs args)
		{
			Services?.InvokeLifecycleEvents<LinuxLifecycle.OnApplicationActivated>(del => del(CurrentGtkApplication, args));
		}

		protected void OnShutdown(object sender, EventArgs args)
		{
			Services?.InvokeLifecycleEvents<LinuxLifecycle.OnShutdown>(del => del(CurrentGtkApplication, args));
		}

		protected void OnCommandLine(object o, CommandLineArgs args)
		{
			;
		}

		protected void OnWindowRemoved(object o, WindowRemovedArgs args)
		{
			;
		}

		protected void OnWindowAdded(object o, WindowAddedArgs args)
		{
			;
		}

		protected void Launch(EventArgs args)
		{

			Gtk.Application.Init();
			var app = new Gtk.Application(Name ?? string.Empty, ApplicationFlags.None);
			RegisterLifecycleEvents(app);

			CurrentGtkApplication = app;

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

			app.AddWindow(MainWindow);

			MainWindow.ShowAll();

			((Application)app).Run();

			Services?.InvokeLifecycleEvents<LinuxLifecycle.OnLaunched>(del => del(CurrentGtkApplication, args));

			Gtk.Application.Run();

#if DEBUG
			if (!isfired)
			{
				Trace.WriteLine("lifecycle broken");
			}
#endif
		}

		protected Container CreateRootContainer()
		{
			var b = new Box(Orientation.Horizontal, 0);

			return b;
		}

		protected void ConfigureNativeServices(HostBuilderContext ctx, IServiceCollection services)
		{ }

	}

	public abstract class MauiGtkApplication
	{

		protected MauiGtkApplication()
		{ }

		public string? Name { get; set; }

		// https://developer.gnome.org/gtk3/stable/GtkApplication.html
		public static Gtk.Application CurrentGtkApplication { get; internal set; } = null!;

		public static MauiGtkApplication Current { get; internal set; } = null!;

		public MauiGtkMainWindow MainWindow { get; protected set; } = null!;

		public IServiceProvider Services { get; protected set; } = null!;

		public IApplication Application { get; protected set; } = null!;

	}

}