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

	[MissingMapper]
	protected virtual void Decelerating(float velocity, Animation animation)
	{
	
	}
	
	public float DecelerationRate { get; set; }

	public int ScrollingEventThreshold { get; set; }

	public bool HideScrollbar { get; set; }

	public bool ScrollEnabled { get; set; }

	public event EventHandler<ScrollEventArgs>? ScrollAnimationEnded;

	public event EventHandler<ScrollEventArgs>? ScrollDragStarted;

	[MissingMapper]
	public Rect GetScrollBound()
	{
		return default;
	}

	[MissingMapper]
	public void ScrollTo(float itemBoundX, bool animate)
	{
	}

	public event EventHandler<ScrollEventArgs>? Scrolling;

	public event EventHandler<EventArgs>? Relayout;
	
	protected void OnLayout(object? sender, SizeAllocatedArgs e)
	{
		if (sender is not Widget w)
			return;
		
		Relayout?.Invoke(sender,new EventArgs());
	}
	
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

	[MissingMapper]
	public float SizeWidth()
	{
		return default;
	}
	
	[MissingMapper]
	public float SizeHeight()
	{
		return default;
	}

	[MissingMapper]
	public void WidthSpecification(LayoutParamPolicies matchParent)
	{
	}

	[MissingMapper]
	public void HeightSpecification(LayoutParamPolicies matchParent)
	{
	}

	[MissingMapper]
	public void WidthResizePolicy(ResizePolicyType fillToParent)
	{
	}

	[MissingMapper]
	public void HeightResizePolicy(ResizePolicyType fillToParent)
	{
	}
}