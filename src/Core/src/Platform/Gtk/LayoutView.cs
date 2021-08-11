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

		public bool RestrictToMesuredAllocation { get; set; } = true;

		public bool RestrictToMeasuredArrange { get; set; } = true;

		protected bool IsReallocating;

		protected bool IsSizeAllocating;

		protected Size? MeasuredArrange { get; set; }

		protected Size? MesuredAllocation { get; set; }

		Size? MeasuredSizeH { get; set; }

		Size? MeasuredSizeV { get; set; }

		Size? _minimumWidth = null;

		Size MinimumWidth
		{
			get
			{
				if (VirtualView is not { } virtualView)
					return Size.Zero;

				return _minimumWidth ??= new Size(virtualView.Max(c => c.DesiredSize.Width), virtualView.Sum(c => c.DesiredSize.Height));

			}

		}

		Size? MeasuredMinimum { get; set; }

		void ClearMeasured()
		{
			IsReallocating = false;
			IsSizeAllocating = false;
			MeasuredArrange = null;
			MeasuredSizeH = null;
			MeasuredSizeV = null;
			_minimumWidth = null;
			MeasuredMinimum = null;
			_measureCache = null!;
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
				ClearMeasured();
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

		Dictionary<(double width, double height, SizeRequestMode mode), SizeRequest> _measureCache = null!;

		public SizeRequest Measure(double widthConstraint, double heightConstraint, SizeRequestMode mode = SizeRequestMode.ConstantSize)
		{

			if (VirtualView is not { LayoutManager: { } layoutManager } virtualView)
				return Size.Zero;

			_measureCache ??= new();
			var key = (widthConstraint, heightConstraint, mode);

			if (_measureCache.TryGetValue(key, out var size1))
				return size1;

			size1 = layoutManager.Measure(widthConstraint, heightConstraint);
			sr++;

			var res = new SizeRequest(size1, size1);

			_measureCache[key] = res;

			return res;
		}

		int ToSize(double it) => double.IsPositiveInfinity(it) ? 0 : (int)it;

		void NegotiateMinimum()
		{
			if (MeasuredMinimum != null)
				return;

			Measure(0, double.PositiveInfinity);
			MeasuredMinimum = MeasuredSizeH = Measure(MinimumWidth.Width, double.PositiveInfinity);
		}

		protected override void OnAdjustSizeRequest(Orientation orientation, out int minimumSize, out int naturalSize)
		{
			base.OnAdjustSizeRequest(orientation, out minimumSize, out naturalSize);

			if (IsSizeAllocating)
			{
				return;
			}

			if (VirtualView is not { LayoutManager: { } layoutManager } virtualView)
				return;

			// if (MesuredAllocation.HasValue && RestrictToMesuredAllocation)
			// {
			// 	minimumSize = orientation == Orientation.Horizontal ? naturalSize = (int)MesuredAllocation.Value.Width : naturalSize = (int)MesuredAllocation.Value.Height;
			//
			// 	return;
			// }

			NegotiateMinimum();

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

				MeasuredSizeH = constraint != 0 ? Measure(constraint, double.PositiveInfinity) : MeasuredMinimum!;

				constraint = MeasuredSizeH.Value.Width;

				minimumSize = (int)MeasuredMinimum!.Value.Width;
				naturalSize = (int)constraint;
			}

			if (orientation == Orientation.Vertical)
			{
				var widthContraint = double.PositiveInfinity;

				if (RequestMode is SizeRequestMode.HeightForWidth or SizeRequestMode.ConstantSize)
				{
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

				MeasuredSizeV = constraint != 0 ? Measure(widthContraint, constraint) : MeasuredMinimum!;

				constraint = MeasuredSizeV.Value.Height;

				minimumSize = (int)MeasuredMinimum!.Value.Height;
				naturalSize = (int)constraint;

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