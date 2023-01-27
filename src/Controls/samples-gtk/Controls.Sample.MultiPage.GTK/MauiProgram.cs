using Maui.Controls.Sample.MultiPage.GTK.ViewModels;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.Controls.PlatformConfiguration;
using Microsoft.Maui.Hosting;
using Prism;
using Prism.DryIoc;
using Prism.Ioc;

namespace Maui.Controls.Sample.MultiPage.GTK
{
	public static class MauiProgram
	{
		public static MauiApp CreateMauiApp() =>
			MauiApp
				.CreateBuilder()
				.UseMauiApp<App>()
				.UsePrism(prism =>
					prism.RegisterTypes(containerRegistry =>
					{
						containerRegistry.RegisterForNavigation<MainPage, MainPageViewModel>();
						containerRegistry.RegisterForNavigation<Page2, Page2ViewModel>();
					}))
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