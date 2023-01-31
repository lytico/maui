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

namespace Maui.Controls.Sample.MultiPage.GTK.ViewModels
{
	public class Page2ViewModel : BindableBase, IInitialize, INavigatedAware, IPageLifecycleAware
	{
		protected INavigationService _navigationService { get; }

		public Page2ViewModel(BaseServices baseServices)
		{
			_navigationService = baseServices.NavigationService;

			// Back Button
			this.BackButtonClickedCommand = new DelegateCommand<object>(this.OnBackButtonClicked, this.OnBackButtonCanSubmit);

			// Checkbox
			this.CheckBoxCheckedChanged = new DelegateCommand<object>(this.OnCheckBoxChanged, this.OnCheckBoxCanSubmit);

			// Radio Buttons
			this.RadioButtonOne_CheckedChanged = new DelegateCommand<object>(this.OnRadioBoxOneCheckedChanged, this.OnRadioBoxCanSubmit);
			this.RadioButtonTwo_CheckedChanged = new DelegateCommand<object>(this.OnRadioBoxTwoCheckedChanged, this.OnRadioBoxCanSubmit);
			this.RadioButtonThree_CheckedChanged = new DelegateCommand<object>(this.OnRadioBoxThreeCheckedChanged, this.OnRadioBoxCanSubmit);
			this.RadioButtonFour_CheckedChanged = new DelegateCommand<object>(this.OnRadioBoxFourCheckedChanged, this.OnRadioBoxCanSubmit);
			this.RadioButtonFive_CheckedChanged = new DelegateCommand<object>(this.OnRadioBoxFiveCheckedChanged, this.OnRadioBoxCanSubmit);
			this.RadioButtonSix_CheckedChanged = new DelegateCommand<object>(this.OnRadioBoxSixCheckedChanged, this.OnRadioBoxCanSubmit);
			this.RadioButtonOneChecked = true;
			this.RadioButtonThreeChecked = true;
			this.RadioButtonFiveChecked = true;
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

		public ICommand BackButtonClickedCommand { get; private set; }
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
		}

		public void OnAppearing()
		{
			//var timer = Microsoft.Maui.Controls.Application.Current.Dispatcher.CreateTimer();
			//timer.Interval = TimeSpan.FromMilliseconds(200);
			//timer.Tick += (s, e) => ClearVisibilityOnButton2();
			//timer.IsRepeating = false;
			//timer.Start();
		}

		public void OnDisappearing()
		{
		}

		private async void OnBackButtonClicked(object arg)
		{
			Console.WriteLine("OnBackButtonClicked");
			//// await Navigation.PushAsync(new Page2());
			//await Navigation.PopAsync();
			await _navigationService.NavigateAsync("MainPage");
		}

		private bool OnBackButtonCanSubmit(object arg)
		{
			return true;
		}

		private void OnCheckBoxChanged(object arg)
		{
			//bool isChecked = false;
			//if (sender is CheckBox checkBox)
			//{
			//	if (checkBox.IsChecked == true)
			//	{
			//		isChecked = true;
			//	}
			//}

			if (IsCheckBoxChecked)
			{
				Console.WriteLine("CheckBox_CheckedChanged CHECKED");
			}
			else
			{
				Console.WriteLine("CheckBox_CheckedChanged NOT CHECKED");
			}
		}

		private bool OnCheckBoxCanSubmit(object arg)
		{
			return true;
		}

		private void RadioBoxWriteLine(string groupName, string valueName)
		{
			Console.WriteLine("OnRadioBox" + valueName + "CheckedChanged SELECTED in group: \"" + groupName + "\" with name: \"" + valueName + "\"");
		}

		private void OnRadioBoxOneCheckedChanged(object arg)
		{
			//bool isChecked = false;
			string groupName = "Start";
			string valueName = "One";

			//if (sender is RadioButton radioButton)
			//{
			//	groupName = radioButton.GroupName;
			//	if (radioButton.Value != null)
			//	{
			//		valueName = radioButton.Value as string;
			//	}
			//	else
			//	{
			//		valueName = "NULL";
			//	}
			//	if (radioButton.IsChecked == true)
			//	{
			//		isChecked = true;
			//	}
			//}
			if (RadioButtonOneChecked)
			{
				RadioBoxWriteLine(groupName, valueName);
			}
			//else if (RadioButtonTwoChecked)
			//{
			//	valueName = "Two";
			//}

			////if (isChecked)
			////{
			//Console.WriteLine("OnRadioBoxOneCheckedChanged SELECTED in group: \"" + groupName + "\" with name: \"" + valueName + "\"");
			////}
			////else
			////{
			////	Console.WriteLine("RadioButton_CheckedChanged NOT CHECKED in group: " + groupName + " with name: " + valueName);
			////}
		}

		private void OnRadioBoxTwoCheckedChanged(object arg)
		{
			string groupName = "Start";
			string valueName = "Two";

			if (RadioButtonTwoChecked)
			{
				RadioBoxWriteLine(groupName, valueName);
			}
		}

		private void OnRadioBoxThreeCheckedChanged(object arg)
		{
			string groupName = "Three";
			string valueName = "Three";

			if (RadioButtonThreeChecked)
			{
				RadioBoxWriteLine(groupName, valueName);
			}
		}

		private void OnRadioBoxFourCheckedChanged(object arg)
		{
			string groupName = "Three";
			string valueName = "Four";

			if (RadioButtonFourChecked)
			{
				RadioBoxWriteLine(groupName, valueName);
			}
		}

		private void OnRadioBoxFiveCheckedChanged(object arg)
		{
			string groupName = "NULL";
			string valueName = "Five";

			if (RadioButtonFiveChecked)
			{
				RadioBoxWriteLine(groupName, valueName);
			}
		}

		private void OnRadioBoxSixCheckedChanged(object arg)
		{
			string groupName = "NULL";
			string valueName = "Six";

			if (RadioButtonSixChecked)
			{
				RadioBoxWriteLine(groupName, valueName);
			}
		}

		private bool OnRadioBoxCanSubmit(object arg)
		{
			return true;
		}
	}
}
