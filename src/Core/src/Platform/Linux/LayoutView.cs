using System;
using System.Collections.Generic;
using System.Linq;
using Gtk;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Graphics.Native.Gtk;
using Rectangle = Microsoft.Maui.Graphics.Rectangle;
using Size = Microsoft.Maui.Graphics.Size;

namespace Microsoft.Maui
{

	// refactored from: https://github.com/mono/xwt/blob/501f6b529fca632655295169094f637627c74c47/Xwt.Gtk/Xwt.GtkBackend/BoxBackend.cs

	public class LayoutView : Container, IGtkContainer
	{

		protected override bool OnDrawn(Cairo.Context cr)
		{
			var bk = this.GetBackgroundColor(this.StateFlags);

			if (bk != null)
			{
				cr.Save();
				cr.SetSourceColor(bk.ToCairoColor());
				cr.Rectangle(0, 0, Allocation.Width, Allocation.Height);

				cr.Fill();
				cr.Restore();
			}

			var r = base.OnDrawn(cr);
#if DEBUG

			cr.Save();
			cr.SetSourceColor(Graphics.Colors.Red.ToCairoColor());
			cr.Rectangle(0, 0, Allocation.Width, Allocation.Height);
			cr.Stroke();

			cr.Restore();

			return r;
		}
#endif

		public Func<ILayout>? CrossPlatformVirtualView { get; set; }

		public ILayout? VirtualView => CrossPlatformVirtualView?.Invoke();

		public bool IsReallocating;
		Dictionary<IView, ChildAllocation> _children = new();

		struct ChildAllocation
		{

			public Rectangle Rect;
			public Widget Widget;

		}

		public LayoutView()
		{
			HasWindow = false;
		}

		public void ReplaceChild(Widget oldWidget, Widget newWidget)
		{
			var view = _children.FirstOrDefault(kvp => kvp.Value.Widget == oldWidget).Key;
			ChildAllocation r = default;

			if (view != null)
			{
				r = _children[view];
			}

			Remove(oldWidget);
			Add(newWidget);

			if (view != null)
			{
				r.Widget = newWidget;
				_children[view] = r;
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
			   .Select(kvp => kvp.Value.Widget)
			   .ToArray();

			FocusChain = focusChain;
		}

		public bool SetAllocation(IView w, Rectangle rect)
		{
			if (VirtualView == null)
			{
				return false;
			}

			_children.TryGetValue(w, out ChildAllocation r);

			if (r.Rect == rect) return false;

			r.Rect = rect;
			_children[w] = r;
			UpdateFocusChain();

			return true;

		}

		public void Add(IView view, Widget gw)
		{
			_children.Add(view, new ChildAllocation
			{
				Widget = gw,
				Rect = new Rectangle(0, 0, 0, 0)
			});

			Add(gw);
		}

		protected override void OnAdded(Widget widget)
		{
			widget.Parent = this;
		}

		protected override void OnRemoved(Widget widget)
		{

			var view = _children.FirstOrDefault(kvp => kvp.Value.Widget == widget).Key;

			if (view != null)
				_children.Remove(view);

			widget.Unparent();
			QueueResize();
		}

		protected void OnReallocate(Gdk.Rectangle allocation = default)
		{
			if (VirtualView == null)
			{
				return;
			}

			if (allocation == default)
			{
				allocation = new Gdk.Rectangle(0, 0, Allocation.Width, Allocation.Height);
			}

			if (allocation.Size != Allocation.Size)
			{
				VirtualView.Arrange(allocation.ToRectangle());
			}

		}

		protected Requisition OnGetRequisition(SizeConstraint widthConstraint, SizeConstraint heightConstraint)
		{
			if (VirtualView == null)
			{
				return Requisition.Zero;
			}

			var size = GetSizeRequest0(widthConstraint, heightConstraint);

			return size.ToGtkRequisition();
		}

		protected override void OnSizeAllocated(Gdk.Rectangle allocation)
		{
			base.OnSizeAllocated(allocation);

			try
			{
				IsReallocating = true;
				OnReallocate(allocation);
			}
			finally
			{
				IsReallocating = false;
			}

			foreach (var cr in _children.ToArray())
			{
				var r = cr.Value.Rect;
				cr.Value.Widget.SizeAllocate(new Gdk.Rectangle(allocation.X + (int)r.X, allocation.Y + (int)r.Y, (int)r.Width, (int)r.Height));
			}
		}

		protected override void ForAll(bool includeInternals, Callback callback)
		{
			base.ForAll(includeInternals, callback);

			foreach (var c in _children.Values.Select(v => v.Widget).ToArray())
				callback(c);
		}

		public void QueueResizeIfRequired()
		{
			// since we have no SizeRequest event, we must always queue up for resize
			QueueResize();
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
					OnReallocate();
				}
				catch
				{
					IsReallocating = false;
				}
			}

