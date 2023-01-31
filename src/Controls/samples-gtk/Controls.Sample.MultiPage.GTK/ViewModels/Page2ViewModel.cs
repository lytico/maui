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
using System.Windows.Forms;

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
			this.RadioButtonOne_CheckedChanged = new DelegateCommand<object>((arg) => this.RadioBoxWriteLine(this.RadioButtonOneChecked, "Start", "One"), this.OnRadioBoxCanSubmit);
			this.RadioButtonTwo_CheckedChanged = new DelegateCommand<object>((arg) => this.RadioBoxWriteLine(this.RadioButtonTwoChecked, "Start", "Two"), this.OnRadioBoxCanSubmit);
			this.RadioButtonThree_CheckedChanged = new DelegateCommand<object>((arg) => this.RadioBoxWriteLine(this.RadioButtonThreeChecked, "Three", "Three"), this.OnRadioBoxCanSubmit);
			this.RadioButtonFour_CheckedChanged = new DelegateCommand<object>((arg) => this.RadioBoxWriteLine(this.RadioButtonFourChecked, "Three", "Four"), this.OnRadioBoxCanSubmit);
			this.RadioButtonFive_CheckedChanged = new DelegateCommand<object>((arg) => this.RadioBoxWriteLine(this.RadioButtonFiveChecked, "NULL", "Five"), this.OnRadioBoxCanSubmit);
			this.RadioButtonSix_CheckedChanged = new DelegateCommand<object>((arg) => this.RadioBoxWriteLine(this.RadioButtonSixChecked, "NULL", "Six"), this.OnRadioBoxCanSubmit);
			this.RadioButtonOneChecked = true;
			this.RadioButtonThreeChecked = true;
			this.RadioButtonFiveChecked = true;
			this.Entry1Text = "testPW";
			this.Entry2Text = "Hello";
			this.Editor1Text = "Hello from editor";
			this.Grid2Label2Text = "Test Label 2";
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

		private string _entry1Text;
		public string Entry1Text
		{
			get => _entry1Text;
			set => SetProperty(ref _entry1Text, value);
		}

		private string _entry2Text;
		public string Entry2Text
		{
			get => _entry2Text;
			set => SetProperty(ref _entry2Text, value);
		}

		private string _editor1Text;
		public string Editor1Text
		{
			get => _editor1Text;
			set => SetProperty(ref _editor1Text, value);
		}

		private string _grid2Label2Text;
		public string Grid2Label2Text
		{
			get => _grid2Label2Text;
			set => SetProperty(ref _grid2Label2Text, value);
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
		}

		public void OnDisappearing()
		{
		}

		private async void OnBackButtonClicked(object arg)
		{
			Console.WriteLine("OnBackButtonClicked Here is Entry 1 text: " + this.Entry1Text);
			Console.WriteLine("OnBackButtonClicked Here is Entry 2 text: " + this.Entry2Text);
			Console.WriteLine("OnBackButtonClicked Here is Editor 1 text: " + this.Editor1Text);
			Console.WriteLine("OnBackButtonClicked Here is Test Label 2 text: " + this.Grid2Label2Text);

			await _navigationService.NavigateAsync("MainPage");
		}

		private bool OnBackButtonCanSubmit(object arg)
		{
			return true;
		}

		private void OnCheckBoxChanged(object arg)
		{
			Console.WriteLine("OnCheckBoxChanged Here is Entry 1 text: " + this.Entry1Text);
			Console.WriteLine("OnCheckBoxChanged Here is Entry 2 text: " + this.Entry2Text);
			Console.WriteLine("OnCheckBoxChanged Here is Editor 1 text: " + this.Editor1Text);
			Console.WriteLine("OnCheckBoxChanged Here is Test Label 2 text: " + this.Grid2Label2Text);

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
