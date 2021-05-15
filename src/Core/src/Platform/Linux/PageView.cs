using System;
using Gtk;
using Microsoft.Maui.Graphics;

namespace Microsoft.Maui
{

	public class PageView : Gtk.Box
	{

		public PageView() : base(Orientation.Horizontal, 0) { }

		internal Func<double, double, Size>? CrossPlatformMeasure { get; set; }

		internal Func<Rectangle, Size>? CrossPlatformArrange { get; set; }

		Widget? _content;

		public Widget? Content
		{
			get => _content;
			set
			{
				if (_content != null && value != null)
				{
					this.ReplaceChild(_content, value);

				}
				else if (value != null)
				{
					PackStart(value, true, true, 0);
				}

				_content = value;

			}
		}

		protected override void OnSizeAllocated(Gdk.Rectangle allocation)
		{
			base.OnSizeAllocated(allocation);
		}

		protected override void OnAdjustSizeRequest(Orientation orientation, out int minimum_size, out int natural_size)
		{
			base.OnAdjustSizeRequest(orientation, out minimum_size, out natural_size);

			if (CrossPlatformMeasure == null)
				return;

			if (orientation == Orientation.Horizontal) // widthRequest
			{
				var m = CrossPlatformMeasure(minimum_size, double.PositiveInfinity);
			}
			else
			{
				var m = CrossPlatformMeasure(double.PositiveInfinity,minimum_size);
			}
		}

	}

}