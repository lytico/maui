using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace Maui.Controls.Sample.MultiPage.GTK
{
	public partial class MainPage : ContentPage
	{
		int count = 0;
		int count2 = 0;
		int count3 = 0;

		public MainPage()
		{
			InitializeComponent();

			// EntryBox1.Text = "hello";
			// Editor1.Text = "Hello from editor";
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();

			var timer = Application.Current.Dispatcher.CreateTimer();
			timer.Interval = TimeSpan.FromMilliseconds(200);
			timer.Tick += (s, e) => ClearVisibilityOnButton2();
			timer.IsRepeating = false;
			timer.Start();
			//CounterBtn2.IsVisible = false;
			//Console.WriteLine("Button 2 made invisible");
		}

		void ClearVisibilityOnButton2()
		{
			Microsoft.Maui.ApplicationModel.MainThread.BeginInvokeOnMainThread(() =>
			{
				//Update view here
				CounterBtn2.IsVisible = false;
				Console.WriteLine("Button 2 made invisible");
			});
		}

		private void OnCounterClicked(object sender, EventArgs e)
		{
			count++;

			if (count == 1)
				CounterBtn.Text = $"Clicked {count} time";
			else
				CounterBtn.Text = $"Clicked {count} times";

			if (RadioBtnOne.IsChecked)
			{
				Console.WriteLine("Button 1 " + CounterBtn.Text + " radio button \"One\" is selected");
			}
			else
			{
				Console.WriteLine("Button 1 " + CounterBtn.Text + " radio button \"Two\" is selected");
			}
		}

		private void OnCounter2Clicked(object sender, EventArgs e)
		{
			count2++;

			if (count2 == 1)
			{
				// Console.WriteLine("Count2 = 1");

				CounterBtn2.IsVisible = true;

				var timer = Application.Current.Dispatcher.CreateTimer();
				timer.Interval = TimeSpan.FromMilliseconds(300);
				timer.Tick += (s, e) => HideButtonWithImage();
				timer.IsRepeating = false;
				timer.Start();
			}
			else
			{
				// Console.WriteLine("Count2 > 1");

				CounterBtn2.IsVisible = true;
				CounterBtn2Image.IsVisible = false;
				CounterBtn2.Text = $"Clicked {count2} times";
			}

			Console.WriteLine("Button 2 " + CounterBtn2.Text + " Clicked");
		}

		private void OnCounter3Clicked(object sender, EventArgs e)
		{
			count3++;

			//if (count3 == 1)
			//{
			//	// Console.WriteLine("Count2 = 1");

			//	CounterBtn3Image.Text = $"Clicked {count3} time";
			//}
			//else
			//{
			//	// Console.WriteLine("Count2 > 1");

			//	CounterBtn3Image.Text = $"Clicked {count2} times";
			//}

			Console.WriteLine("Image Button " + count3.ToString("D") + " Clicked");
		}

		private async void OnButton4Clicked(object sender, EventArgs e)
		{
			count3++;

			//if (count3 == 1)
			//{
			//	// Console.WriteLine("Count2 = 1");

			//	CounterBtn3Image.Text = $"Clicked {count3} time";
			//}
			//else
			//{
			//	// Console.WriteLine("Count2 > 1");

			//	CounterBtn3Image.Text = $"Clicked {count2} times";
			//}

			Console.WriteLine("OnButton4Clicked");
			await Navigation.PushAsync(new Page2());
		}

		private void HideButtonWithImage()
		{
			Microsoft.Maui.ApplicationModel.MainThread.BeginInvokeOnMainThread(() =>
			{
				CounterBtn2Image.IsVisible = false;
				//CounterBtn2.Text = $"Clicked {count2} time";
				Console.WriteLine("Hiding CounterBtn2Image");
			});
		}

		private void CheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
		{
			bool isChecked = false;
			if (sender is CheckBox checkBox)
			{
				if (checkBox.IsChecked == true)
				{
					isChecked = true;
				}
			}

			if (isChecked)
			{
				Console.WriteLine("CheckBox_CheckedChanged CHECKED");
			}
			else
			{
				Console.WriteLine("CheckBox_CheckedChanged NOT CHECKED");
			}
		}

		private void RadioButton_CheckedChanged(object sender, CheckedChangedEventArgs e)
		{
			bool isChecked = false;
			string groupName = string.Empty;
			string valueName = string.Empty;

			if (sender is RadioButton radioButton)
			{
				groupName = radioButton.GroupName;
				if (radioButton.Value != null)
				{
					valueName = radioButton.Value as string;
				}
				else
				{
					valueName = "NULL";
				}
				if (radioButton.IsChecked == true)
				{
					isChecked = true;
				}
			}

			if (isChecked)
			{
				Console.WriteLine("RadioButton_CheckedChanged SELECTED in group: \"" + groupName + "\" with name: \"" + valueName + "\"");
			}
			//else
			//{
			//	Console.WriteLine("RadioButton_CheckedChanged NOT CHECKED in group: " + groupName + " with name: " + valueName);
			//}
		}
	}
}