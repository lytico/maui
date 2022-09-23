using System;
using System.Linq;
using Gtk;

namespace Microsoft.Maui.Platform
{
	public class CustomSliderEventArgs : EventArgs
	{
		public double CustomValue { get; set; }

		public CustomSliderEventArgs(double value)
		{
			CustomValue = value;
		}
	}

	public class CustomSlider : CustomView
	{
		public CustomSlider(double minimum, double maximum, double step)
		{
			HScaleWidget = new Gtk.HScale(minimum, maximum, step);
			CustomMinimum = minimum;
			CustomMaximum = maximum;
			CustomStep = step;

			HScaleWidget.ValueChanged += CustomSlider_ValueChanged;
			Add(HScaleWidget);
		}

		public delegate void CustomSliderEventHandler(object sender, CustomSliderEventArgs args);
		public event CustomSliderEventHandler CustomValueChanged = null!;

		public Gtk.HScale HScaleWidget { get; set; }
		public double CustomMinimum { get; set; }
		public double CustomMaximum { get; set; }
		public double CustomStep { get; set; }

		public void UpdateMinimum(ISlider slider)
		{
			CustomMinimum = slider.Minimum;

			HScaleWidget.SetRange(CustomMinimum, CustomMaximum);
		}

		public void UpdateMaximum(ISlider slider)
		{
			CustomMinimum = slider.Minimum;

			HScaleWidget.SetRange(CustomMinimum, CustomMaximum);
		}

		public void UpdateValue(ISlider slider)
		{
			HScaleWidget.Value = slider.Value;
		}

		private void CustomSlider_ValueChanged(object? sender, EventArgs e)
		{
			if (CustomValueChanged != null!)
			{
				CustomSliderEventArgs args = new CustomSliderEventArgs(HScaleWidget.Value);
				if (sender == null)
					CustomValueChanged.Invoke(this, args);
				else
					CustomValueChanged.Invoke(sender, args);
			}
		}
	}
}
