using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Maui.Controls.Internals;
using Microsoft.Maui.Graphics;

namespace Microsoft.Maui.Controls
{
	// TODO: We don't currently have any concept of a page in Maui
	// so this just treats it as a layout for now
	public partial class ContentPage : Microsoft.Maui.ILayout
	{
		IReadOnlyList<Microsoft.Maui.IView> Microsoft.Maui.ILayout.Children =>
			new List<IView>() { Content };

		ILayoutHandler Maui.ILayout.LayoutHandler => Handler as ILayoutHandler;

		Thickness Maui.IView.Margin => new Thickness();

		public Primitives.LayoutAlignment HorizontalLayoutAlignment => Primitives.LayoutAlignment.Fill;

		void Maui.ILayout.Add(IView child)
		{
			Content = (View)child;
		}

		void Maui.ILayout.Remove(IView child)
		{
			Content = null;
		}
		protected override Size MeasureOverride(double widthConstraint, double heightConstraint)
		{
			if (Content is IFrameworkElement frameworkElement)
			{
				frameworkElement.Measure(widthConstraint, heightConstraint);
			}

			return new Size(widthConstraint, heightConstraint);
		}

		protected override void ArrangeOverride(Rectangle bounds)
		{
			// Update the Bounds (Frame) for this page
			Layout(bounds);

			if (Content is IFrameworkElement element)
			{
				element.Arrange(bounds);
				element.Handler?.SetFrame(element.Frame);
			}

			// return Frame.Size;
		}

		protected override void InvalidateMeasureOverride()
		{
			base.InvalidateMeasureOverride();
			if (Content is IFrameworkElement frameworkElement)
			{
				frameworkElement.InvalidateMeasure();
			}
		}

	}
}
