using System;
using System.Diagnostics;
using Gdk;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Maui.Hosting;
using Microsoft.Maui.LifecycleEvents;
using Gtk;
using Microsoft.Maui.Graphics;

namespace Microsoft.Maui
{

	public class MauiGtkApplication<TStartup> : MauiGtkApplication
		where TStartup : IStartup, new()
	{

		protected override IStartup OnCreateStartup() => new TStartup();

		// https://developer.gnome.org/gio/stable/GApplication.html#g-application-id-is-valid
		// TODO: find a better algo for id
		public override string ApplicationId => $"{typeof(TStartup).Namespace}.{typeof(TStartup).Name}.{Name}".PadRight(255, ' ').Substring(0, 255).Trim();

	}

	public abstract class MauiGtkApplication
	{

		public abstract string ApplicationId { get; }

		/// <summary>
		/// overrides creation of rootcontainer
		/// rootcontainer is MainWindow 's <see cref="Gtk.Window.Child"/>
		/// paramter is Maui's Mainwindows <see cref="IWindow.Page"/> as Gtk.Widget
		/// </summary>
		public Func<Widget, Widget> TopContainerOverride { get; set; } = null!;

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

		public void Run()
		{
			Launch(new EventArgs());
		}

		protected void RegisterLifecycleEvents(Gtk.Application app)
		{

			app.Startup += OnStartup!;
			app.Shutdown += OnShutdown!;
			app.Opened += OnOpened;
			app.WindowAdded += OnWindowAdded;
			app.Activated += OnActivated!;
			app.WindowRemoved += OnWindowRemoved;
			app.CommandLine += OnCommandLine;

		}

		protected void OnStartup(object sender, EventArgs args)
		{

			CreateMainWindow();

			Services.InvokeLifecycleEvents<GtkLifecycle.OnStartup>(del => del(CurrentGtkApplication, args));
		}

		protected void OnOpened(object o, GLib.OpenedArgs args)
		{
			Services?.InvokeLifecycleEvents<GtkLifecycle.OnOpened>(del => del(CurrentGtkApplication, args));
		}

		protected void OnActivated(object sender, EventArgs args)
		{
			StartupLauch(sender, args);

			Services?.InvokeLifecycleEvents<GtkLifecycle.OnApplicationActivated>(del => del(CurrentGtkApplication, args));
		}

		protected void OnShutdown(object sender, EventArgs args)
		{
			Services?.InvokeLifecycleEvents<GtkLifecycle.OnShutdown>(del => del(CurrentGtkApplication, args));

			DispatchPendingEvents();

		}

		protected void OnCommandLine(object o, GLib.CommandLineArgs args)
		{
			// future use: to resolove command line arguments at cross platform Application level
		}

		protected void OnWindowRemoved(object o, WindowRemovedArgs args)
		{
			// future use: to have notifications at cross platform Window level
		}

		protected void OnWindowAdded(object o, WindowAddedArgs args)
		{
			// future use: to have notifications at cross platform Window level
		}

		Widget CreateRootContainer(Widget nativePage)
		{
			var b = new Box(Orientation.Vertical, 0)
			{
				Expand = true,
			};

			b.PackStart(nativePage, true, true, 0);

			return b;
		}

		protected abstract IStartup OnCreateStartup();

		protected void StartupLauch(object sender, EventArgs args)
		{
			var startup = OnCreateStartup();

			var host = startup
			   .CreateAppHostBuilder()
			   .ConfigureServices(ConfigureNativeServices)
			   .ConfigureUsing(startup)
			   .Build();

			Services = host.Services;

			var mauiContext = new MauiContext(Services);
			Services.InvokeLifecycleEvents<GtkLifecycle.OnMauiContextCreated>(del => del(mauiContext));

			var activationState = new ActivationState(mauiContext);

			Services.InvokeLifecycleEvents<GtkLifecycle.OnLaunching>(del => del(this, new ActivationEventArgs(activationState)));

			Application = Services.GetRequiredService<IApplication>();

			var window = Application.CreateWindow(activationState);

			var content = window.Content;
			var nativeContent = content.ToNative(mauiContext);

			var canvas = TopContainerOverride?.Invoke(nativeContent) ?? CreateRootContainer(nativeContent);
#if DEBUG
			nativeContent.SetBackgroundColor(Colors.White);
#endif
			MainWindow.Child = canvas;
			MainWindow.QueueDraw();
			MainWindow.ShowAll();

			MainWindow.Present();

			Services?.InvokeLifecycleEvents<GtkLifecycle.OnLaunched>(del => del(CurrentGtkApplication, new ActivationEventArgs(activationState)));
		}

		void CreateMainWindow()
		{
			MainWindow = new MauiGtkMainWindow();
			CurrentGtkApplication.AddWindow(MainWindow);

		}

		protected void Launch(EventArgs args)
		{

			Gtk.Application.Init();
			var app = new Gtk.Application(ApplicationId, GLib.ApplicationFlags.None);

			RegisterLifecycleEvents(app);

			CurrentGtkApplication = app;

			Current = this;

			((GLib.Application)app).Run();

		}

		protected void ConfigureNativeServices(HostBuilderContext ctx, IServiceCollection services)
		{
			//future use: there will be a need of GtkNativeServices, eg. for WebView
		}

		public static void DispatchPendingEvents()
		{
			// The loop is limited to 1000 iterations as a workaround for an issue that some users
			// have experienced. Sometimes EventsPending starts return 'true' for all iterations,
			// causing the loop to never end.

			int n = 1000;
#pragma warning disable 612
			Gdk.Threads.Enter();
#pragma warning restore 612

			while (Gtk.Application.EventsPending() && --n > 0)
			{
				Gtk.Application.RunIteration(false);
			}

#pragma warning disable 612
			Gdk.Threads.Leave();
#pragma warning restore 612
		}

		public static void Invoke(System.Action action)
		{
			if (action == null)
				throw new ArgumentNullException("action");

			// Switch to no Invoke(Action) once a gtk# release is done.
			Gtk.Application.Invoke((o, args) =>
			{
				action();
			});
		}

	}

}