using System;
using System.Collections.Generic;
using System.Linq;
using Gtk;
using Microsoft.Maui.Graphics.Native.Gtk;
using Rectangle = Microsoft.Maui.Graphics.Rectangle;
using Size = Microsoft.Maui.Graphics.Size;
using Point = Microsoft.Maui.Graphics.Point;

#pragma warning disable 162

namespace Microsoft.Maui.Native
{

	// refactored from: https://github.com/mono/xwt/blob/501f6b529fca632655295169094f637627c74c47/Xwt.Gtk/Xwt.GtkBackend/BoxBackend.cs

	public class LayoutView : Container, IGtkContainer
	{

		protected override bool OnDrawn(Cairo.Context cr)
		{
			var stc = this.StyleContext;
			stc.RenderBackground(cr, 0, 0, Allocation.Width, Allocation.Height);

			var r = base.OnDrawn(cr);
#if DEBUG

			cr.Save();
			cr.SetSourceColor(Graphics.Colors.Red.ToCairoColor());
			cr.Rectangle(0, 0, Allocation.Width, Allocation.Height);
			cr.Stroke();

			cr.MoveTo(0, Allocation.Height - 12);
			cr.ShowText($"{sr} | {Allocation.Size} | {MeasuredArrange}");
			cr.Restore();

			return r;
		}
#endif

		public Func<ILayout>? CrossPlatformVirtualView { get; set; }

		public ILayout? VirtualView => CrossPlatformVirtualView?.Invoke();

		protected bool IsReallocating;

		protected bool IsSizeAllocating;

		protected Size? MeasuredArrange { get; set; }

		protected Size? MesuredAllocation { get; set; }

		public bool RestrictToMesuredAllocation { get; set; } = true;

		public bool RestrictToMeasuredArrange { get; set; } = true;

		Dictionary<IView, Widget> _children = new();

		public LayoutView()
		{
			HasWindow = false;
		}

		public void ReplaceChild(Widget oldWidget, Widget newWidget)
		{
			var view = _children.FirstOrDefault(kvp => kvp.Value == oldWidget).Key;

			Remove(oldWidget);
			Add(newWidget);

			if (view != null)
			{
				_children[view] = newWidget;
			}
		}

		// this is maybe not needed:
		void UpdateFocusChain()
		{
			Orientation GetOrientation() =>
				// TODO: find out what orientation it has, or find another sort kriteria, eg. tabstop
				Orientation.Vertical;

			var orientation = GetOrientation();

			var focusChain = _children
				// .OrderBy(kvp => orientation == Orientation.Horizontal ? kvp.Value.Rect.X : kvp.Value.Rect.Y)
			   .Values
			   .ToArray();

			FocusChain = focusChain;
		}

		protected override void ForAll(bool includeInternals, Callback callback)
		{
			base.ForAll(includeInternals, callback);

			foreach (var c in _children.Values.ToArray())
				callback(c);
		}

		public void ClearChildren()
		{
			foreach (var c in Children)
			{
				Remove(c);
			}

			_children.Clear();
		}

		public void Add(IView view, Widget gw)
		{
			if (_children.ContainsKey(view))
				return;

			_children[view] = gw;

			Add(gw);
		}

		protected override void OnAdded(Widget widget)
		{
			widget.Parent = this;
		}

		protected override void OnRemoved(Widget widget)
		{

			var view = _children.FirstOrDefault(kvp => kvp.Value == widget).Key;

			if (view != null)
				_children.Remove(view);

			widget.Unparent();
			QueueResize();
		}

		protected void AllocateChildren(Rectangle allocation)
		{

			foreach (var cr in _children.ToArray())
			{
				var w = cr.Value;
				var v = cr.Key;
				var r = v.Frame;

				if (r.IsEmpty)
					continue;

				var cAlloc = new Gdk.Rectangle((int)(allocation.X + r.X), (int)(allocation.Y + r.Y), (int)r.Width, (int)r.Height);

				// if (cAlloc != w.Allocation) // it's allways needed to implicit arrange children:
				w.SizeAllocate(cAlloc);
			}
		}

