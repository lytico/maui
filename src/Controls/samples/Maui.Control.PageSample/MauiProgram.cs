using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.Hosting;

namespace Maui.Control.PageSample
{
	public static class MauiProgram
	{
		public static MauiApp CreateMauiApp()
		{
			var builder = MauiApp.CreateBuilder();

			builder.UseMauiApp<App>();

			return builder.Build();
		}
	}

	class App : Application
	{
		protected override Window CreateWindow(IActivationState activationState) =>
			new Window(new DerbyPage());
	}
}
