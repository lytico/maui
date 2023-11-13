﻿#nullable disable

namespace Microsoft.Maui.Controls.Handlers.Items
{

	internal class PositionalSmoothScroller : LinearSmoothScroller
	{

		readonly ScrollToPosition _scrollToPosition;

		public PositionalSmoothScroller(ScrollToPosition scrollToPosition) : base()
		{
			_scrollToPosition = scrollToPosition;
		}

		protected override int VerticalSnapPreference => SnapPreference;

		protected override int HorizontalSnapPreference => SnapPreference;

		public override int CalculateDtToFit(int viewStart, int viewEnd, int boxStart, int boxEnd, int snapPreference)
		{
			if (snapPreference == SnapToAny && _scrollToPosition == ScrollToPosition.Center)
			{
				// The only option that LinearSmoothScroller doesn't have built in is Center	
				return (boxStart + (boxEnd - boxStart) / 2) - (viewStart + (viewEnd - viewStart) / 2);
			}

			// For the other options (Start and End), we can just use the built-in logic			
			return base.CalculateDtToFit(viewStart, viewEnd, boxStart, boxEnd, snapPreference);
		}

		int SnapPreference
		{
			get
			{
				switch (_scrollToPosition)
				{
					case ScrollToPosition.Start:
						return SnapToStart;
					case ScrollToPosition.End:
						return SnapToEnd;
					case ScrollToPosition.Center:
					case ScrollToPosition.MakeVisible:
					default:
						return SnapToAny;
				}
			}
		}

	}

	class LinearSmoothScroller
	{

		public int SnapToAny { get; set; }

		public int SnapToEnd { get; set; }

		public int SnapToStart { get; set; }

		protected virtual int VerticalSnapPreference { get; }

		protected virtual int HorizontalSnapPreference { get;  }

		public virtual int CalculateDtToFit(int viewStart, int viewEnd, int boxStart, int boxEnd, int snapPreference)
		{
			throw new System.NotImplementedException();
		}

	}

}