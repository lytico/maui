using Microsoft.Maui.Controls;

namespace UWPTestApp
{
	public partial class App : Application
	{
		public App()
		{
			InitializeComponent();

			MainPage = new AppShell();
		}
	}
}
