using System;
using System.Collections.Generic;
using System.Linq;
using Gdk;
using Gtk;
using Microsoft.Maui.Graphics.Native.Gtk;
using Microsoft.Maui.Layouts;
using Rectangle = Microsoft.Maui.Graphics.Rectangle;
using Size = Microsoft.Maui.Graphics.Size;

namespace Microsoft.Maui
{

	public class LayoutView : Gtk.Container
	{

#if DEBUG

		protected override bool OnDrawn(Cairo.Context cr)
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
		public Func<ILayout>? CrossPlatformVirtualView { get; set; }

		public ILayout? VirtualView => CrossPlatformVirtualView?.Invoke();

		public bool IsReallocating;
		Dictionary<IView, WidgetData> _children = new();

		struct WidgetData
		{

			public Graphics.Rectangle Rect;
			public Gtk.Widget Widget;

		}

		public LayoutView()
		{
			HasWindow = false;
		}

		public void ReplaceChild(Gtk.Widget oldWidget, Gtk.Widget newWidget)
		{
			var view = _children.FirstOrDefault(kvp => kvp.Value.Widget == oldWidget).Key;
			WidgetData r = default;	
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

		void UpdateFocusChain(Orientation orientation)
		{
			var focusChain = _children
			   .OrderBy(kvp => orientation == Orientation.Horizontal ? kvp.Value.Rect.X : kvp.Value.Rect.Y)
			   .Select(kvp => kvp.Value.Widget)
			   .ToArray();

			FocusChain = focusChain;
		}

		public bool SetAllocation(IView w, Graphics.Rectangle rect)
		{
			if (VirtualView == null)
			{
				return false;
			}

			_children.TryGetValue(w, out WidgetData r);

			if (r.Rect == rect) return false;

			r.Rect = rect;
			_children[w] = r;
			UpdateFocusChain(GetOrientation());

			return true;

		}

		public void Add(IView view, Gtk.Widget gw)
		{
			_children.Add(view, new WidgetData
			{
				Widget = gw,
				Rect = new(0, 0, 0, 0)
			});

			Add(gw);
		}

		protected override void OnAdded(Gtk.Widget widget)
		{
			widget.Parent = this;
		}

		protected override void OnRemoved(Gtk.Widget widget)
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

			VirtualView.InvalidateMeasure();
			VirtualView.InvalidateArrange();

			if (allocation == default)
			{
				allocation = new(0, 0, Allocation.Width, Allocation.Height);
			}

			// VirtualView.Arrange(allocation.ToRectangle());
		}

		protected Gtk.Requisition OnGetRequisition(SizeConstraint widthConstraint, SizeConstraint heightConstraint)
		{
			if (VirtualView == null)

			{
				return Gtk.Requisition.Zero;
			}

			var size = GetSizeRequest(widthConstraint, heightConstraint);

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
				cr.Value.Widget.SizeAllocate(new(allocation.X + (int)r.X, allocation.Y + (int)r.Y, (int)r.Width, (int)r.Height));
			}
		}

		protected override void ForAll(bool includeInternals, Gtk.Callback callback)
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

		protected override Gtk.SizeRequestMode OnGetRequestMode()
		{
			// dirty fix: unwrapped labels report fixed sizes, forcing parents to fixed mode
			//            -> report always width_for_height, since we don't support angles
			// return Gtk.SizeRequestMode.WidthForHeight;
			return base.OnGetRequestMode();
		}

		protected override void OnGetPreferredHeight(out int minimum_height, out int natural_height)
		{
			base.OnGetPreferredHeight(out minimum_height, out natural_height);
			// containers need initial width in heigt_for_width mode
			// dirty fix: do not constrain width on first allocation 
			var force_width = SizeConstraint.Unconstrained;

			if (IsReallocating)
				force_width = SizeConstraint.WithSize(Allocation.Width);

			var size = OnGetRequisition(force_width, SizeConstraint.Unconstrained);

			if (size.Height < HeightRequest)
				minimum_height = natural_height = HeightRequest;
			else
				minimum_height = natural_height = size.Height;
		}

		protected override void OnGetPreferredWidth(out int minimum_width, out int natural_width)
		{
			base.OnGetPreferredWidth(out minimum_width, out natural_width);
			// containers need initial height in width_for_height mode
			// dirty fix: do not constrain height on first allocation
			var force_height = SizeConstraint.Unconstrained;

			if (IsReallocating)
				force_height = SizeConstraint.WithSize(Allocation.Width);

			var size = OnGetRequisition(SizeConstraint.Unconstrained, force_height);

			if (size.Width < WidthRequest)
				minimum_width = natural_width = WidthRequest;
			else
				minimum_width = natural_width = size.Width;
		}

