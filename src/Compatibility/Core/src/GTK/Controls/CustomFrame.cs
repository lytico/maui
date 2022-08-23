using Gdk;
using Gtk;
using Microsoft.Maui.Controls.Compatibility.Platform.GTK.Extensions;

namespace Microsoft.Maui.Controls.Compatibility.Platform.GTK.Controls
{
	public class CustomFrame : Gtk.Frame
	{
		private Graphics.Color _defaultBorderColor;
		private Graphics.Color _defaultBackgroundColor;
		private Graphics.Color? _borderColor;
		private Graphics.Color? _backgroundColor;

		private uint _borderWidth;
		private bool _hasShadow;
		private uint _shadowWidth;

		public CustomFrame()
		{
			ShadowType = ShadowType.None;
			BorderWidth = 0;

			_borderWidth = 0;
			_hasShadow = false;
			_shadowWidth = 2;
			_defaultBackgroundColor = Style.Backgrounds[(int)StateType.Normal].ToMAUIColor();
			_defaultBorderColor = Style.BaseColors[(int)StateType.Active].ToMAUIColor();
		}

		public void SetBackgroundColor(Graphics.Color? color)
		{
			_backgroundColor = color;
			QueueDraw();
		}

		public void ResetBackgroundColor()
		{
			_backgroundColor = _defaultBackgroundColor;
			QueueDraw();
		}

		public void SetBorderWidth(uint width)
		{
			_borderWidth = width;
			QueueDraw();
		}

		public void SetBorderColor(Graphics.Color? color)
		{
			_borderColor = color;
			QueueDraw();
		}

		public void ResetBorderColor()
		{
			_borderColor = _defaultBorderColor;
			QueueDraw();
		}

		public void SetShadow()
		{
			_hasShadow = true;
			QueueDraw();
		}

		public void ResetShadow()
		{
			_hasShadow = false;
			QueueDraw();
		}

		public void SetShadowWidth(uint width)
		{
			_shadowWidth = width;
			QueueDraw();
		}

		protected override bool OnExposeEvent(EventExpose evnt)
		{
			using (var cr = CairoHelper.Create(GdkWindow))
			{
				// Draw Shadow
				if (_hasShadow)
				{
					var color = Graphics.Colors.Black;
					cr.SetSourceRGBA(color.Red, color.Green, color.Blue, color.Alpha);
					cr.Rectangle(Allocation.Left + _shadowWidth, Allocation.Top + _shadowWidth, Allocation.Width + _shadowWidth, Allocation.Height + _shadowWidth);
					cr.Fill();
				}

				// Draw BackgroundColor
				if (_backgroundColor != null)
				{
					var color = _backgroundColor;
					cr.SetSourceRGBA(color.Red, color.Green, color.Blue, color.Alpha);
					cr.Rectangle(Allocation.Left, Allocation.Top, Allocation.Width, Allocation.Height);
					cr.FillPreserve();
				}

				// Draw BorderColor
				if (_borderColor != null)
				{
					cr.LineWidth = _borderWidth;
					var color = _borderColor;
					cr.SetSourceRGBA(color.Red, color.Green, color.Blue, color.Alpha);
					cr.Rectangle(Allocation.Left, Allocation.Top, Allocation.Width, Allocation.Height);
					cr.StrokePreserve();
				}
			}

			return base.OnExposeEvent(evnt);
		}
	}
}
