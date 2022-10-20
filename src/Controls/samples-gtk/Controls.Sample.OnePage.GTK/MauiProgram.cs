using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.Controls.PlatformConfiguration;
using Microsoft.Maui.Hosting;

namespace Maui.Controls.Sample.OnePage.GTK
{
	public static class MauiProgram
	{
		public static MauiApp CreateMauiApp() =>
			MauiApp
				.CreateBuilder()
				.UseMauiApp<App>()
				.Build();
	}

	class App : Microsoft.Maui.Controls.Application
	{
		protected override Window CreateWindow(IActivationState activationState) =>
			new Window(new MainPage());
	}
}