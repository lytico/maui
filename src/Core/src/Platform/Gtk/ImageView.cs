using System;
using System.Net.Mime;
using Cairo;
using Gtk;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Graphics.Platform.Gtk;
using Rectangle = Gdk.Rectangle;

namespace Microsoft.Maui.Platform
{

	// https://docs.gtk.org/gtk3/class.Image.html 

	// GtkImage has nothing like Aspect; maybe an ownerdrawn class is needed 
	// could be: https://docs.gtk.org/gtk3/class.DrawingArea.html
	// or Microsoft.Maui.Graphics.Platform.Gtk.GtkGraphicsView

	public class ImageView : Gtk.DrawingArea
	{

		public ImageView()
		{
			CanFocus = true;
			AddEvents((int)Gdk.EventMask.AllEventsMask);
		}

		Gdk.Pixbuf? Pixbuf;

		public Gdk.Pixbuf? Image
		{
			get => Pixbuf;
			set
			{
				Pixbuf = value;
				QueueResize();
			}
		}

		Aspect _aspect;

		public Aspect Aspect
		{
			get => _aspect;
			set
			{
				_aspect = value;
			}
		}

		public static Size Measure(Aspect aspect, Size desiredSize, double widthConstraint, double heightConstraint)
		{
			var desiredAspect = Math.Round(desiredSize.Width / desiredSize.Height,5);
			var constraintAspect = Math.Round(widthConstraint / heightConstraint,5);

			var desiredWidth = desiredSize.Width;
			var desiredHeight = desiredSize.Height;

			if (desiredWidth == 0 || desiredHeight == 0)
				return new SizeRequest(new Size(0, 0));

			var width = desiredWidth;
			var height = desiredHeight;

			if (constraintAspect > desiredAspect)
			{
				// constraint area is proportionally wider than image
				switch (aspect)
				{
					case Aspect.AspectFit:
						height = Math.Min(desiredHeight, heightConstraint);
						width = desiredWidth * (height / desiredHeight);

						break;
					case Aspect.AspectFill:
						height = heightConstraint;
						width = desiredWidth * (height / desiredHeight);

						break;
					case Aspect.Fill:
						width = widthConstraint;
						height = heightConstraint;

						break;
				}
			}
			else if (constraintAspect <= desiredAspect)
			{
				// constraint area is proportionally taller than image
				switch (aspect)
				{
					case Aspect.AspectFit:
						width = Math.Min(desiredWidth, widthConstraint);
						height = desiredHeight * (width / desiredWidth);

						break;
					case Aspect.AspectFill:
						width = widthConstraint;
						height = desiredHeight * (width / desiredWidth);

						break;
					case Aspect.Fill:
						height = heightConstraint;
						width = widthConstraint;

						break;
				}
			}
			else
			{
				// constraint area is same aspect as image
				width = Math.Min(desiredWidth, widthConstraint);
				height = desiredHeight * (width / desiredWidth);
			}

			return new Size(width, height);
		}

		protected override bool OnDrawn(Context cr)
		{
			var a = Allocation;
			var stc = this.StyleContext;
			stc.RenderBackground(cr, 0, 0, a.Width, a.Height);

			if (Image is not { } image)
				return true;

			// HACK: Gtk sends sometimes a draw event while the widget reallocates.
			//       In that case we would draw in the wrong area, which may lead to artifacts
			//       if no other widget updates it. Alternative: we could clip the
			//       allocation bounds, but this may have other issues.
			if (a.Width == 1 && a.Height == 1 && a.X == -1 && a.Y == -1) // the allocation coordinates on reallocation
				return base.OnDrawn(cr);

			var imageSize = Measure(Aspect, new Size(image.Width, image.Height), a.Width, a.Height);
			var x = (a.Width - imageSize.Width) / 2;
			var y = (a.Height - imageSize.Height) / 2;

			cr.DrawPixbuf(image, x, y, imageSize.Width, imageSize.Height);

			return base.OnDrawn(cr);
		}

		protected override void OnAdjustSizeRequest(Orientation orientation, out int minimum_size, out int natural_size)
		{
			base.OnAdjustSizeRequest(orientation, out minimum_size, out natural_size);

			if (Image is not { })
				return;

		}

		protected override void OnRealized()
		{
			base.OnRealized();
		}

		protected override SizeRequestMode OnGetRequestMode()
		{
			return SizeRequestMode.HeightForWidth;
		}

		protected override void OnSizeAllocated(Rectangle allocation)
		{
			base.OnSizeAllocated(allocation);

		}

		protected override void OnGetPreferredHeightForWidth(int width, out int minimum_height, out int natural_height)
		{
			base.OnGetPreferredHeightForWidth(width, out minimum_height, out natural_height);

			if (Image is { })
			{

				var imgSize = new Size(Image.Width, Image.Height);
				var size = Measure(Aspect, new Size(Image.Width, Image.Height), width, imgSize.Height * (width / imgSize.Width));
				minimum_height = (int)size.Height;
				natural_height = Math.Max(Image.Height, minimum_height);
			}
		}

		protected override void OnGetPreferredWidthForHeight(int height, out int minimum_width, out int natural_width)
		{
			base.OnGetPreferredWidthForHeight(height, out minimum_width, out natural_width);

			if (Image is { })
			{
				var imgSize = new Size(Image.Width, Image.Height);
				var size = Measure(Aspect, new Size(Image.Width, Image.Height), imgSize.Width * (height / imgSize.Height), height);
				minimum_width = (int)size.Width;
				natural_width = Math.Max(Image.Width, minimum_width);

			}
		}

	}

}