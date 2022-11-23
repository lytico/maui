using System;
using Cairo;
using Gdk;
using GLib;
using Gtk;

namespace Microsoft.Maui.Platform.GTK
{
	public class MauiGTKBox : Box
	{
		private Gdk.Color _defaultBorderColor;
		private Gdk.Color _defaultBackgroundColor;

		public MauiGTKBox(Orientation orientation, int spacing) : base(orientation, spacing)
		{
			_defaultBackgroundColor = new Gdk.Color(0, 0, 0);
			_defaultBorderColor = new Gdk.Color(0, 0, 0);
			BackgroundColor = _defaultBackgroundColor;
			BorderColor = _defaultBorderColor;

			HasWindow = false;
			AppPaintable = true;
		}

		public Gdk.Color? BorderColor { get; set; }
		public Gdk.Color? BackgroundColor { get; set; }
		public uint BorderWidthBox { get; set; } = 5;

		protected override bool OnDrawn(Context cr)
		{
			double colorMaxValue = 65535;

			cr.Rectangle(5, 5, Allocation.Width - 10, Allocation.Height - 10);

			// Draw BackgroundColor
			if (BackgroundColor.HasValue)
			{
				var color = BackgroundColor.Value;
				// Console.WriteLine("OnDrawn color: " + color.Red.ToString("D") + color.Green.ToString("D") + color.Blue.ToString("D"));
				cr.SetSourceRGBA(color.Red / colorMaxValue, color.Green / colorMaxValue, color.Blue / colorMaxValue, 1.0);
				cr.FillPreserve();
			}

			// Draw BorderColor
			if (BorderColor.HasValue && BorderWidthBox > 0)
			{
				var color = BorderColor.Value;
				double[] dashed1 = { 1.0 };

				cr.LineWidth = BorderWidthBox;
				cr.SetDash(dashed1, 0);
				cr.SetSourceRGB(color.Red / colorMaxValue, color.Green / colorMaxValue, color.Blue / colorMaxValue);
				// cr.Stroke();
				cr.StrokePreserve();
			}

			//cr.GetTarget().Dispose();
			//cr.Dispose();

			return base.OnDrawn(cr);
		}
	}
}
