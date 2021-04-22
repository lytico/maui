using System;
using GLib;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Maui.Hosting;
using Microsoft.Maui.LifecycleEvents;
using Gtk;

namespace Microsoft.Maui
{

	public class MauiGtkApplication<TStartup> : MauiGtkApplication0
		where TStartup : IStartup, new()
	{

		protected override void OnStartup()
		{
			base.OnStartup();
			Startup += OnStartup;
			Shutdown += OnShutdown;
			Opened += OnOpened;
			WindowAdded += OnWindowAdded;
			Activated += OnActivated;
			WindowRemoved += OnWindowRemoved;
			CommandLine += OnCommandLine;
		}

		void OnStartup(object? sender, EventArgs args)
		{
			Current.Services?.InvokeLifecycleEvents<LinuxLifecycle.OnStartup>(del => del(this, args));
			OnLaunched(args);
		}

		void OnOpened(object o, OpenedArgs args)
		{
			Current.Services?.InvokeLifecycleEvents<LinuxLifecycle.OnOpened>(del => del(this, args));
		}

		void OnActivated(object? sender, EventArgs args)
		{
			Current.Services?.InvokeLifecycleEvents<LinuxLifecycle.OnApplicationActivated>(del => del(this, args));
		}

		void OnShutdown(object? sender, EventArgs args)
		{
			Current.Services?.InvokeLifecycleEvents<LinuxLifecycle.OnShutdown>(del => del(this, args));
		}

		void OnCommandLine(object o, CommandLineArgs args)
		{ }

		void OnWindowRemoved(object o, WindowRemovedArgs args)
		{ }

		void OnWindowAdded(object o, WindowAddedArgs args)
		{ }

		protected void OnLaunched(EventArgs args)
		{

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

			MainWindow.Activate();

			Current.Services?.InvokeLifecycleEvents<LinuxLifecycle.OnLaunched>(del => del(this, args));
		}

		Container CreateRootContainer()
		{
			var b = new Box(Orientation.Horizontal, 0);

			return b;
		}

		void ConfigureNativeServices(HostBuilderContext ctx, IServiceCollection services)
		{ }

	}

	public abstract class MauiGtkApplication0 : Gtk.Application
	{

		protected MauiGtkApplication0() : base(string.Empty, ApplicationFlags.None)
		{ }

		public static MauiGtkApplication0 Current => (MauiGtkApplication0)Gtk.Application.Default;

		public MauiGtkMainWindow MainWindow { get; protected set; } = null!;

		public IServiceProvider Services { get; protected set; } = null!;

		public IApplication Application { get; protected set; } = null!;

	}

}