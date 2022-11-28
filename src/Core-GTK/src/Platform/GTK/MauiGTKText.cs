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
		private static double colorMaxValue = 65535;

		public static string GetUpdateText(string text, int fontSize, Gdk.Color fontGdkColor)
		{
			var font_Size = (int)(fontSize * Pango.Scale.PangoScale);
			var fontColor = "#";
			// color.Red / colorMaxValue, color.Green / colorMaxValue, color.Blue / colorMaxValue, 1.0
			var red = (ushort)(fontGdkColor.Red / colorMaxValue) * 0xFF;
			var green = (ushort)(fontGdkColor.Green / colorMaxValue) * 0xFF;
			var blue = (ushort)(fontGdkColor.Blue / colorMaxValue) * 0xFF;
			fontColor += red.ToString("X2") + green.ToString("X2") + blue.ToString("X2");
			var markup = "<span foreground='";
			markup += fontColor;
			markup += "' size='";
			markup += font_Size.ToString("D");
			markup += "'>";
			markup += text;
			markup += "</span>";
			return markup;
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

			var markup = string.Empty;

			if (button.TextColor == null)
			{
				markup += "<span";
			}
			else
			{
				markup += "<span foreground='";
				markup += button.TextColor.ToColorString();
				markup += "'";
			}

			if (button.Font.Family == null)
			{
				if (button.TextColor != null)
				{
					markup += "'";
				}
			}
			else
			{
				if (button.TextColor != null)
				{
					markup += "'";
				}
				markup += " font_desc='";
				markup += button.Font.Family;
				markup += "'";
			}

			if (button.Font.Size > 0)
			{
				if (button.TextColor != null || button.Font.Family != null)
				{
					markup += "'";
				}
				markup += " font_size='";
				markup += Convert.ToInt32(button.Font.Size).ToString("D");
				markup += "'";
			}

			markup += ">";
			markup += chosenText;
			markup += "</span>";
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
	}
}
