using System;
using Cairo;
using Gtk;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Graphics.Native.Gtk;

namespace Microsoft.Maui
{

	public class LayoutView : Fixed
	{

#if DEBUG
		public LayoutView() : base()
		{ }

		// protected override bool OnDrawn(Context cr)
		// {
		// 	var r = base.OnDrawn(cr);
		//
		// 	cr.Save();
		// 	cr.ShowText($"{nameof(LayoutView)}");
		// 	cr.Restore();
		// 	return r;
		// }
#endif

		internal Func<double, double, Size>? CrossPlatformMeasure { get; set; }

		internal Action<Graphics.Rectangle>? CrossPlatformArrange { get; set; }

		internal System.Action? CrossPlatformArrangeChildren { get; set; }

		public System.Action? CrossPlatformInvalidateChildrenMeasure { get; internal set; }

		internal Func<Size>? CrossPlatformDesiredSizeSize { get; set; }

		int SizeFor(Orientation or, Graphics.Size size)
			=> (int)(or == Orientation.Horizontal ? size.Width : size.Height);

		protected override void OnAdjustSizeRequest(Orientation orient, out int minimum_size, out int natural_size)
		{
			base.OnAdjustSizeRequest(orient, out minimum_size, out natural_size);

			if (CrossPlatformMeasure == null || CrossPlatformDesiredSizeSize == null)
			{
				return;
			}

			// the following is not working code:
			
			CrossPlatformInvalidateChildrenMeasure?.Invoke();
			
			var desiredSize = CrossPlatformDesiredSizeSize();
			var orientedSize = SizeFor(orient, CrossPlatformDesiredSizeSize());

			// var mesuredSize = CrossPlatformMeasure(orient == Orientation.Horizontal ? minimum_size : -1, orient == Orientation.Horizontal ? -1 : minimum_size);
			
			// minimum_size = Math.Max(orientedSize, minimum_size);
			// natural_size = Math.Max(orientedSize, natural_size);
			;
		}

		protected override void OnSizeAllocated(Gdk.Rectangle allocation)
		{

			base.OnSizeAllocated(allocation);
			
			// the following is not working code:

			CrossPlatformInvalidateChildrenMeasure?.Invoke();
			CrossPlatformArrange?.Invoke(allocation.ToRectangle());
			var desiredSize = CrossPlatformDesiredSizeSize?.Invoke();
			// var mesuredSize = CrossPlatformMeasure?.Invoke(allocation.Width,allocation.Height);



			;

		}

		public new void Add(Widget child)
		{
			base.Add(child);
		}

		public new void Remove(Widget child)
		{
			base.Remove(child);
		}

	}

}