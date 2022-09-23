namespace Microsoft.Maui.Platform
{
	public class MauiStepper : MauiView
	{
		public MauiStepper(double min, double max, double step)
		{
			StepperWidget = new Gtk.SpinButton(min, max, step);
			Add(StepperWidget);
		}

		public Gtk.SpinButton StepperWidget { get; set; }
	}
}
