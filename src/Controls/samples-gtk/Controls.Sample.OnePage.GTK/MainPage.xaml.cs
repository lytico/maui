using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace Maui.Controls.Sample.OnePage.GTK
{
	public partial class MainPage : ContentPage
	{
		int count = 0;
		int count2 = 0;

		public MainPage()
		{
			InitializeComponent();
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

			Console.WriteLine("Button 1 " + CounterBtn.Text);
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

		private void HideButtonWithImage()
		{
			Microsoft.Maui.ApplicationModel.MainThread.BeginInvokeOnMainThread(() =>
			{
				CounterBtn2Image.IsVisible = false;
				//CounterBtn2.Text = $"Clicked {count2} time";
				Console.WriteLine("Hiding CounterBtn2Image");
			});
		}
	}
}