using Microsoft.Maui;
using Microsoft.Maui.Hosting;
using Maui.Control.PageSample;

namespace Maui.Control.PageSample.Platform
{
	public partial class App : MauiWinUIApplication
	{
		public App()
		{
			InitializeComponent();
		}

		protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
	}
}