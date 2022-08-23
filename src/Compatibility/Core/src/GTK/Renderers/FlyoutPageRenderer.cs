using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Gtk;
using Microsoft.Maui.Controls.Internals;
using Microsoft.Maui.Controls.Compatibility.Platform.GTK.Controls;
using Microsoft.Maui.Controls.Compatibility.Platform.GTK.Extensions;

namespace Microsoft.Maui.Controls.Compatibility.Platform.GTK.Renderers
{
	public class FlyoutPageRenderer : AbstractPageRenderer<Controls.FlyoutPage, FlyoutPage>
	{
		Page _currentFlyout = null!;
		Page _currentDetail = null!;

		public FlyoutPageRenderer()
		{
			MessagingCenter.Subscribe(this, Forms.BarTextColor, (NavigationPage? sender, Graphics.Color color) =>
			{
				var barTextColor = color;

				if (barTextColor == null || barTextColor.IsDefaultOrTransparent())
				{
					if (Widget != null)
						Widget.UpdateBarTextColor(null);
				}
				else
				{
					if (Widget != null)
						Widget.UpdateBarTextColor(color.ToGtkColor());
				}
			});

			MessagingCenter.Subscribe(this, Forms.BarBackgroundColor, (NavigationPage? sender, Graphics.Color color) =>
			{
				var barBackgroundColor = color;

				if (barBackgroundColor == null || barBackgroundColor.IsDefaultOrTransparent())
				{
					if (Widget != null)
						Widget.UpdateBarBackgroundColor(null);
				}
				else
				{
					if (Widget != null)
						Widget.UpdateBarBackgroundColor(color.ToGtkColor());
				}
			});
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (Widget != null)
				{
					Widget.IsPresentedChanged -= OnIsPresentedChanged;
				}

				MessagingCenter.Unsubscribe<NavigationPage, Color>(this, Forms.BarTextColor);
				MessagingCenter.Unsubscribe<NavigationPage, Color>(this, Forms.BarBackgroundColor);

				if (Page?.Flyout != null)
				{
					Page.Flyout.PropertyChanged -= HandleFlyoutPropertyChanged;
				}
			}

			base.Dispose(disposing);
		}

		protected override void OnElementChanged(VisualElementChangedEventArgs e)
		{
			base.OnElementChanged(e);

			if (e.NewElement != null)
			{
				if (Widget == null)
				{
					// There is nothing similar in Gtk. 
					// Custom control has been created that simulates the expected behavior.
					Widget = new Controls.FlyoutPage();
					var eventBox = new GtkFormsContainer();
					eventBox.Add(Widget);

					Control.Content = eventBox;

					Widget.IsPresentedChanged += OnIsPresentedChanged;

					UpdateFlyoutPage();
					UpdateFlyoutLayoutBehavior();
					UpdateIsPresented();
					UpdateBarTextColor();
					UpdateBarBackgroundColor();
				}
			}
		}

		protected override void OnSizeAllocated(Gdk.Rectangle allocation)
		{
			base.OnSizeAllocated(allocation);

			Control?.Content?.SetSize(allocation.Width, allocation.Height);
		}

		protected override void OnElementPropertyChanged(object? sender, PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);


