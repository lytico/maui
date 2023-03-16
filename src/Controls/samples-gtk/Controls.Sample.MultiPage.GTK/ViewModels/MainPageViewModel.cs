using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.AppModel;
using Prism.Mvvm;
using Prism.Navigation;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using Prism.Services;
using Prism.Commands;
using Microsoft.Maui.Controls;
using Prism.Navigation.Xaml;
using System.Windows.Input;
using GLib;
using Microsoft.Maui;

namespace Maui.Controls.Sample.MultiPage.GTK.ViewModels
{
	public class MainPageViewModel : BindableBase, IInitialize, INavigatedAware, IPageLifecycleAware
	{
		protected INavigationService _navigationService { get; }

		private int count = 0;
		private int count2 = 0;
		private int count3 = 0;

		public MainPageViewModel(BaseServices baseServices)
		{
			_navigationService = baseServices.NavigationService;

			// Button 1
			this.CounterClickedCommand = new DelegateCommand<object>(this.OnCounterClicked, this.OnCounterCanSubmit);
			this.ButtonText = "Click Me";

			// Button 2
			this.Counter2ClickedCommand = new DelegateCommand<object>(this.OnCounter2Clicked, this.OnCounter2CanSubmit);
			this.Button2Text = "Clicked 1 time";

			// Button 2 Image
			this.CounterBtn2ImageVisible = true;

			// Button 3
			this.Counter3ClickedCommand = new DelegateCommand<object>(this.OnCounter3Clicked, this.OnCounter3CanSubmit);

			// Button 4
			this.Button4ClickedCommand = new DelegateCommand<object>(this.OnButton4Clicked, this.OnButton4CanSubmit);

			// Checkbox
			this.CheckBoxCheckedChanged = new DelegateCommand<object>(this.OnCheckBoxChanged, this.OnCheckBoxCanSubmit);

			// Radio Buttons
			this.RadioButtonOne_CheckedChanged = new DelegateCommand<object>((arg) => this.RadioBoxWriteLine(this.RadioButtonOneChecked, "Start", "One"), this.OnRadioBoxCanSubmit);
			this.RadioButtonTwo_CheckedChanged = new DelegateCommand<object>((arg) => this.RadioBoxWriteLine(this.RadioButtonTwoChecked, "Start", "Two"), this.OnRadioBoxCanSubmit);
			this.RadioButtonThree_CheckedChanged = new DelegateCommand<object>((arg) => this.RadioBoxWriteLine(this.RadioButtonThreeChecked, "Three", "Three"), this.OnRadioBoxCanSubmit);
			this.RadioButtonFour_CheckedChanged = new DelegateCommand<object>((arg) => this.RadioBoxWriteLine(this.RadioButtonFourChecked, "Three", "Four"), this.OnRadioBoxCanSubmit);
			this.RadioButtonFive_CheckedChanged = new DelegateCommand<object>((arg) => this.RadioBoxWriteLine(this.RadioButtonFiveChecked, "NULL", "Five"), this.OnRadioBoxCanSubmit);
			this.RadioButtonSix_CheckedChanged = new DelegateCommand<object>((arg) => this.RadioBoxWriteLine(this.RadioButtonSixChecked, "NULL", "Six"), this.OnRadioBoxCanSubmit);

			// Radio Button 1
			this.RadioButtonOneChecked = true;

			// Radio Button 3
			this.RadioButtonThreeChecked = true;

			// Radio Button 5
			this.RadioButtonFiveChecked = true;

			this.SecondLabelVisible = false;

			EditorText = "Hello from editor";
		}

		private string _buttonText;
		public string ButtonText
		{
			get => _buttonText;
			set => SetProperty(ref _buttonText, value);
		}

		private bool _secondLabelVisible;
		public bool SecondLabelVisible
		{
			get => _secondLabelVisible;
			set => SetProperty(ref _secondLabelVisible, value);
		}

		private bool _counterBtn2Visible;
		public bool CounterBtn2Visible
		{
			get => _counterBtn2Visible;
			set => SetProperty(ref _counterBtn2Visible, value);
		}

		private string _button2Text;
		public string Button2Text
		{
			get => _button2Text;
			set => SetProperty(ref _button2Text, value);
		}

		private bool _counterBtn2ImageVisible;
		public bool CounterBtn2ImageVisible
		{
			get => _counterBtn2ImageVisible;
			set => SetProperty(ref _counterBtn2ImageVisible, value);
		}

		private bool _isCheckBoxChecked;
		public bool IsCheckBoxChecked
		{
			get => _isCheckBoxChecked;
			set => SetProperty(ref _isCheckBoxChecked, value);
		}

		private bool _radioButtonOneChecked;
		public bool RadioButtonOneChecked
		{
			get => _radioButtonOneChecked;
			set => SetProperty(ref _radioButtonOneChecked, value);
		}

		private bool _radioButtonTwoChecked;
		public bool RadioButtonTwoChecked
		{
			get => _radioButtonTwoChecked;
			set => SetProperty(ref _radioButtonTwoChecked, value);
		}

		private bool _radioButtonThreeChecked;
		public bool RadioButtonThreeChecked
		{
			get => _radioButtonThreeChecked;
			set => SetProperty(ref _radioButtonThreeChecked, value);
		}

