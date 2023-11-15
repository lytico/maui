using Microsoft.Maui.Graphics;
using Microsoft.Maui.Graphics.Platform.Gtk;
using Microsoft.Maui.Platform;

namespace Gtk.UIExtensions.NUI;

public static class WidgetExtensions
{

	public static float PositionX(this Widget it) => it.Allocation.X;

	public static float PositionY(this Widget it) => it.Allocation.Y;

	public static float SizeWidth(this Widget it) => it.AllocatedWidth;

	public static float SizeHeight(this Widget it) => it.AllocatedHeight;

	public static Size Size(this Widget it) => new Size(it.Allocation.Width, it.Allocation.Height);

	public static void UpdateBounds(this Widget it, Rect bounds)
	{
		it.Arrange(bounds);
	}

	public static Rect GetBounds(this Widget it) => it.Allocation.ToRect();

	public static void UpdateSize(this Widget nativeView, Size size)
	{
		var widthRequest = Microsoft.Maui.WidgetExtensions.Request(size.Width);
		var doResize = false;

		if (widthRequest != -1 && widthRequest != nativeView.WidthRequest && widthRequest != nativeView.AllocatedWidth)
		{
			nativeView.WidthRequest = widthRequest;
			doResize = true;
		}

		var heightRequest = Microsoft.Maui.WidgetExtensions.Request(size.Height);

		if (heightRequest != -1 && heightRequest != nativeView.HeightRequest && heightRequest != nativeView.AllocatedHeight)
		{
			nativeView.HeightRequest = heightRequest;
			doResize = true;
		}

		if (doResize)
			nativeView.QueueResize();
	}

	[MissingMapper]
	public static void RaiseToTop(this Widget it)
	{ }

	public static void Add(this Widget it, Widget child)
	{
		if (child is Container c)
		{
			c.Add(child);
		}
	}

	public static int ToScaledPixel(this double it) => (int)it;
	public static int ToScaledDP(this double it) => (int)it;
	public static Size ToPixel(this Size it) => it;
	
	public static void WidthSpecification(this Widget it, LayoutParamPolicies p) { }
	public static void HeightSpecification(this Widget it, LayoutParamPolicies p) { }
	public static void WidthResizePolicy(this Widget it, ResizePolicyType p) { }
		
	public static void HeightResizePolicy(this Widget it, ResizePolicyType p) { }
	
	
}

/// <summary>
/// Layout policies to decide the size of View when the View is laid out in its parent View.
/// </summary>
/// <example>
/// <code>
/// // matchParentView matches its size to its parent size.
/// matchParentView.WidthSpecification (LayoutParamPolicies.MatchParent);
/// matchParentView.HeightSpecification (LayoutParamPolicies.MatchParent);
///
/// // wrapContentView wraps its children with their desired size.
/// wrapContentView.WidthSpecification (LayoutParamPolicies.WrapContent);
/// wrapContentView.HeightSpecification (LayoutParamPolicies.WrapContent);
/// </code>
/// </example>
/// <since_tizen> 9 </since_tizen>
public enum LayoutParamPolicies
{
	/// <summary>
	/// Constant which indicates child size should match parent size.
	/// </summary>
	/// <since_tizen> 9 </since_tizen>
	MatchParent,
	/// <summary>
	/// Constant which indicates parent should take the smallest size possible to wrap its children with their desired size.
	/// </summary>
	WrapContent
}

public enum ResizePolicyType
{
	FillToParent
}