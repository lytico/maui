#nullable disable
namespace Microsoft.Maui.Controls
{
	public abstract class StackBase : Layout, IStackLayout
	{
		public static readonly BindableProperty SpacingProperty = BindableProperty.Create(nameof(Spacing), typeof(double), typeof(StackBase), 0d,
				propertyChanged: (bindable, oldvalue, newvalue) => ((IView)bindable).InvalidateMeasure());

		public double Spacing
		{
			get { return (double)GetValue(SpacingProperty); }
			set { SetValue(SpacingProperty, value); }
		}
#if __GTK__
		/// <include file="../../../docs/Microsoft.Maui.Controls/StackLayout.xml" path="//Member[@MemberName='OrientationProperty']/Docs/*" />
		public static readonly BindableProperty OrientationProperty = BindableProperty.Create(nameof(Orientation), typeof(StackOrientation), typeof(StackLayout), StackOrientation.Vertical,
			propertyChanged: OrientationChanged);

		/// <include file="../../../docs/Microsoft.Maui.Controls/StackLayout.xml" path="//Member[@MemberName='Orientation']/Docs/*" />
		public StackOrientation Orientation
		{
			get { return (StackOrientation)GetValue(OrientationProperty); }
			set { SetValue(OrientationProperty, value); }
		}

		static void OrientationChanged(BindableObject bindable, object oldValue, object newValue)
		{
			var layout = (StackLayout)bindable;
			layout.InvalidateMeasure();
		}
#endif
	}
}
