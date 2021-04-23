using System;
using Cairo;
using Gtk;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Graphics.Native.Gtk;

namespace Microsoft.Maui
{
	public class LayoutView : Box
	{
#if DEBUG
		public LayoutView():base(Orientation.Vertical,0)
		{
		}

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

		internal Func<Size>? CrossPlatformSize { get; set; }

		int SizeFor(Orientation or, Graphics.Size size)
			=> (int) (or == Orientation.Horizontal ? size.Width : size.Height);
		// protected override void OnAdjustSizeRequest(Orientation orientation, out int minimum_size, out int natural_size)
		// {
		// 	base.OnAdjustSizeRequest(orientation, out minimum_size, out natural_size);
		// 	if (CrossPlatformMeasure == null || CrossPlatformSize==null)
		// 	{
		// 		return;
		// 	}
		//
		// 	// var size = SizeFor(orientation,CrossPlatformSize());
		// 	// minimum_size = Math.Max(size,minimum_size);
		// 	// natural_size = Math.Max(size,natural_size);
		// 	
		// }

		// protected override void OnSizeAllocated(Gdk.Rectangle allocation)
		// {
		// 	if (CrossPlatformArrange == null)
		// 	{
		// 		base.OnSizeAllocated(allocation);
		// 		return;
		// 	}
		// 	//
		// 	// CrossPlatformArrange(allocation.ToRectangle());
		// }
		
		
		public new void Add(Widget child)
		{
			PackStart(child,false,false,0);
		}

		public new void Remove (Widget child)
		{
			base.Remove(child);
		}
		
	}
}