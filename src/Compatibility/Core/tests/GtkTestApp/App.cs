namespace GtkTestApp
{
	public class App : Microsoft.Maui.Controls.Application
	{
		public App()
		{
			Resources.Add("Source", "Resources/Styles/Colors.xaml");
			Resources.Add("Source", "Resources/Styles/Styles.xaml");

			MainPage = new MainPage();
		}
	}
}
