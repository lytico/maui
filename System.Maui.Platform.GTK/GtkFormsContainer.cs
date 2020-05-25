using System;
using Cairo;
using Gdk;
using Gtk;

namespace System.Maui.Platform.GTK
{
	/// <summary>
	/// A generic container to embed visual elements.
	/// </summary>
	public class GtkFormsContainer : Gtk.EventBox
	{
		Color _backgroundColor;

		public GtkFormsContainer()
		{
			VisibleWindow = false;
			BackgroundColor = Color.Transparent;
		}

		Color BackgroundColor
		{
			get => _backgroundColor;
			set
			{
				_backgroundColor = value;
				QueueDraw();
			}
		}

		public void SetBackgroundColor(Color color)
		{
			BackgroundColor = color;
		}

		/// <summary>
		/// Subclasses can override this method to draw custom content over the background.
		/// </summary>
		/// <param name="clipArea">The clipped area that needs a redraw.</param>
		/// <param name="cr">Context.</param>
		protected virtual void Draw(Gdk.Rectangle clipArea, Context cr)
		{
		}

		protected override bool OnDrawn(Context cr)
		{		
				// Windowless widgets receive expose events with the whole area of
				// of it's container, so we first clip it to the allocation of the
				// widget it self
				var clipBox = Allocation;
				// Draw first the background with the color defined in BackgroundColor
				cr.Rectangle(clipBox.X, clipBox.Y, clipBox.Width, clipBox.Height);
				cr.Clip();
				cr.Save();
				cr.SetSourceRGBA(BackgroundColor.R, BackgroundColor.G, BackgroundColor.B, BackgroundColor.A);
				cr.Operator = Operator.Over;
				cr.Paint();
				cr.Restore();
				// Let subclasses perform their own drawing operations
				cr.Translate(Allocation.X, Allocation.Y);
				Draw(clipBox, cr);
				// And finally forward the expose event to the children
				return base.OnDrawn(cr);
			
		}

	}
}