		private bool _radioButtonFourChecked;
		public bool RadioButtonFourChecked
		{
			get => _radioButtonFourChecked;
			set => SetProperty(ref _radioButtonFourChecked, value);
		}

		private bool _radioButtonFiveChecked;
		public bool RadioButtonFiveChecked
		{
			get => _radioButtonFiveChecked;
			set => SetProperty(ref _radioButtonFiveChecked, value);
		}

		private bool _radioButtonSixChecked;
		public bool RadioButtonSixChecked
		{
			get => _radioButtonSixChecked;
			set => SetProperty(ref _radioButtonSixChecked, value);
		}

		private string _editorText;
		public string EditorText
		{
			get => _editorText;
			set => SetProperty(ref _editorText, value);
		}

		public ICommand CounterClickedCommand { get; private set; }

		public ICommand Counter2ClickedCommand { get; private set; }

		public ICommand Counter3ClickedCommand { get; private set; }

		public ICommand Button4ClickedCommand { get; private set; }

		public ICommand CheckBoxCheckedChanged { get; }

		public ICommand RadioButtonOne_CheckedChanged { get; }
		public ICommand RadioButtonTwo_CheckedChanged { get; }
		public ICommand RadioButtonThree_CheckedChanged { get; }
		public ICommand RadioButtonFour_CheckedChanged { get; }
		public ICommand RadioButtonFive_CheckedChanged { get; }
		public ICommand RadioButtonSix_CheckedChanged { get; }

		public void Initialize(INavigationParameters parameters)
		{
		}

		public void OnNavigatedFrom(INavigationParameters parameters)
		{
		}

		public void OnNavigatedTo(INavigationParameters parameters)
		{
			this.SecondLabelVisible = true;
		}

		public void OnAppearing()
		{
			var timer = Microsoft.Maui.Controls.Application.Current.Dispatcher.CreateTimer();
			timer.Interval = TimeSpan.FromMilliseconds(200);
			timer.Tick += (s, e) => ClearVisibilityOnButton2();
			timer.IsRepeating = false;
			timer.Start();
		}

		public void OnDisappearing()
		{
		}

		void ClearVisibilityOnButton2()
		{
			Microsoft.Maui.ApplicationModel.MainThread.BeginInvokeOnMainThread(() =>
			{
				//Update view here
				// CounterBtn2.IsVisible = false;
				CounterBtn2Visible = false;
				Console.WriteLine("Button 2 made invisible");
			});
		}

		private void OnCounterClicked(object arg)
		{
			count++;

			if (count == 1)
				ButtonText = $"Clicked {count} time";
			else
				ButtonText = $"Clicked {count} times";

			if (this.RadioButtonOneChecked)
			{
				Console.WriteLine("Button 1 " + ButtonText + " radio button \"One\" is selected");
			}
			else
			{
				Console.WriteLine("Button 1 " + ButtonText + " radio button \"Two\" is selected");
			}
		}

		private bool OnCounterCanSubmit(object arg)
		{
			return true;
		}

		private void OnCounter2Clicked(object arg)
		{
			count2++;

			if (count2 == 1)
			{
				// Console.WriteLine("Count2 = 1");

				CounterBtn2Visible = true;

				var timer = Microsoft.Maui.Controls.Application.Current.Dispatcher.CreateTimer();
				timer.Interval = TimeSpan.FromMilliseconds(300);
				timer.Tick += (s, e) => HideButtonWithImage();
				timer.IsRepeating = false;
				timer.Start();
			}
			else
			{
				// Console.WriteLine("Count2 > 1");

				CounterBtn2Visible = true;
				CounterBtn2ImageVisible = false;
				Button2Text = $"Clicked {count2} times";
			}

			Console.WriteLine("Button 2 " + Button2Text + " Clicked");
		}

		private bool OnCounter2CanSubmit(object arg)
		{
			return true;
		}

		private void OnCounter3Clicked(object arg)
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

		private bool OnCounter3CanSubmit(object arg)
		{
			return true;
		}

		private async void OnButton4Clicked(object arg)
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
			// await Navigation.PushAsync(new Page2());
			await _navigationService.NavigateAsync("Page2");
		}

		private bool OnButton4CanSubmit(object arg)
		{
			return true;
		}

		private void OnCheckBoxChanged(object arg)
		{
			if (IsCheckBoxChecked)
			{
				Console.WriteLine("CheckBox_CheckedChanged CHECKED");
			}
			else
			{
				Console.WriteLine("CheckBox_CheckedChanged NOT CHECKED");
			}

			Console.WriteLine("Here is the contents of the Editor: " + EditorText);
		}

		private bool OnCheckBoxCanSubmit(object arg)
		{
			return true;
		}

		private void HideButtonWithImage()
		{
			Microsoft.Maui.ApplicationModel.MainThread.BeginInvokeOnMainThread(() =>
			{
				CounterBtn2ImageVisible = false;
				//CounterBtn2.Text = $"Clicked {count2} time";
				Console.WriteLine("Hiding CounterBtn2Image");
			});
		}

		private void RadioBoxWriteLine(bool isChecked, string groupName, string valueName)
		{
			if (isChecked)
			{
				Console.WriteLine("OnRadioBox" + valueName + "CheckedChanged SELECTED in group: \"" + groupName + "\" with name: \"" + valueName + "\"");
			}
		}

		private bool OnRadioBoxCanSubmit(object arg)
		{
			return true;
		}
	}
}
