using System;
using System.Collections.Generic;
using System.Text;
using Gtk;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Handlers;

namespace Microsoft.Maui.Platform.GTK
{
	public static class MauiGTKText
	{
		// private static double colorMaxValue = 65535;

		public static string GetUpdateText(string text, int fontSize, Gdk.Color fontGdkColor, string fontFamily)
		{
			var byteRed = (int)fontGdkColor.Red;
			var byteGreen = (int)fontGdkColor.Green;
			var byteBlue = (int)fontGdkColor.Blue;
			var mauiColor = new Color(byteRed, byteGreen, byteBlue);
			var textStyle = new TextStyleContainer(mauiColor, fontFamily, fontSize);
			return CalculateMarkupAttributes(text, textStyle);
			//var font_Size = (int)(fontSize * Pango.Scale.PangoScale);
			//var fontColor = "#";
			//// color.Red / colorMaxValue, color.Green / colorMaxValue, color.Blue / colorMaxValue, 1.0
			//var red = (ushort)(fontGdkColor.Red / colorMaxValue) * 0xFF;
			//var green = (ushort)(fontGdkColor.Green / colorMaxValue) * 0xFF;
			//var blue = (ushort)(fontGdkColor.Blue / colorMaxValue) * 0xFF;
			//fontColor += red.ToString("X2") + green.ToString("X2") + blue.ToString("X2");
			//var markup = "<span foreground='";
			//markup += fontColor;
			//markup += "' size='";
			//markup += font_Size.ToString("D");
			//markup += "'>";
			//markup += text;
			//markup += "</span>";
			//return markup;
		}

		public static void SetMarkupAttributes(object handler, ITextStyle button, string buttonText = "")
		{
			string chosenText = string.Empty;

			if (handler is IButtonHandler buttonHandler)
			{
				if (buttonHandler.PlatformView is MauiGTKButton handlerButtonBox)
				{
					if (handlerButtonBox.GetInternalLabel == null)
					{
						if (string.IsNullOrEmpty(buttonText))
						{
							return;
						}
						else
						{
							handlerButtonBox.Label = buttonText;
						}
					}

					if (handlerButtonBox.GetInternalLabel == null)
					{
						return;
					}

					chosenText = handlerButtonBox.GetInternalLabel.Text;
				}
			}
			else if (handler is ILabelHandler labelHandler)
			{
				if (labelHandler.PlatformView is MauiGTKLabel handlerLabelBox)
				{
					if (handlerLabelBox == null)
					{
						if (string.IsNullOrEmpty(buttonText))
						{
							return;
						}
						return;
					}
					else
					{
						handlerLabelBox.Text = buttonText;
					}

					if (handlerLabelBox.Text == null)
					{
						return;
					}

					chosenText = handlerLabelBox.Text;
				}
			}

			var markup = CalculateMarkupAttributes(chosenText, button);

			if (handler is IButtonHandler buttonHandlerWriter)
			{
				if (buttonHandlerWriter.PlatformView is MauiGTKButton handlerButtonBox)
				{
					handlerButtonBox.GetInternalLabel.Markup = markup;
				}
			}
			else if (handler is ILabelHandler labelHandlerWriter)
			{
				if (labelHandlerWriter.PlatformView is MauiGTKLabel handlerLabelBox)
				{
					handlerLabelBox.Markup = markup;
				}
			}
		}

		public static string CalculateMarkupAttributes(string chosenText, ITextStyle button)
		{
			var markup = string.Empty;

			if (button.TextColor == null)
			{
				markup += "<span";
			}
			else
			{
				markup += "<span foreground='";

				var fontColor = "#";
				// color.Red / colorMaxValue, color.Green / colorMaxValue, color.Blue / colorMaxValue, 1.0
				var red = (ushort)(button.TextColor.Red * 0xFF);
				var green = (ushort)(button.TextColor.Green * 0xFF);
				var blue = (ushort)(button.TextColor.Blue * 0xFF);
				fontColor += red.ToString("X2") + green.ToString("X2") + blue.ToString("X2");
				markup += fontColor;

				// markup += button.TextColor.ToColorString();
				markup += "'";
			}

			if (button.Font.Family != null)
			{
				markup += " font_desc='";
				markup += button.Font.Family;
				markup += "'";
			}

			if (button.Font.Size > 0)
			{
				var font_Size = (int)(button.Font.Size * Pango.Scale.PangoScale);
				markup += " font_size='";
				markup += font_Size.ToString("D");
				markup += "'";
			}

			markup += ">";
			markup += chosenText;
			markup += "</span>";

			// Getting:
			// <span foreground='#000000FF''' font_size='18432'>Welcome to .NET Multi-platform App UI for GTK</span>

			return markup;
		}
	}

	public class TextStyleContainer : ITextStyle
	{
		public Color TextColor { get; }

		public Font Font { get; }

		public double CharacterSpacing { get; }

		public TextStyleContainer(Color textColor, string fontFamily, double size)
		{
			// var fontBuilder2 = new Font
			// var fontBuilder = new Font { Family = fontFamily, Slant = FontSlant.Default, Weight = FontWeight.Regular };
			var fontBuilder = Font.OfSize(fontFamily, size);

			TextColor = textColor;
			Font = fontBuilder;
			CharacterSpacing = 1.0;
		}
	}
}
