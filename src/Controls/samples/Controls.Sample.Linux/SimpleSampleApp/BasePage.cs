using Microsoft.Maui;
using Microsoft.Maui.Controls;

namespace Maui.SimpleSampleApp
{
	public class BasePage : ContentPage, IPage
	{
		IView IPage.View
		{
			get => Content;
			set => Content = (View)value;
		}
	}
}