using System;
using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui;

namespace Maui.SimpleSampleApp
{
	public class MyApp : IApplication
	{
		public MyApp(IServiceProvider services, ITextService textService)
		{
			Services = services;

			Debug.WriteLine($"The injected text service had a message: '{textService.GetText()}'");
		}

		public IServiceProvider Services { get; }

		public IWindow CreateWindow(IActivationState activationState)
		{
			Microsoft.Maui.Controls.Compatibility.Forms.Init(activationState);

			return Services.GetRequiredService<IWindow>();
		}
	}
}
