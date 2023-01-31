using System;

namespace Microsoft.Maui.Platform
{
	public partial class NavigationRootManager
	{
		// IMauiContext _mauiContext;
		MauiGTKWindow _platformWindow;
		//ContentViewGroup _rootView;
		//bool _disconnected = true;
		bool _isActiveRootManager;

		// TODO MAUI: temporary event to alert when rootview is ready
		// handlers and various bits use this to start interacting with rootview
		internal event EventHandler? RootViewChanged;

		public MauiGTKWindow RootView => _platformWindow;

		public NavigationRootManager(MauiGTKWindow platformWindow)
		{
			// _mauiContext = mauiContext;
			_platformWindow = platformWindow;
			//_rootView = new ContentViewGroup();
			// _rootView = new MauiGTKWindow("My Maui GTK Window");
			//_rootView.BackRequested += OnBackRequested;
			//_rootView.OnApplyTemplateFinished += OnApplyTemplateFinished;
			//_rootView.OnAppTitleBarChanged += OnAppTitleBarChanged;
		}

		//void OnBackRequested(NavigationView sender, NavigationViewBackRequestedEventArgs args)
		//{
		//	_platformWindow
		//		.GetWindow()?
		//		.BackButtonClicked();
		//}

		//internal ContentControl? AppTitleBarContentControl => _rootView.AppTitleBarContentControl;
		//internal FrameworkElement? AppTitleBar => _rootView.AppTitleBar;
		//internal MauiToolbar? Toolbar => _rootView.Toolbar;

		void OnApplyTemplateFinished(object? sender, EventArgs e)
		{
			//if (_rootView.AppTitleBar != null)
			//{
			//	_platformWindow.ExtendsContentIntoTitleBar = true;
			//	UpdateAppTitleBar(true);
			//}
		}

		void OnAppTitleBarChanged(object? sender, EventArgs e)
		{
			UpdateAppTitleBar(true);
			//if (AppTitleBar != null)
			//{
			//	var handle = _platformWindow.GetWindowHandle();
			//	var result = PlatformMethods.GetCaptionButtonsBound(handle);
			//	_rootView.UpdateAppTitleBar(result, _platformWindow.ExtendsContentIntoTitleBar);
			//}
		}

		public virtual void Connect(IWindow window)
		{
			ClearPlatformParts();

			var rootNavigationView = new RootNavigationView();
			var containerView = window.ToContainerView();

			AddPlatformParts(containerView);
			//_rootView = containerView;

			//SetContentView(containerView);

			RootViewChanged?.Invoke(this, EventArgs.Empty);
		}

		public virtual void Disconnect()
		{
			//_platformWindow.Activated -= OnWindowActivated;
			//_rootView.Content = null;
			//_disconnected = true;
		}

		void ClearPlatformParts()
		{
			// _rootView = null;
			if (_platformWindow != null)
			{
				foreach (var child in _platformWindow.Children)
				{
					_platformWindow.Remove(child);
				}
			}
		}

		void AddPlatformParts(MauiGTKWindow? containerView)
		{
			if (containerView != null && _platformWindow != null)
			{
				foreach (var child in containerView.Children)
				{
					_platformWindow.Add(child);
					return;
				}
			}

		}

		internal void UpdateAppTitleBar(bool isActive)
		{
			//if (_rootView.AppTitleBarContentControl != null &&
			//	_platformWindow.ExtendsContentIntoTitleBar)
			//{
			//	if (isActive)
			//	{
			//		_rootView.Visibility = UI.Xaml.Visibility.Visible;
			//		SetTitleBar(_rootView.AppTitleBarContentControl);

			//		SetWindowTitle(_platformWindow.GetWindow()?.Title);
			//	}
			//	else
			//	{
			//		_rootView.Visibility = UI.Xaml.Visibility.Collapsed;
			//	}
			//}
			//else
			//{
			//	SetTitleBar(null);
			//}

			//if (!_isActiveRootManager && isActive)
			//{
			//	_platformWindow.Activated += OnWindowActivated;
			//}
			//else if (!isActive)
			//{
			//	_platformWindow.Activated -= OnWindowActivated;
			//}

			_isActiveRootManager = isActive;
		}

		//void SetTitleBar(UIElement? titleBar)
		//{
		//	if (_platformWindow is MauiWinUIWindow mauiWindow)
		//		mauiWindow.MauiCustomTitleBar = titleBar;
		//	else
		//		_platformWindow.SetTitleBar(titleBar);
		//}

		//internal void SetWindowTitle(string? title)
		//{
		//	_rootView.SetWindowTitle(title);
		//}

		//internal void SetMenuBar(MenuBar? menuBar)
		//{
		//	_rootView.MenuBar = menuBar;
		//}

		//internal void SetToolbar(FrameworkElement? toolBar)
		//{
		//	_rootView.Toolbar = toolBar as MauiToolbar;
		//}

		//void OnWindowActivated(object sender, WindowActivatedEventArgs e)
		//{
		//	if (!_isActiveRootManager)
		//	{
		//		_platformWindow.Activated -= OnWindowActivated;
		//	}

		//	if (_rootView.AppTitle == null)
		//		return;

		//	SolidColorBrush defaultForegroundBrush = (SolidColorBrush)Application.Current.Resources["TextFillColorPrimaryBrush"];
		//	SolidColorBrush inactiveForegroundBrush = (SolidColorBrush)Application.Current.Resources["TextFillColorDisabledBrush"];

		//	if (e.WindowActivationState == WindowActivationState.Deactivated)
		//	{
		//		_rootView.AppTitle.Foreground = inactiveForegroundBrush;
		//	}
		//	else
		//	{
		//		_rootView.AppTitle.Foreground = defaultForegroundBrush;
		//		SetWindowTitle(_platformWindow.GetWindow()?.Title);
		//	}
		//}

		//void SetContentView(MauiView? view)
		//{
		//	if (view != null)
		//	{
		//		_viewFragment = new ViewFragment(view);
		//		FragmentManager
		//			.BeginTransaction()
		//			.Replace(Resource.Id.navigationlayout_content, _viewFragment)
		//			.SetReorderingAllowed(true)
		//			.Commit();
		//	}
		//}
	}
}