		protected override void OnGetPreferredHeightForWidth(int width, out int minimum_height, out int natural_height)
		{
			base.OnGetPreferredHeightForWidth(width, out minimum_height, out natural_height);
			var size = OnGetRequisition(SizeConstraint.WithSize(width), SizeConstraint.Unconstrained);

			if (size.Height < HeightRequest)
				minimum_height = natural_height = HeightRequest;
			else
				minimum_height = natural_height = size.Height;
		}

		protected override void OnGetPreferredWidthForHeight(int height, out int minimum_width, out int natural_width)
		{
			base.OnGetPreferredWidthForHeight(height, out minimum_width, out natural_width);
			var size = OnGetRequisition(SizeConstraint.Unconstrained, SizeConstraint.WithSize(height));

			if (size.Width < WidthRequest)
				minimum_width = natural_width = WidthRequest;
			else
				minimum_width = natural_width = size.Width;
		}

		#region adoptions

		public void SetFrame(Rectangle rect)
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

		public Size GetSizeRequest(SizeConstraint widthConstraint, SizeConstraint heightConstraint)
		{
			if (VirtualView == null)
			{
				return Size.Zero;
			}

			// if (IsReallocating)
			// {
			VirtualView.InvalidateMeasure();

			var size1 = VirtualView.Measure(widthConstraint.IsConstrained ? widthConstraint.AvailableSize : -1, heightConstraint.IsConstrained ? heightConstraint.AvailableSize : -1);

			var size = Graphics.Size.Zero;

			// return size.IsZero?;
			foreach (var v in VirtualView.Children)
			{
				if (v.IsMeasureValid)
				{
					size.Width = Math.Max(size.Width, v.DesiredSize.Width + v.Frame.X);
					size.Height = Math.Max(size.Height, v.DesiredSize.Height + v.Frame.Y);
				}

				v.InvalidateArrange();
				v.Arrange(v.Frame);
				SetAllocation(v, v.Frame);

			}

			var mesured = new Size(size.Width > 0 ? size.Width : widthConstraint.AvailableSize, size.Height > 0 ? size.Height : heightConstraint.AvailableSize);
			VirtualView.InvalidateMeasure();
			VirtualView.InvalidateArrange();
			VirtualView.Arrange(new(Graphics.Point.Zero, mesured));
			// }

			return mesured;

		}

		Orientation GetOrientation() => Orientation.Vertical;

		#endregion

	}

	public struct SizeConstraint : IEquatable<SizeConstraint>
	{

		// The value '0' is used for Unconstrained, since that's the default value of SizeContraint
		// Since a constraint of '0' is valid, we use NegativeInfinity as a marker for constraint=0.
		double value;

		public static readonly SizeConstraint Unconstrained = new SizeConstraint();

		static public implicit operator SizeConstraint(double size)
		{
			return new SizeConstraint() { AvailableSize = size };
		}

		public static SizeConstraint WithSize(double size)
		{
			return new SizeConstraint() { AvailableSize = size };
		}

		public double AvailableSize
		{
			get
			{
				if (double.IsNegativeInfinity(value))
					return 0;
				else
					return value;
			}
			set
			{
				this.value = value <= 0 ? double.NegativeInfinity : value;
			}
		}

		public bool IsConstrained
		{
			get { return value != 0; }
		}

		public static bool operator ==(SizeConstraint s1, SizeConstraint s2)
		{
			return (s1.value == s2.value);
		}

		public static bool operator !=(SizeConstraint s1, SizeConstraint s2)
		{
			return (s1.value != s2.value);
		}

		public static SizeConstraint operator +(SizeConstraint c, double s)
		{
			if (!c.IsConstrained)
				return c;
			else
				return SizeConstraint.WithSize(c.AvailableSize + s);
		}

		public static SizeConstraint operator -(SizeConstraint c, double s)
		{
			if (!c.IsConstrained)
				return c;
			else
				return SizeConstraint.WithSize(Math.Max(c.AvailableSize - s, 0));
		}

		public override bool Equals(object ob)
		{
			return (ob is SizeConstraint) && this == (SizeConstraint)ob;
		}

		public bool Equals(SizeConstraint other)
		{
			return this == other;
		}

		public override int GetHashCode()
		{
			return value.GetHashCode();
		}

		public override string ToString()
		{
			if (IsConstrained)
				return AvailableSize.ToString();
			else
				return "Unconstrained";
		}

	}

}