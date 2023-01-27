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

namespace Maui.Controls.Sample.MultiPage.GTK.ViewModels
{
	public class MainPageViewModel : BindableBase, IInitialize, INavigatedAware, IPageLifecycleAware
	{
		protected INavigationService _navigationService { get; }
		protected IPageDialogService _pageDialogs { get; }
		protected IDialogService _dialogs { get; }

		private int count = 0;
		private int count2 = 0;
		private int count3 = 0;

		public MainPageViewModel(BaseServices baseServices)
		{
			_navigationService = baseServices.NavigationService;
			_pageDialogs = baseServices.PageDialogs;
			_dialogs = baseServices.Dialogs;

			Title = Regex.Replace(GetType().Name, "ViewModel", string.Empty);
			Id = Guid.NewGuid().ToString();
			NavigateCommand = new DelegateCommand<string>(OnNavigateCommandExecuted);
			ShowPageDialog = new DelegateCommand(OnShowPageDialog);
			Messages = new ObservableCollection<string>();
			Messages.CollectionChanged += (sender, args) =>
			{
				foreach (string message in args.NewItems)
					Console.WriteLine($"{Title} - {message}");
			};

			AvailableDialogs = baseServices.DialogRegistry.Registrations.Select(x => x.Name).ToList();
			SelectedDialog = AvailableDialogs.FirstOrDefault();
			ShowDialog = new DelegateCommand(OnShowDialogCommand, () => !string.IsNullOrEmpty(SelectedDialog))
				.ObservesProperty(() => SelectedDialog);
		}

		public IEnumerable<string> AvailableDialogs { get; }

		public string Title { get; }

		public string Id { get; }

		private string _selectedDialog;
		public string SelectedDialog
		{
			get => _selectedDialog;
			set => SetProperty(ref _selectedDialog, value);
		}

		public ObservableCollection<string> Messages { get; }

		public DelegateCommand<string> NavigateCommand { get; }

		public DelegateCommand ShowPageDialog { get; }

		public DelegateCommand ShowDialog { get; }

		private void OnNavigateCommandExecuted(string uri)
		{
			Messages.Add($"OnNavigateCommandExecuted: {uri}");
			_navigationService.NavigateAsync(uri)
				.OnNavigationError(ex => Console.WriteLine(ex));
		}

		private void OnShowPageDialog()
		{
			Messages.Add("OnShowPageDialog");
			_pageDialogs.DisplayAlertAsync("Message", $"Hello from {Title}. This is a Page Dialog Service Alert!", "Ok");
		}

		private void OnShowDialogCommand()
		{
			Messages.Add("OnShowDialog");
			_dialogs.ShowDialog(SelectedDialog, null, DialogCallback);
		}

		private void DialogCallback(IDialogResult result) =>
			Messages.Add("Dialog Closed");

		public void Initialize(INavigationParameters parameters)
		{
			Messages.Add("ViewModel Initialized");
			foreach (var parameter in parameters.Where(x => x.Key.Contains("message", StringComparison.Ordinal)))
				Messages.Add(parameter.Value.ToString());


			var timer = Microsoft.Maui.Controls.Application.Current.Dispatcher.CreateTimer();
			timer.Interval = TimeSpan.FromMilliseconds(200);
			timer.Tick += (s, e) => ClearVisibilityOnButton2();
			timer.IsRepeating = false;
			timer.Start();
			//CounterBtn2.IsVisible = false;
			//Console.WriteLine("Button 2 made invisible");
		}

		public void OnNavigatedFrom(INavigationParameters parameters)
		{
			Messages.Add("ViewModel NavigatedFrom");
		}

		public void OnNavigatedTo(INavigationParameters parameters)
		{
			Messages.Add("ViewModel NavigatedTo");
		}

		public void OnAppearing()
		{
			Messages.Add("View Appearing");
		}

		public void OnDisappearing()
		{
			Messages.Add("View Disappearing");
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
