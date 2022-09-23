using System;
using Gtk;
using Pango;

namespace Microsoft.Maui.Platform
{
	public class MauiStepperControl : MauiView
	{
		public MauiStepperControl(double min, double max, double step)
		{
			StepperWidget = new Gtk.SpinButton(min, max, step);
			Add(StepperWidget);
		}

		public Gtk.SpinButton StepperWidget { get; set; }
	}
}
