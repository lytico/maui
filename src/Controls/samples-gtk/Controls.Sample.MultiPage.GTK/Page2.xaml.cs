using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace Maui.Controls.Sample.MultiPage.GTK
{
	public partial class Page2 : ContentPage
	{
		public Page2()
		{
			InitializeComponent();
		}

		//private async void Button_Clicked(object sender, EventArgs e)
		//{
		//	Console.WriteLine("Button_Clicked");
		//	// await Navigation.PushAsync(new Page2());
		//	await Navigation.PopAsync();
		//}

		//private void CheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
		//{
		//	bool isChecked = false;
		//	if (sender is CheckBox checkBox)
		//	{
		//		if (checkBox.IsChecked == true)
		//		{
		//			isChecked = true;
		//		}
		//	}

		//	if (isChecked)
		//	{
		//		Console.WriteLine("CheckBox_CheckedChanged CHECKED");
		//	}
		//	else
		//	{
		//		Console.WriteLine("CheckBox_CheckedChanged NOT CHECKED");
		//	}
		//}

		//private void RadioButton_CheckedChanged(object sender, CheckedChangedEventArgs e)
		//{
		//	bool isChecked = false;
		//	string groupName = string.Empty;
		//	string valueName = string.Empty;

		//	if (sender is RadioButton radioButton)
		//	{
		//		groupName = radioButton.GroupName;
		//		if (radioButton.Value != null)
		//		{
		//			valueName = radioButton.Value as string;
		//		}
		//		else
		//		{
		//			valueName = "NULL";
		//		}
		//		if (radioButton.IsChecked == true)
		//		{
		//			isChecked = true;
		//		}
		//	}

		//	if (isChecked)
		//	{
		//		Console.WriteLine("RadioButton_CheckedChanged SELECTED in group: \"" + groupName + "\" with name: \"" + valueName + "\"");
		//	}
		//	//else
		//	//{
		//	//	Console.WriteLine("RadioButton_CheckedChanged NOT CHECKED in group: " + groupName + " with name: " + valueName);
		//	//}
		//}
	}
}