using GLib;
using Microsoft.Maui.Accessibility;
using Microsoft.Maui.Controls;
using System.Xml.Linq;

namespace WpfApp1;

public class MainPage : ContentPage
{
	int count = 0;
	Button button;

	public MainPage()
	{
        var scrollView = new ScrollView();
		Content = scrollView;

		var vertLayout = new VerticalStackLayout {
			Spacing = 25,
			Padding = new Microsoft.Maui.Thickness(30,0),
			VerticalOptions = LayoutOptions.Center,
		};
		scrollView.Content = vertLayout;

		var image = new Image {
			Source = "dotnet_bot.png",
			HeightRequest = 200,
			HorizontalOptions = LayoutOptions.Center
		};
		SemanticProperties.SetDescription(image, "Cute dot net bot waving hi to you!");
		vertLayout.Children.Add(image);

		var label1 = new Label {
			Text = "Hello, World!",
			FontSize = 32,
			HorizontalOptions = LayoutOptions.Center
		};
		SemanticProperties.SetHeadingLevel(label1, Microsoft.Maui.SemanticHeadingLevel.Level1);
		vertLayout.Children.Add(label1);

		var label2 = new Label {
			Text = "Welcome to .NET Multi-platform App UI",
			FontSize = 18,
			HorizontalOptions = LayoutOptions.Center
		};
		SemanticProperties.SetHeadingLevel(label2, Microsoft.Maui.SemanticHeadingLevel.Level2);
		SemanticProperties.SetDescription(label2, "Welcome to dot net Multi platform App U I");
		vertLayout.Children.Add(label2);

		button = new Button {
			Text = "Click me",
			HorizontalOptions = LayoutOptions.Center
		};
		SemanticProperties.SetHint(button, "Counts the number of times you click");
		vertLayout.Children.Add(button);
	}

	private void OnCounterClicked(object sender, System.EventArgs e)
	{
		count++;

		if (count == 1)
			button.Text = $"Clicked {count} time";
		else
			button.Text = $"Clicked {count} times";

		SemanticScreenReader.Announce(button.Text);
	}
}

