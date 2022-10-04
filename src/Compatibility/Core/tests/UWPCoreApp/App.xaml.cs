using Microsoft.Maui.Controls;

namespace UWPCoreApp;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();

		// MainPage = new AppShell();
		MainPage = new MainPage();
	}
}
