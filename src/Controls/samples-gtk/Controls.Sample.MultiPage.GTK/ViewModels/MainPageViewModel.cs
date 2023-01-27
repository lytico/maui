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

			// Radio Button 1
			this.RadioBtnOneIsChecked = true;
			this.RadioButton1ChangedCommand = new DelegateCommand<object>(this.RadioButton_CheckedChanged, this.RadioButton_CheckedChangedCanSubmit);

			// Radio Button 3
			this.RadioBtnThreeIsChecked = true;

			// Radio Button 5
			this.RadioBtnFiveIsChecked = true;
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

		public string ButtonText { get; private set; }

		public bool CounterBtn2Visible { get; private set; }

		public string Button2Text { get; private set; }

		public bool CounterBtn2ImageVisible { get; private set; }

		public bool RadioBtnOneIsChecked { get; set; }

		public bool RadioBtnThreeIsChecked { get; set; }

		public bool RadioBtnFiveIsChecked { get; set; }

		public ObservableCollection<string> Messages { get; }

		public DelegateCommand<string> NavigateCommand { get; }

		public DelegateCommand ShowPageDialog { get; }

		public DelegateCommand ShowDialog { get; }

		public ICommand CounterClickedCommand { get; private set; }

		public ICommand Counter2ClickedCommand { get; private set; }

		public ICommand Counter3ClickedCommand { get; private set; }

		public ICommand Button4ClickedCommand { get; private set; }

		public ICommand RadioButton1ChangedCommand { get; private set; }

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

			if (this.RadioBtnOneIsChecked)
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

				var timer = Application.Current.Dispatcher.CreateTimer();
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

		private void HideButtonWithImage()
		{
			Microsoft.Maui.ApplicationModel.MainThread.BeginInvokeOnMainThread(() =>
			{
				CounterBtn2ImageVisible = false;
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

		private void RadioButton_CheckedChanged(object arg)
		{
			if (RadioBtnOneIsChecked || RadioBtnThreeIsChecked || RadioBtnFiveIsChecked)
			{
				Console.WriteLine("RadioButton_CheckedChanged SELECTED in group: \"" + arg);
			}
		}

		private bool RadioButton_CheckedChangedCanSubmit(object arg)
		{
			return true;
		}
	}
}
