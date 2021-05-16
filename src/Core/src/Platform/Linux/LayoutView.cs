using System;
using System.Collections.Generic;
using System.Linq;
using Gtk;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Graphics.Native.Gtk;
using Microsoft.Maui.Layouts;
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

		public Size GetSizeRequest(double widthConstraint, double heightConstraint, SizeRequestMode mode)
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
				var widget = kvp.Value.Widget;
				var allocation = kvp.Value;
				var view = kvp.Key;

				if (heightConstraint == 1)
				{
					var fullsize = view.Measure(view.Margin.VerticalThickness, view.Margin.HorizontalThickness);
					var heigthForMinWidth = widget.GetDesiredSize(0, double.PositiveInfinity);
				}
				var sizeRequest = widget.GetDesiredSize(widthConstraint, heightConstraint);
				var measure = view.Measure(widthConstraint, heightConstraint);
				// after Measure, IView.DesiredSize == measured size == sizeRequest.Request
				
				var viewFrame = view.Frame;

				var minimumFrame = new Rectangle(viewFrame.Location, sizeRequest.Minimum);
				var desiredFrame = new Rectangle(viewFrame.Location, sizeRequest.Request);
				minSize.Width = Math.Max(minSize.Width, minimumFrame.Right);
				minSize.Height = Math.Max(minSize.Height, minimumFrame.Bottom);
				desiredSize.Width = Math.Max(desiredSize.Width, desiredFrame.Right);
				desiredSize.Height = Math.Max(desiredSize.Height, desiredFrame.Bottom);
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
			var size = GetSizeRequest(width, double.PositiveInfinity, SizeRequestMode.HeightForWidth);

			if (size.Height < HeightRequest)
				minimumHeight = naturalHeight = HeightRequest;
			else
				minimumHeight = naturalHeight = (int)size.Height;
		}

		protected override void OnGetPreferredWidthForHeight(int height, out int minimumWidth, out int naturalWidth)
		{
			base.OnGetPreferredWidthForHeight(height, out minimumWidth, out naturalWidth);
			var size = GetSizeRequest(double.PositiveInfinity, height, SizeRequestMode.WidthForHeight);

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

		#endregion

	}

}