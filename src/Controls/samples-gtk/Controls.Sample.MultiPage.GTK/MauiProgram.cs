using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.Controls.PlatformConfiguration;
using Microsoft.Maui.Hosting;

namespace Maui.Controls.Sample.MultiPage.GTK
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
		protected override Window CreateWindow(IActivationState activationState)
		{
			var navPage = new NavigationPage(new MainPage());
			navPage.WidthRequest = 600;
			navPage.HeightRequest = 600;
			return new Window(navPage);
		}
	}
}