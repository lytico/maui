using System;
using System.Linq;
using Gtk;

namespace Microsoft.Maui.Platform
{
	public class MauiSliderEventArgs : EventArgs
	{
		public double CustomValue { get; set; }

		public MauiSliderEventArgs(double value)
		{
			CustomValue = value;
		}
	}

	public class MauiSlider : MauiView
	{
		public MauiSlider(double minimum, double maximum, double step)
		{
			HScaleWidget = new Gtk.HScale(minimum, maximum, step);
			CustomMinimum = minimum;
			CustomMaximum = maximum;
			CustomStep = step;

			HScaleWidget.ValueChanged += CustomSlider_ValueChanged;
			Add(HScaleWidget);
		}

		public delegate void CustomSliderEventHandler(object sender, MauiSliderEventArgs args);
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
				MauiSliderEventArgs args = new MauiSliderEventArgs(HScaleWidget.Value);
				if (sender == null)
					CustomValueChanged.Invoke(this, args);
				else
					CustomValueChanged.Invoke(sender, args);
			}
		}
	}
}
