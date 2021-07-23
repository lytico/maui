﻿using System;
using Microsoft.Maui.Graphics;

namespace Microsoft.Maui
{
	public interface IScrollView : IView
	{
		// TODO ezhart 2021-07-08 It might make sense for IPage and IScrollView to derive from (the not yet created) IContentView 

		/// <summary>
		/// Gets the view that contains the content of the ScrollView.
		/// </summary>
		public IView Content { get; }

		/// <summary>
		/// Gets a value indicating the visibility rules for the horizontal scroll bar.
		/// </summary>
		ScrollBarVisibility HorizontalScrollBarVisibility { get; }

		/// <summary>
		/// Gets a value indicating the visibility rules for the vertical scroll bar.
		/// </summary>
		ScrollBarVisibility VerticalScrollBarVisibility { get; }

		/// <summary>
		/// Gets a value indicating the scroll orientation of the ScrollView.
		/// </summary>
		ScrollOrientation Orientation { get; }

		/// <summary>
		/// Gets the size of the scrollable content in the ScrollView.
		/// </summary>
		Size ContentSize { get; }

		/// <summary>
		/// Gets the current scroll position of the ScrollView along the horizontal axis.
		/// </summary>
		double HorizontalOffset { get; set; }

		/// <summary>
		/// Gets the current scroll position of the ScrollView along the vertical axis.
		/// </summary>
		double VerticalOffset { get; set; }

		/// <summary>
		/// Allows the native ScrollView to inform that cross-platform code that a scroll operation has completed.
		/// </summary>
		void ScrollFinished();

		void RequestScrollTo(double horizontalOffset, double verticalOffset, bool instant);
	}
}