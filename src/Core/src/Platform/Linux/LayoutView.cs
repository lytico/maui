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

		bool isWithAllocated = false;
		int withAllocate = -1;
		bool isHeigthAllocated = false;
		int heigthAllocate = -1;

		protected override void OnAdjustSizeAllocation(Orientation orientation, out int minimumSize, out int naturalSize, out int allocatedPos, out int allocatedSize)
		{
			base.OnAdjustSizeAllocation(orientation, out minimumSize, out naturalSize, out allocatedPos, out allocatedSize);
			isWithAllocated = orientation == Orientation.Horizontal && allocatedSize > 1;
			isHeigthAllocated = orientation == Orientation.Vertical && allocatedSize > 1;

			if (isWithAllocated)
				withAllocate = allocatedSize;

			if (isHeigthAllocated)
				heigthAllocate = allocatedSize;
		}

		// protected override void OnAdjustSizeRequest(Orientation orientation, out int minimumSize, out int naturalSize)
		// {
		// 	base.OnAdjustSizeRequest(orientation, out minimumSize, out naturalSize);
		// }

		public SizeRequest GetSizeRequest(double widthConstraint, double heightConstraint, SizeRequestMode mode)
		{
			var widthHandled = AllocatedWidth > 1; // && virtualView.DesiredSize.Width > 0;
			var heightHandled = AllocatedHeight > 1; // && virtualView.DesiredSize.Height > 0;
			var widthConstrained = !double.IsPositiveInfinity(widthConstraint);
			var heightConstrained = !double.IsPositiveInfinity(heightConstraint);

			if (!widthHandled || !heightHandled)
			{
				return new Size(widthConstraint, heightConstraint);
			}

			var virtualView = VirtualView;

			if (virtualView == null)
			{
				return Size.Zero;
			}

			var withFactor = widthHandled && widthConstrained && widthConstraint > 1 ? widthConstraint / AllocatedWidth : 1;
			var heigthFactor = heightHandled && heightConstrained && heightConstraint > 1 ? heightConstraint / AllocatedHeight : 1;

			var size1 = virtualView.Measure(widthConstraint, heightConstraint);

			if (false)
#pragma warning disable 162
			{
				var minimumSize = Size.Zero;
				var desiredSize = Size.Zero;

				foreach (var kvp in _children.ToArray())
				{
					var widget = kvp.Value.Widget;
					var allocation = kvp.Value;
					var view = kvp.Key;

					if (view.Handler == null)
						continue;

					var minWidth = 0d;
					var minHeight = 0d;
					var desiredWidth = 0d;
					var desiredHeight = 0d;
					var viewFrame = view.Frame;

					if (mode == SizeRequestMode.WidthForHeight)
					{
						// initial width request for widget:
						if (allocation.Rect.Width == 0)
						{
							//var fullsize = view.Measure(view.Margin.HorizontalThickness, view.Margin.VerticalThickness);
							var mea = view.Measure(AllocatedWidth + view.Margin.HorizontalThickness, double.PositiveInfinity);
							minWidth = mea.Width;
							desiredHeight = mea.Height;
							var s = view.Handler.GetDesiredSize(double.PositiveInfinity, double.PositiveInfinity);
							desiredWidth = s.Width;
							minHeight = s.Height;
						}
						else
						{
							var wfh = view.Measure(view.Margin.HorizontalThickness, allocation.Rect.Height * heigthFactor);
							desiredHeight = wfh.Height;
							desiredWidth = wfh.Width;
							var s = view.Handler.GetDesiredSize(view.Margin.HorizontalThickness, view.Margin.VerticalThickness);
							minHeight = s.Height;
							minWidth = s.Width;

						}
					}
					else
					{
						// initial heigh request for widget:
						if (allocation.Rect.Height == 0)
						{
							var mea = view.Measure(widthConstraint + view.Margin.VerticalThickness, double.PositiveInfinity);
							desiredWidth = mea.Width;
							minHeight = mea.Height;
							var s = view.Handler.GetDesiredSize(widthConstraint, double.PositiveInfinity);
							minWidth = s.Width;
							desiredHeight = s.Height;
						}
						else
						{
							var hfw = view.Measure(allocation.Rect.Width * withFactor, view.Margin.VerticalThickness);
							desiredHeight = hfw.Height;
							desiredWidth = hfw.Width;
							var s = view.Handler.GetDesiredSize(view.Margin.HorizontalThickness, view.Margin.VerticalThickness);
							minHeight = s.Height;
							minWidth = s.Width;

						}
					}

					viewFrame = view.Frame;

					minimumSize.Width = Math.Max(minimumSize.Width, viewFrame.X + minWidth);
					minimumSize.Height = Math.Max(minimumSize.Height, viewFrame.Y + minHeight);
					desiredSize.Width = Math.Max(Math.Max(desiredSize.Width, viewFrame.X + desiredWidth), minimumSize.Width);
					desiredSize.Height = Math.Max(Math.Max(desiredSize.Height, viewFrame.Y + desiredHeight), minimumSize.Height);
					// SetAllocation(cv, fr);

				}

				var size2 = virtualView.Arrange(new(Point.Zero, minimumSize));
			}
#pragma warning restore 162

			var size2_ = virtualView.Arrange(new(Point.Zero, size1));

			foreach (var child in virtualView.Children)
			{
				SetAllocation(child, child.Frame);
			}

			return new SizeRequest(size1, size1);
		}

		int ToSize(double it) => double.IsPositiveInfinity(it) ? 0 : (int)it;

		protected override void OnGetPreferredWidthForHeight(int height, out int minimumWidth, out int naturalWidth)
		{
			base.OnGetPreferredWidthForHeight(height, out minimumWidth, out naturalWidth);
			var sizeRequest = GetSizeRequest(double.PositiveInfinity, height, SizeRequestMode.WidthForHeight);

			minimumWidth = Math.Max(WidthRequest, ToSize(sizeRequest.Minimum.Width));
			naturalWidth = Math.Max(WidthRequest, ToSize(sizeRequest.Request.Width));
		}

		protected override void OnGetPreferredHeightForWidth(int width, out int minimumHeight, out int naturalHeight)
		{
			base.OnGetPreferredHeightForWidth(width, out minimumHeight, out naturalHeight);
			var sizeRequest = GetSizeRequest(width, double.PositiveInfinity, SizeRequestMode.HeightForWidth);

			minimumHeight = Math.Max(HeightRequest, ToSize(sizeRequest.Minimum.Height));
			naturalHeight = Math.Max(HeightRequest, ToSize(sizeRequest.Request.Height));
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