		protected void ArrangeAllocation(Rectangle allocation)
		{
			if (VirtualView is not { LayoutManager: { } layoutManager } virtualView)
				return;

			layoutManager.ArrangeChildren(allocation);

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

			try
			{
				IsReallocating = true;

				MesuredAllocation = MeasuredArrange ??= Measure(allocation.Width, allocation.Height, SizeRequestMode.ConstantSize);

				var mAllocation = allocation.ToRectangle();

				if (RestrictToMesuredAllocation)
					mAllocation.Size = MesuredAllocation.Value;

				ArrangeAllocation(new Rectangle(Point.Zero, mAllocation.Size));
				AllocateChildren(mAllocation);
				IsSizeAllocating = true;

				if (virtualView.Frame != mAllocation)
				{
					IsSizeAllocating = true;

					// TODO: virtualview Frame has wrong size?
					Arrange(mAllocation);
				}

				base.OnSizeAllocated(allocation);

			}
			finally
			{
				IsReallocating = false;
				IsSizeAllocating = false;
				MeasuredArrange = null;
				MeasuredSizeH = null;
				MeasuredSizeV = null;
			}

		}

		protected override void OnUnrealized()
		{
			// force reallocation on next realization, since allocation may be lost
			IsReallocating = false;
			base.OnUnrealized();
		}

		protected override void OnRealized()
		{
			// force reallocation, if unrealized previously
			if (!IsReallocating)
			{
				try
				{
					MesuredAllocation ??= Measure(Allocation.Width, Allocation.Height, SizeRequestMode.ConstantSize);
				}
				catch
				{
					IsReallocating = false;
				}
			}

			base.OnRealized();
		}

		int sr = 0;

		public SizeRequest Measure(double widthConstraint, double heightConstraint, SizeRequestMode mode = SizeRequestMode.ConstantSize)
		{

			if (VirtualView is not { LayoutManager: { } layoutManager } virtualView)
				return Size.Zero;

			var size1 = layoutManager.Measure(widthConstraint, heightConstraint);
			sr++;

			return new SizeRequest(size1, size1);
		}

		int ToSize(double it) => double.IsPositiveInfinity(it) ? 0 : (int)it;

		Size? MeasuredSizeH { get; set; }

		Size? MeasuredSizeV { get; set; }

		protected override void OnGetPreferredWidthForHeight(int height, out int minimumWidth, out int naturalWidth)
		{
			base.OnGetPreferredWidthForHeight(height, out minimumWidth, out naturalWidth);
		}

		protected override void OnGetPreferredHeightForWidth(int width, out int minimumHeight, out int naturalHeight)
		{
			base.OnGetPreferredHeightForWidth(width, out minimumHeight, out naturalHeight);
		}

		protected override void OnAdjustSizeRequest(Orientation orientation, out int minimumSize, out int naturalSize)
		{
			base.OnAdjustSizeRequest(orientation, out minimumSize, out naturalSize);

			// if (MeasuredArrange.HasValue)
			// {
			// 	minimumSize = orientation == Orientation.Horizontal ? naturalSize = (int)MeasuredArrange.Value.Width : naturalSize = (int)MeasuredArrange.Value.Height;
			// }

			double constraint = minimumSize;

			if (orientation == Orientation.Horizontal)
			{
				if (RequestMode is SizeRequestMode.WidthForHeight or SizeRequestMode.ConstantSize)
				{
					if (MeasuredSizeV is { Width : > 0 } size)
						constraint = size.Width;

					constraint = constraint == 0 ? double.PositiveInfinity : constraint;
				}
				else
				{
					;
				}

				MeasuredSizeH = Measure(constraint, double.PositiveInfinity);
				minimumSize = naturalSize = (int)MeasuredSizeH.Value.Width;
			}

			if (orientation == Orientation.Vertical)
			{
				if (RequestMode is SizeRequestMode.HeightForWidth or SizeRequestMode.ConstantSize)
				{
					if (MeasuredSizeH is { Height: > 0 } size)
						constraint = size.Height;

					constraint = constraint == 0 ? double.PositiveInfinity : constraint;

				}
				else
				{
					;
				}

				MeasuredSizeV = Measure(double.PositiveInfinity, constraint);
				minimumSize = naturalSize = (int)MeasuredSizeV.Value.Height;
			}
		}

		public void Arrange(Rectangle rect)
		{

			if (rect.IsEmpty)
				return;

			if (IsSizeAllocating)
			{
				MeasuredArrange = rect.Size;
				SizeAllocate(rect.ToNative());

				return;
			}

			if (rect != Allocation.ToRectangle() || MeasuredArrange == null)
			{
				MeasuredArrange = Measure(rect.Width, rect.Height, SizeRequestMode.ConstantSize);
				var alloc = new Rectangle(rect.Location, RestrictToMeasuredArrange ? MeasuredArrange.Value : rect.Size);
				SizeAllocate(alloc.ToNative());
				QueueAllocate();
			}
		}

	}

}