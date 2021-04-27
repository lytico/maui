using System;
using System.Linq;
using Cairo;
using Gtk;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Graphics.Native.Gtk;
using Microsoft.Maui.Layouts;

namespace Microsoft.Maui
{

	public class LayoutViewAsFixed : Fixed
	{

#if DEBUG
		public LayoutViewAsFixed() : base()

		{ }

		protected override bool OnDrawn(Context cr)
		{
			var r = base.OnDrawn(cr);

			cr.Save();
			cr.SetSourceColor(Graphics.Colors.Red.ToCairoColor());
			cr.Rectangle(0, 0, Allocation.Width, Allocation.Height);
			cr.Stroke();
			cr.Restore();

			return r;
		}
#endif

		public Func<ILayout>? Layout { get; set; }

		int SizeFor(Orientation or, Graphics.Size size)
			=> (int)(or == Orientation.Horizontal ? size.Width : size.Height);

		int SizeFor(Orientation or, Gtk.Requisition size)
			=> or == Orientation.Horizontal ? size.Width : size.Height;

		protected override void OnAdjustSizeAllocation(Orientation orientation, out int minimum_size, out int natural_size, out int allocated_pos, out int allocated_size)
		{

			base.OnAdjustSizeAllocation(orientation, out minimum_size, out natural_size, out allocated_pos, out allocated_size);
			var layout = Layout?.Invoke();

			if (layout == null)
			{
				return;
			}

			var childNaturalSize = 0;
			var childMinimalSize = 0;

			foreach (var c in layout.Children)
			{

				if (c.Handler?.NativeView is Widget w)
				{

					w.GetPreferredSize(out var cmin, out var cnat);
					childNaturalSize += SizeFor(orientation, cnat);
					childMinimalSize += SizeFor(orientation, cmin);

				}
			}

			var frame = layout.Frame;
			var size = SizeFor(orientation, frame.Size);

			if (size != allocated_size)
			{
				layout.InvalidateMeasure();

				var mesuredSize = layout.Measure(
					orientation == Orientation.Horizontal ? Math.Max(childNaturalSize, childMinimalSize) : frame.Width,
					orientation == Orientation.Vertical ? Math.Max(childNaturalSize, childMinimalSize) : frame.Height);
			}

			minimum_size = childMinimalSize;
			natural_size = childNaturalSize;
			base.OnAdjustSizeAllocation(orientation, out minimum_size, out natural_size, out allocated_pos, out allocated_size);
			;
		}

		void ArrangeChildren(ILayout layout)
		{
			foreach (var element in layout.Children)
			{
				if (element.Handler?.NativeView is Widget w)
				{
					element.Handler?.SetFrame(w.Clip.ToRectangle());

					var r = element.Frame;
					var x = (int)r.X;
					var y = (int)r.Y;

					this.Move(w, x, y);

				}

			}
		}

		void InvalidateChildren(ILayout layout)
		{
			foreach (var element in layout.Children)
			{
				element.InvalidateMeasure();

			}
		}

		protected override void OnSizeAllocated(Gdk.Rectangle allocation)
		{

			base.OnSizeAllocated(allocation);

			var layout = Layout?.Invoke();

			if (layout == null)
				return;

			var alloc = allocation.ToRectangle();
			var frame = layout.Frame;

			if (alloc != frame)
			{
				layout.InvalidateArrange();
				// InvalidateChildren(layout);
				layout.Arrange(new(0, 0, alloc.Width, alloc.Height));
				ArrangeChildren(layout);
				// UpdateFocusChain(Orientation.Vertical);

			}

		}

		public new void Add(Widget child)
		{

			base.Add(child);
		}

		public new void Remove(Widget child)
		{
			base.Remove(child);
		}

		void UpdateFocusChain(Orientation orientation)

		{
			var layout = Layout?.Invoke();

			if (layout == null)
				return;

			var children = layout.Children
				//.OrderBy(v=> orientation == Orientation.Horizontal ? v.Frame.X : v.Frame.Y)
			   .Select(c => c.Handler?.NativeView as Widget)
			   .Where(w => w != null)
			   .OrderBy(v => orientation == Orientation.Horizontal ? v!.Allocation.X : v!.Allocation.Y)
			   .ToArray();

			var focusChain = children.ToArray();

			FocusChain = focusChain;
		}

	}

}