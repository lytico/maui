using Microsoft.Maui;
using Microsoft.Maui.Controls;

namespace GtkTestApp
{
	public class MainPage : ContentPage
	{
		int count = 0;

		Microsoft.Maui.Controls.Button _button;

		public MainPage()
		{
			_button = new Microsoft.Maui.Controls.Button { Text = "Click me", HorizontalOptions = LayoutOptions.Center };
			_button.Clicked += OnCounterClicked;

			Content = new ScrollView
			{
				Content = new VerticalStackLayout
				{
					Margin = new Thickness(20),
					Children =
					{
						new Microsoft.Maui.Controls.Image { Source = "dotnet_bot.png", HeightRequest = 200, HorizontalOptions = LayoutOptions.Center },
						new Microsoft.Maui.Controls.Label { Text = "Hello, World!", FontSize = 32, HorizontalOptions = LayoutOptions.Center },
						new Microsoft.Maui.Controls.Label { Text = "Welcome To .NET GTK using MAUI", FontSize = 18, HorizontalOptions = LayoutOptions.Center },
						_button
					}
				}
			};
		}

		private void OnCounterClicked(object? sender, EventArgs e)
		{
			count++;

			if (count == 1)
				_button.Text = $"Clicked {count} time";
			else
				_button.Text = $"Clicked {count} times";

			// SemanticScreenReader.Announce(_button.Text);
		}
	}
}
