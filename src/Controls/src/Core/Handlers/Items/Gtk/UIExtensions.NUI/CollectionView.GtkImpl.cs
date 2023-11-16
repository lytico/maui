using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Graphics.Platform.Gtk;
using Point = Microsoft.Maui.Graphics.Point;
using Size = Microsoft.Maui.Graphics.Size;
using Rectangle = Microsoft.Maui.Graphics.Rect;

namespace Gtk.UIExtensions.NUI;

public partial class CollectionView
{
	protected override bool OnDrawn(Cairo.Context cr)
	{
		var r = base.OnDrawn(cr);

		return r;
	}


	bool IsReallocating { get; set; }
	bool IsSizeAllocating { get; set; }
	protected IView? VirtualView { get; set; }

	Rect LastAllocation { get; set; }

	const bool RestrictToMesuredAllocation = true;

	protected Size? MeasuredSizeH { get; set; }

	protected Size? MeasuredSizeV { get; set; }

	protected Size? MeasuredMinimum { get; set; }

	protected void ClearMeasured(bool clearCache = true)
	{
		MeasuredSizeH = null;
		MeasuredSizeV = null;
		MeasuredMinimum = null;
	}

	List<Widget> _children = new();

	protected override void ForAll(bool include_internals, Callback callback)
	{
		base.ForAll(include_internals, callback);
		foreach (var w in _children)
		{
			callback(w);
		}
	}

	public new void Add(Widget widget)
	{
		_children.Add(widget);
		base.Add(widget);
	}

	protected override void OnAdded(Widget widget)
	{
		widget.Parent = this;
		ClearMeasured();
	}

	protected override void OnRemoved(Widget widget)
	{
		widget.Unparent();
		_children.Remove(widget);
		ClearMeasured();
		QueueResize();
	}


	protected override void OnSizeAllocated(Gdk.Rectangle allocation)
	{
		if (IsSizeAllocating)
			return;

		if (VirtualView is not { } virtualView)
		{
			base.OnSizeAllocated(allocation);

			return;
		}

		var clearCache = true;

		try
		{
			IsReallocating = true;

			var mAllocation = allocation.ToRect();

			clearCache = LastAllocation.IsEmpty || mAllocation.IsEmpty || LastAllocation != mAllocation;
			ClearMeasured(clearCache);

			LastAllocation = mAllocation;

			var mesuredAllocation = Measure(allocation.Width, allocation.Height);

			if (RestrictToMesuredAllocation)
				mAllocation.Size = mesuredAllocation;

			ArrangeAllocation(new Rectangle(Point.Zero, mAllocation.Size));
			AllocateChildren(mAllocation);

			if (virtualView.Frame != mAllocation)
			{
				IsSizeAllocating = true;

				Arrange(mAllocation);
			}

			base.OnSizeAllocated(allocation);
		}
		finally
		{
			IsReallocating = false;
			IsSizeAllocating = false;
		}
	}

	IEnumerable<(Widget w, IView view)> GetVisibleChildren()
	{
		if (Adaptor is not { })
			yield break;

		foreach (var cr in _children.ToArray())
		{
			var w = cr;
			if (w is ViewHolder vw && Adaptor.GetTemplatedView(vw.Child) is { } v)
			{
				yield return (w, v);
			}
		}
	}

	void AllocateChildren(Rect allocation)
	{
		if (Adaptor is not { })
			return;

		foreach (var cr in GetVisibleChildren())
		{
			var (w, v) = cr;
			var r = v.Frame;

			if (r.IsEmpty)
				continue;

			var cAlloc = new Gdk.Rectangle((int)(allocation.X + r.X), (int)(allocation.Y + r.Y), (int)r.Width, (int)r.Height);

			// if (cAlloc != w.Allocation) // it's always needed to implicit arrange children:
			w.SizeAllocate(cAlloc);
		}
	}

	void ArrangeAllocation(Rectangle allocation)
	{
		if (VirtualView is not { } virtualView)
			return;
		
		foreach (var cr in GetVisibleChildren())
		{
			var (w, v) = cr;
		}

		VirtualView.Arrange(allocation);
	}

