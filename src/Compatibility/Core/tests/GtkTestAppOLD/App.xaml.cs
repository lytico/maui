using Microsoft.Maui;

namespace GtkTestApp {
	public partial class App : Microsoft.Maui.Controls.Application
	{
		public App() {
			InitializeComponent();

			MainPage = new AppShell();
		}
	}
}