			base.OnRealized();
		}

		protected override SizeRequestMode OnGetRequestMode()
		{
			// dirty fix: unwrapped labels report fixed sizes, forcing parents to fixed mode
			// -> report always width_for_height, since we don't support angles
			return SizeRequestMode.WidthForHeight;
			// return base.OnGetRequestMode();
		}

		public Size GetSizeRequest1(double widthConstraint, double heightConstraint, SizeRequestMode mode)
		{
			var virtualView = VirtualView;

			if (virtualView == null)
			{
				return Size.Zero;
			}

			var widthConstrained = !double.IsPositiveInfinity(widthConstraint);
			var heightConstrained = !double.IsPositiveInfinity(heightConstraint);
			var size1 = virtualView.Measure(widthConstraint, heightConstraint);

			var minSize = Size.Zero;
			var desiredSize = Size.Zero;

			foreach (var kvp in _children.ToArray())
			{
				var w = kvp.Value.Widget;
				var allocation = kvp.Value;
				var cv = kvp.Key;
				var s = w.GetDesiredSize(widthConstraint, heightConstraint);

				var ms = cv.Measure(widthConstraint, widthConstraint);
				var fr = cv.Frame;

				var minRe = new Rectangle(fr.Location, s.Minimum);
				var desRe = new Rectangle(fr.Location, s.Request);
				minSize.Width = Math.Max(minSize.Width, minRe.Right);
				minSize.Height = Math.Max(minSize.Height, minRe.Bottom);
				desiredSize.Width = Math.Max(desiredSize.Width, desRe.Right);
				desiredSize.Height = Math.Max(desiredSize.Height, desRe.Bottom);
				// SetAllocation(cv, fr);

			}

			var size2 = virtualView.Arrange(new(Point.Zero, minSize));

			foreach (var child in virtualView.Children)
			{
				SetAllocation(child, child.Frame);
			}

			return size2;
		}

		protected override void OnGetPreferredHeightForWidth(int width, out int minimumHeight, out int naturalHeight)
		{
			base.OnGetPreferredHeightForWidth(width, out minimumHeight, out naturalHeight);
			var size = GetSizeRequest1(width, double.PositiveInfinity, SizeRequestMode.HeightForWidth);

			if (size.Height < HeightRequest)
				minimumHeight = naturalHeight = HeightRequest;
			else
				minimumHeight = naturalHeight = (int)size.Height;
		}

		protected override void OnGetPreferredWidthForHeight(int height, out int minimumWidth, out int naturalWidth)
		{
			base.OnGetPreferredWidthForHeight(height, out minimumWidth, out naturalWidth);
			var size = GetSizeRequest1(double.PositiveInfinity, height, SizeRequestMode.WidthForHeight);

			if (size.Width < WidthRequest)
				minimumWidth = naturalWidth = WidthRequest;
			else
				minimumWidth = naturalWidth = (int)size.Width;
		}

		#region adoptions

