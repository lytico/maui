using System;
using Cairo;
using Gdk;
using GLib;
using Gtk;
using Pango;

namespace Microsoft.Maui.Platform.GTK
{
	public class MauiGTKLabel : Label
	{
		// private double colorMaxValue = 65535;
		private Gdk.Color _defaultBorderColor;
		private Gdk.Color _defaultBackgroundColor;
		private int _fontSize = 16;

		public MauiGTKLabel()
		{
			Initialize(string.Empty);
		}

		public MauiGTKLabel(IView button)
		{
			if (button is ILabel virtualTextButton)
			{
				_fontSize = Convert.ToInt32(virtualTextButton.Font.Size);
				if (virtualTextButton.TextColor != null)
				{
					var red = Convert.ToByte(virtualTextButton.TextColor.Red * 255);
					var green = Convert.ToByte(virtualTextButton.TextColor.Green * 255);
					var blue = Convert.ToByte(virtualTextButton.TextColor.Blue * 255);
					FontColor = new Gdk.Color(red, green, blue);
				}
				//else if (virtualTextButton.StrokeColor != null)
				//{
				//	var red = Convert.ToByte(virtualTextButton.StrokeColor.Red * 255);
				//	var green = Convert.ToByte(virtualTextButton.StrokeColor.Green * 255);
				//	var blue = Convert.ToByte(virtualTextButton.StrokeColor.Blue * 255);
				//	FontColor = new Gdk.Color(red, green, blue);
				//}

				Initialize(virtualTextButton.Text);

				return;
			}

			Initialize(string.Empty);
		}

		public Gdk.Color? BorderColor { get; set; }
		public Gdk.Color? BackgroundColor { get; set; }
		public uint BorderWidthLabel { get; set; } = 5;

		public int FontSize
		{
			get { return _fontSize; }
			set
			{
				_fontSize = value;
			}
		}

		public Gdk.Color FontColor { get; set; }
		public string FontFamily {  get; set; } = null!;

		public void SetBackgroundColor(Gdk.Color backgroundColor)
		{
			BackgroundColor = backgroundColor;
		}

		public void UpdateText(string text)
		{
			this.Text = text;
			var markup = MauiGTKText.GetUpdateText(text, _fontSize, FontColor, FontFamily);
			//var font_Size = (int)(_fontSize * Pango.Scale.PangoScale);
			//var fontColor = "#";
			//// color.Red / colorMaxValue, color.Green / colorMaxValue, color.Blue / colorMaxValue, 1.0
			//var red = (ushort)(FontColor.Red / colorMaxValue) * 0xFF;
			//var green = (ushort)(FontColor.Green / colorMaxValue) * 0xFF;
			//var blue = (ushort)(FontColor.Blue / colorMaxValue) * 0xFF;
			//fontColor += red.ToString("X2") + green.ToString("X2") + blue.ToString("X2");
			//var markup = "<span foreground='";
			//markup += fontColor;
			//markup += "' size='";
			//markup += font_Size.ToString("D");
			//markup += "'>";
			//markup += this.Text;
			//markup += "</span>";
			this.Markup = markup;
		}

		public void UpdateTextHtml(string label)
		{
			this.Markup = label;
		}

		private void Initialize(string label)
		{
			_defaultBackgroundColor = new Gdk.Color(0, 0, 0);
			_defaultBorderColor = new Gdk.Color(0, 0, 0);
			BackgroundColor = _defaultBackgroundColor;
			BorderColor = _defaultBorderColor;

			UpdateText(label);
		}
	}
}
