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

		private void OnCounterClicked(object sender, EventArgs e)
		{
			count++;

			if (count == 1)
				CounterBtn.Text = $"Clicked {count} time";
			else
				CounterBtn.Text = $"Clicked {count} times";
		}

		private void OnCounter2Clicked(object sender, EventArgs e)
		{
			count2++;

			if (count2 == 1)
			{
				CounterBtn2.ImageSource = null;
				CounterBtn2.Text = $"Clicked {count2} time";
			}
			else
			{
				CounterBtn2.ImageSource = null;
				CounterBtn2.Text = $"Clicked {count2} times";
			}
		}
	}
}