		public void NativeArrange(Rectangle rect)
		{
			var nativeView = this;
			var virtualView = VirtualView;

			if (nativeView == null || virtualView == null)
				return;

			if (rect.Width < 0 || rect.Height < 0)
				return;

			if (rect != virtualView.Frame)
			{
				nativeView.SizeAllocate(rect.ToNative());
				nativeView.QueueResize();
			}
		}

		public Size GetSizeRequest0(SizeConstraint widthConstraint, SizeConstraint heightConstraint)
		{
			var virtualView = VirtualView;

			if (virtualView == null)
			{
				return Size.Zero;
			}

			var frame = new Rectangle(virtualView.Frame.X, virtualView.Frame.Y, virtualView.DesiredSize.Width, virtualView.DesiredSize.Height);

			if (frame != virtualView.Frame)
			{
				virtualView.InvalidateMeasure();

			}

			var size1 = virtualView.Measure(widthConstraint.IsConstrained ? widthConstraint.AvailableSize : double.PositiveInfinity, heightConstraint.IsConstrained ? heightConstraint.AvailableSize : double.PositiveInfinity);

			var size = Size.Zero;

			foreach (var child in virtualView.Children)
			{

				size.Width = Math.Max(size.Width, child.DesiredSize.Width + child.Frame.X);
				size.Height = Math.Max(size.Height, child.DesiredSize.Height + child.Frame.Y);
				var childFrame = new Rectangle(child.Frame.X, child.Frame.Y, child.DesiredSize.Width, child.DesiredSize.Height);

				var childSize = child.Measure(double.PositiveInfinity, double.PositiveInfinity);

				// if (childFrame != child.Frame)
				// {
				// 	child.InvalidateArrange();
				// 	child.Arrange(childFrame);
				// }
				// else
				// {
				// 	;
				// }

				SetAllocation(child, child.Frame);

			}

			size = size1;
			var mesured = new Size(size.Width > 0 ? size.Width : widthConstraint.AvailableSize, size.Height > 0 ? size.Height : heightConstraint.AvailableSize);

			if (frame.Size != mesured)
			{
				virtualView.InvalidateMeasure();
				virtualView.InvalidateArrange();
				virtualView.Arrange(new Rectangle(Graphics.Point.Zero, mesured));
			}

			return mesured;

		}

		#endregion

	}

	public struct SizeConstraint : IEquatable<SizeConstraint>
	{

		// The value '0' is used for Unconstrained, since that's the default value of SizeContraint
		// Since a constraint of '0' is valid, we use NegativeInfinity as a marker for constraint=0.
		double _value;

		public static readonly SizeConstraint Unconstrained = new();

		public static implicit operator SizeConstraint(double size) => new() { AvailableSize = size };

		public static SizeConstraint WithSize(double size) => new() { AvailableSize = size };

		public double AvailableSize
		{
			get => double.IsNegativeInfinity(_value) ? 0 : _value;
			set
			{
				_value = value <= 0 ? double.NegativeInfinity : value;
			}
		}

		public bool IsConstrained
		{
			get => _value != 0;
		}

		public static bool operator ==(SizeConstraint s1, SizeConstraint s2) => (s1._value == s2._value);

		public static bool operator !=(SizeConstraint s1, SizeConstraint s2) => (s1._value != s2._value);

		public static SizeConstraint operator +(SizeConstraint c, double s) => !c.IsConstrained ? c : WithSize(c.AvailableSize + s);

		public static SizeConstraint operator -(SizeConstraint c, double s) => !c.IsConstrained ? c : WithSize(Math.Max(c.AvailableSize - s, 0));

		public override bool Equals(object? ob) => (ob is SizeConstraint constraint) && this == constraint;

		public bool Equals(SizeConstraint other) => this == other;

		public override int GetHashCode() => _value.GetHashCode();

		public override string ToString() => IsConstrained ? AvailableSize.ToString() : "Unconstrained";

	}

}