			if (!string.IsNullOrEmpty(e.PropertyName) && (e.PropertyName.Equals(nameof(FlyoutPage.Flyout), StringComparison.Ordinal) 
				|| e.PropertyName.Equals(nameof(FlyoutPage.Detail), StringComparison.Ordinal)))
			{
				UpdateFlyoutPage();
				UpdateFlyoutLayoutBehavior();
				UpdateIsPresented();
			}
			else if (e.PropertyName == FlyoutPage.IsPresentedProperty.PropertyName)
				UpdateIsPresented();
			else if (e.PropertyName == FlyoutPage.FlyoutLayoutBehaviorProperty.PropertyName)
				UpdateFlyoutLayoutBehavior();
		}

		private async void HandleFlyoutPropertyChanged(object? sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == Microsoft.Maui.Controls.Page.IconImageSourceProperty.PropertyName)
				await UpdateHamburguerIconAsync();
		}

		private void UpdateFlyoutPage()
		{
			Gtk.Application.Invoke(async delegate
			{
				await UpdateHamburguerIconAsync();
				if (Page != null && Page.Flyout != _currentFlyout)
				{
					if (_currentFlyout != null)
					{
						_currentFlyout.PropertyChanged -= HandleFlyoutPropertyChanged;
					}
					if (Platform.GetRenderer(Page.Flyout) == null)
						Platform.SetRenderer(Page.Flyout, Platform.CreateRenderer(Page.Flyout));
					if (Widget != null)
					{
						Widget.Flyout = Platform.GetRenderer(Page.Flyout).Container;
						Widget.FlyoutTitle = Page.Flyout?.Title ?? string.Empty;
						if (Page.Flyout != null)
							Page.Flyout.PropertyChanged += HandleFlyoutPropertyChanged;

						if (Page != null && Page.Flyout != null)
							_currentFlyout = Page.Flyout;
					}
				}
				if (Page != null && Page.Detail != _currentDetail)
				{
					if (Platform.GetRenderer(Page.Detail) == null)
						Platform.SetRenderer(Page.Detail, Platform.CreateRenderer(Page.Detail));

					if (Widget != null)
						Widget.Detail = Platform.GetRenderer(Page.Detail).Container;

					_currentDetail = Page.Detail;
				}
				UpdateBarTextColor();
				UpdateBarBackgroundColor();
			});
		}

		private void UpdateIsPresented()
		{
			if (Widget != null && Page != null)
				Widget.IsPresented = Page.IsPresented;
		}

		private void UpdateFlyoutLayoutBehavior()
		{
			if (Page == null || Widget == null)
				return;

			if (Page.Detail is NavigationPage)
			{
				Widget.FlyoutLayoutBehaviorType = GetFlyoutLayoutBehavior(Page.FlyoutLayoutBehavior);
			}
			else
			{
				// The only way to display Flyout page is from a toolbar. If we have not access to one,
				// we should force split mode to display menu (as no gestures are implemented).
				Widget.FlyoutLayoutBehaviorType = FlyoutLayoutBehaviorType.Split;
			}

			Widget.DisplayTitle = Widget.FlyoutLayoutBehaviorType != FlyoutLayoutBehaviorType.Split;
		}

		private void UpdateBarTextColor()
		{
			if (Page == null || Widget == null)
				return;

			var navigationPage = Page.Detail as NavigationPage;

			if (navigationPage != null)
			{
				var barTextColor = navigationPage.BarTextColor;

				Widget.UpdateBarTextColor(barTextColor.ToGtkColor());
			}
		}

		private void UpdateBarBackgroundColor()
		{
			if (Page == null || Widget == null)
				return;

			var navigationPage = Page.Detail as NavigationPage;

			if (navigationPage != null)
			{
				var barBackgroundColor = navigationPage.BarBackgroundColor;
				Widget.UpdateBarBackgroundColor(barBackgroundColor.ToGtkColor());
			}
		}

		private Task UpdateHamburguerIconAsync()
		{
			if (Page == null || Widget == null)
				return Task.FromResult(0);

			return Page.Flyout.ApplyNativeImageAsync(Microsoft.Maui.Controls.Page.IconImageSourceProperty, image =>
			{
				if (image != null)
					Widget.UpdateHamburguerIcon(image);

				if (Page.Detail is NavigationPage navigationPage)
				{
					var navigationRenderer = Platform.GetRenderer(navigationPage) as IToolbarTracker;
					navigationRenderer?.NativeToolbarTracker.UpdateToolBar();
				}
			});
		}

		private FlyoutLayoutBehaviorType GetFlyoutLayoutBehavior(FlyoutLayoutBehavior flyoutBehavior)
		{
			switch (flyoutBehavior)
			{
				case FlyoutLayoutBehavior.Split:
				case FlyoutLayoutBehavior.SplitOnLandscape:
				case FlyoutLayoutBehavior.SplitOnPortrait:
					return FlyoutLayoutBehaviorType.Split;
				case FlyoutLayoutBehavior.Popover:
					return FlyoutLayoutBehaviorType.Popover;
				case FlyoutLayoutBehavior.Default:
					return FlyoutLayoutBehaviorType.Default;
				default:
					throw new ArgumentOutOfRangeException(nameof(flyoutBehavior));
			}
		}

		private void OnIsPresentedChanged(object? sender, EventArgs e)
		{
			if (ElementController != null && Widget != null)
				ElementController.SetValueFromRenderer(FlyoutPage.IsPresentedProperty, Widget.IsPresented);
		}
	}

	public class MasterDetailPageRenderer : FlyoutPageRenderer
	{

	}
}
