﻿using System;
using Gtk;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Maui.Hosting;
using Microsoft.Maui.LifecycleEvents;

namespace Microsoft.Maui
{
	
	[Obsolete("use MauiGtkApplication")]
	public class MauiWindow<TStartup> : Window
		where TStartup : IStartup, new()
	{
		public MauiWindow() : base(WindowType.Toplevel)
		{
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
			var content = window.Content;

			Add(content.ToNative(mauiContext));
			Child.ShowAll();
		}

		public new IApplication Application { get; protected set; } = null!;

		public IServiceProvider Services { get; protected set; } = null!;

		// Configure native services like HandlersContext, ImageSourceHandlers etc.. 
		void ConfigureNativeServices(HostBuilderContext ctx, IServiceCollection services)
		{
		}
	}
}