	Size Measure(double widthConstraint, double heightConstraint)
	{
		if (VirtualView is not { } virtualView|| LayoutManager is not {})
			return new Size(widthConstraint, heightConstraint);

		var size = new Size(widthConstraint, heightConstraint);
		LayoutManager.SizeAllocated(size);
		var measured = LayoutManager.GetScrollCanvasSize();
		return measured;
	}

	protected override void OnUnrealized()
	{
		// force reallocation on next realization, since allocation may be lost
		IsReallocating = false;
		ClearMeasured();
		base.OnUnrealized();
	}

	protected override void OnRealized()
	{
		// force reallocation, if unrealized previously
		if (!IsReallocating)
		{
			try
			{
				LastAllocation = Allocation.ToRect();
				Measure(Allocation.Width, Allocation.Height);
			}
			catch
			{
				IsReallocating = false;
			}
		}

		base.OnRealized();
	}


	protected Size MeasureMinimum()
	{
		if (MeasuredMinimum != null)
			return MeasuredMinimum.Value;

		if (VirtualView is not { } virtualView)
			return Size.Zero;

		// ensure all children have DesiredSize:

		Measure(0, double.PositiveInfinity);

		var desiredMinimum = GetVisibleChildren().Select(c => c.view)
			.Aggregate(new Size(), (s, c) => new Size(Math.Max(s.Width, c.DesiredSize.Width), s.Height + c.DesiredSize.Height));


		MeasuredMinimum = Measure(desiredMinimum.Width, desiredMinimum.Height);

		return MeasuredMinimum.Value;
	}

	protected override void OnAdjustSizeRequest(Orientation orientation, out int minimumSize, out int naturalSize)
	{
		base.OnAdjustSizeRequest(orientation, out minimumSize, out naturalSize);

		if (IsSizeAllocating)
		{
			return;
		}

		if (VirtualView is not { } virtualView)
			return;

		var measuredMinimum = MeasureMinimum();

		double constraint = minimumSize;

		if (orientation == Orientation.Horizontal)
		{
			if (RequestMode is SizeRequestMode.WidthForHeight or SizeRequestMode.ConstantSize)
			{
				if (MeasuredSizeV is { Width : > 0 } size && (constraint == 0))
					constraint = size.Width;

				constraint = constraint == 0 ? double.PositiveInfinity : constraint;
			}
			else
			{
				;
			}

			MeasuredSizeH = constraint != 0 ? Measure(constraint, double.PositiveInfinity) : measuredMinimum;

			constraint = MeasuredSizeH.Value.Width;

			minimumSize = (int)measuredMinimum.Width;
			naturalSize = (int)constraint;
		}

		if (orientation == Orientation.Vertical)
		{
			var widthContraint = double.PositiveInfinity;

			if (RequestMode is SizeRequestMode.HeightForWidth or SizeRequestMode.ConstantSize)
			{
				MeasuredSizeH ??= measuredMinimum;

				if (MeasuredSizeH is { } size && constraint == 0)
				{
					if (size.Height > 0)
						constraint = size.Height;

					if (size.Width > 0)
						widthContraint = size.Width;
				}

				constraint = constraint == 0 ? double.PositiveInfinity : constraint;
			}
			else
			{
				;
			}

			MeasuredSizeV = constraint != 0 ? Measure(widthContraint, constraint) : measuredMinimum;

			constraint = MeasuredSizeV.Value.Height;

			minimumSize = (int)measuredMinimum.Height;
			naturalSize = (int)constraint;
		}
	}

	const bool RestrictToMeasuredArrange = true;

	public void Arrange(Rectangle rect)
	{
		if (rect.IsEmpty)
			return;

		if (rect == Allocation.ToRect()) return;

		if (IsSizeAllocating)
		{
			SizeAllocate(rect.ToNative());

			return;
		}

		var measuredArrange = Measure(rect.Width, rect.Height);
		var alloc = new Rectangle(rect.Location, RestrictToMeasuredArrange ? measuredArrange : rect.Size);
		SizeAllocate(alloc.ToNative());
		QueueAllocate();
	}
}