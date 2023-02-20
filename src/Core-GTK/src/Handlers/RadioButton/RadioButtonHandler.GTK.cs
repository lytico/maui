using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Gtk;

namespace Microsoft.Maui.Handlers
{
	public partial class RadioButtonHandler : ViewHandler<IRadioButton, MauiView>
	{
		static MauiView? GetPlatformRadioButton(IRadioButtonHandler handler) => handler.PlatformView as MauiView;
		static Dictionary<Gtk.RadioButton, string> RadioButtonGrouping = new Dictionary<Gtk.RadioButton, string>();

		protected override MauiView CreatePlatformView(IView radioButtonView)
		{
			var plat = new MauiView();
			bool bShow = false;
			if (radioButtonView is IRadioButton radioButton)
			{
				var radioButtonText = string.Empty;
				var radioButtonGroupName = "NULL";

				if (radioButton.Value is string valueText)
				{
					radioButtonText = valueText;
				}

				if (radioButton.GroupName is string foundGroupName)
				{
					radioButtonGroupName = foundGroupName;
				}

				if (RadioButtonGrouping.ContainsValue(radioButtonGroupName))
				{
					var radioButtonGroup = RadioButtonGrouping.FirstOrDefault(x => x.Value.Equals(radioButtonGroupName));
					if (radioButtonGroup.Value.Equals(radioButtonGroupName))
					{
						var platformRadioButton = new Gtk.RadioButton(radioButtonGroup.Key);
						if (!string.IsNullOrEmpty(radioButtonText))
						{
							platformRadioButton.Label = radioButtonText;
						}

						plat.AddChildWidget(platformRadioButton);
					}
					else
					{
						var platformRadioButton = new Gtk.RadioButton((Gtk.RadioButton)null!);
						if (!string.IsNullOrEmpty(radioButtonText))
						{
							platformRadioButton.Label = radioButtonText;
						}

						RadioButtonGrouping.Add(platformRadioButton, radioButtonGroupName);

						plat.AddChildWidget(platformRadioButton);
					}
				}
				else
				{
					var platformRadioButton = new Gtk.RadioButton((Gtk.RadioButton)null!);
					if (!string.IsNullOrEmpty(radioButtonText))
					{
						platformRadioButton.Label = radioButtonText;
					}

					RadioButtonGrouping.Add(platformRadioButton, radioButtonGroupName);

					plat.AddChildWidget(platformRadioButton);
				}

				if (radioButton.Visibility == Visibility.Visible)
				{
					bShow = true;
				}
			}
			Gtk.Widget widget = plat;
			SetMargins(radioButtonView, ref widget);

			if (bShow)
			{
				plat.Show();
			}
			return plat;
		}

		protected override void ConnectHandler(MauiView platformView)
		{
			if (platformView.GetChildWidget() is Gtk.RadioButton radioButton)
			{
				//radioButton.Clicked += RadioButton_Clicked;
				radioButton.Released += RadioButton_Released;
			}

			//RadioButton? platformRadioButton = GetPlatformRadioButton(this);
			//if (platformRadioButton != null)
			//	platformRadioButton.cli
			//	platformRadioButton.CheckedChange += OnCheckChanged;
		}

		private void RadioButton_Released(object sender, EventArgs e)
		{
			// Console.WriteLine("Radio Button Clicked");
			if (VirtualView != null)
			{
				if (sender is Gtk.RadioButton radioButton)
				{
					if (radioButton.Active)
					{
						VirtualView.IsChecked = true;
						// Console.WriteLine("Read Checked!");
					}
					else
					{
						VirtualView.IsChecked = false;
						// Console.WriteLine("Read NOT Checked!");
					}
				}
			}
		}

		private void RadioButton_Clicked(object sender, System.EventArgs e)
		{
			//// Debug.WriteLine("Clicked");
			//Console.WriteLine("Radio Button Clicked");
			//if (VirtualView != null)
			//{
			//	if (sender is Gtk.RadioButton radioButton)
			//	{
			//		if (radioButton.Active)
			//		{
			//			VirtualView.IsChecked = true;
			//			// Console.WriteLine("Read Checked!");
			//		}
			//		else
			//		{
			//			VirtualView.IsChecked = false;
			//			// Console.WriteLine("Read NOT Checked!");
			//		}
			//	}
			//}
		}

		protected override void DisconnectHandler(MauiView platformView)
		{
			if (platformView.GetChildWidget() is Gtk.RadioButton radioButton)
			{
				//radioButton.Clicked -= RadioButton_Clicked;
				radioButton.Released -= RadioButton_Released;
				if (RadioButtonGrouping.ContainsKey(radioButton))
				{
					RadioButtonGrouping.Remove(radioButton);
				}
			}
			//if (platformView is AppCompatRadioButton platformRadioButton)
			//	platformRadioButton.CheckedChange -= OnCheckChanged;
		}

		public static void MapBackground(IRadioButtonHandler handler, IRadioButton radioButton)
		{
			//GetPlatformRadioButton(handler)?.UpdateBackground(radioButton);
		}

		public static void MapIsChecked(IRadioButtonHandler handler, IRadioButton radio)
		{
			if (handler.PlatformView?.GetChildWidget() is Gtk.RadioButton radioButton)
			{
				if (radio.IsChecked)
				{
					radioButton.Active = true;
					//Console.WriteLine("Mark Checked!");
				}
				else
				{
					radioButton.Active = false;
					//Console.WriteLine("Mark NOT Checked!");
				}
			}
			//GetPlatformRadioButton(handler)?.UpdateIsChecked(radioButton);
		}

		public static void MapContent(IRadioButtonHandler handler, IRadioButton radioButton)
		{
			//GetPlatformRadioButton(handler)?.UpdateContent(radioButton);
		}

		public static void MapTextColor(IRadioButtonHandler handler, ITextStyle textStyle)
		{
			//GetPlatformRadioButton(handler)?.UpdateTextColor(textStyle);
		}

		public static void MapCharacterSpacing(IRadioButtonHandler handler, ITextStyle textStyle)
		{
			//GetPlatformRadioButton(handler)?.UpdateCharacterSpacing(textStyle);
		}

		public static void MapFont(IRadioButtonHandler handler, ITextStyle textStyle)
		{
			//var fontManager = handler.GetRequiredService<IFontManager>();

			//GetPlatformRadioButton(handler)?.UpdateFont(textStyle, fontManager);
		}

		public static void MapStrokeColor(IRadioButtonHandler handler, IRadioButton radioButton)
		{
			//GetPlatformRadioButton(handler)?.UpdateStrokeColor(radioButton);
		}

		public static void MapStrokeThickness(IRadioButtonHandler handler, IRadioButton radioButton)
		{
			//GetPlatformRadioButton(handler)?.UpdateStrokeThickness(radioButton);
		}

		public static void MapCornerRadius(IRadioButtonHandler handler, IRadioButton radioButton)
		{
			//GetPlatformRadioButton(handler)?.UpdateCornerRadius(radioButton);
		}

		//void OnCheckChanged(object? sender, CompoundButton.CheckedChangeEventArgs e)
		//{
		//	if (VirtualView == null)
		//		return;

		//	VirtualView.IsChecked = e.IsChecked;
		//}
	}
}