using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.Controls.PlatformConfiguration;
using Microsoft.Maui.Hosting;

namespace Maui.Controls.Sample
{
	public static class MauiProgram
	{
		static void Main()
		{
			// Gtk.Application.Init();
			// MauiProgram.CreateMauiApp();

			// var app = new App();

			// Gtk.Application.Run();

			System.Diagnostics.Debug.WriteLine("Inside MauiProgram.Main.");
		}

		public static MauiApp CreateMauiApp() =>
			MauiApp
				.CreateBuilder()
				.UseMauiApp<App>()
				.Build();
	}

	class App : Application
	{
		//protected override Window CreateWindow(IActivationState activationState) =>
		//	new Window(new MainPage());
	}
}