using System;
using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Maui.Hosting;
using Microsoft.Maui.LifecycleEvents;
using Gtk;

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

			if (true)
#pragma warning disable 162
			{
				// this has to be after all event registration?
				app.Register(GLib.Cancellable.Current);
			}
#pragma warning restore 162
			app.Startup += OnStartup!;
			app.Shutdown += OnShutdown!;
			app.Opened += OnOpened;
			app.WindowAdded += OnWindowAdded;
			app.Activated += OnActivated!;
			app.WindowRemoved += OnWindowRemoved!;
			app.CommandLine += OnCommandLine!;

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

		protected void OnOpened(object o, GLib.OpenedArgs args)
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

		protected void OnCommandLine(object o, GLib.CommandLineArgs args)
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

		// https://developer.gnome.org/gio/stable/GApplication.html#g-application-id-is-valid
		// TODO: find a better algo for id
		public string ApplicationId => $"{typeof(TStartup).Namespace}.{typeof(TStartup).Name}.{base.Name}".PadRight(255, ' ').Substring(0, 255).Trim();

		protected void Launch(EventArgs args)
		{

			Gtk.Application.Init();
			var app = new Gtk.Application(ApplicationId, GLib.ApplicationFlags.None);
			RegisterLifecycleEvents(app);

			CurrentGtkApplication = app;

			Current = this;

			MainWindow = new MauiGtkMainWindow();
			app.AddWindow(MainWindow);

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

			MainWindow.ShowAll();

			((GLib.Application)app).Run();

			Gtk.Application.Run();
			Services?.InvokeLifecycleEvents<LinuxLifecycle.OnLaunched>(del => del(CurrentGtkApplication, args));

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

		string? _name;

		// https://developer.gnome.org/gio/stable/GApplication.html#g-application-id-is-valid
		public string? Name
		{
			get => _name ??= $"A{Guid.NewGuid()}";
			set { _name = value; }
		}

		// https://developer.gnome.org/gtk3/stable/GtkApplication.html
		public static Gtk.Application CurrentGtkApplication { get; internal set; } = null!;

		public static MauiGtkApplication Current { get; internal set; } = null!;

		public MauiGtkMainWindow MainWindow { get; protected set; } = null!;

		public IServiceProvider Services { get; protected set; } = null!;

		public IApplication Application { get; protected set; } = null!;

	}

}