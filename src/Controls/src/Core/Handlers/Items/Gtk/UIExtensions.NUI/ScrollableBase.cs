using System;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

#pragma warning disable CS0067 // Event is never used

namespace Gtk.UIExtensions.NUI;

public class ScrollableBase
{

	public ScrollableBase(Gtk.Widget contentContainer)
	{
		ContentContainer = contentContainer;
	}

	public Direction ScrollingDirection { get; set; }

	public Gtk.Widget ContentContainer { get; set; }

	protected virtual void Decelerating(float velocity, Animation animation)
	{
		throw new System.NotImplementedException();
	}
	
	public float DecelerationRate { get; set; }

	public int ScrollingEventThreshold { get; set; }

	public bool HideScrollbar { get; set; }

	public bool ScrollEnabled { get; set; }

	public event EventHandler<ScrollEventArgs>? ScrollAnimationEnded;

	public event EventHandler<ScrollEventArgs>? ScrollDragStarted;

	public Rect GetScrollBound()
	{
		throw new NotImplementedException();
	}

	public void ScrollTo(float itemBoundX, bool animate)
	{
		throw new NotImplementedException();
	}

	public event EventHandler<ScrollEventArgs>? Scrolling;

	public event EventHandler<EventArgs>? Relayout;
	
	/// <summary>
	/// The direction axis to scroll.
	/// </summary>
	/// <since_tizen> 8 </since_tizen>
	public enum Direction
	{
		/// <summary>
		/// Horizontal axis.
		/// </summary>
		/// <since_tizen> 8 </since_tizen>
		Horizontal,

		/// <summary>
		/// Vertical axis.
		/// </summary>
		/// <since_tizen> 8 </since_tizen>
		Vertical
	}

	public event EventHandler<EventArgs>? ScrollDragEnded;

	public event EventHandler<EventArgs>? ScrollAnimationStarted;

	public float SizeWidth()
	{
		throw new NotImplementedException();
	}
	
	public float SizeHeight()
	{
		throw new NotImplementedException();
	}
}