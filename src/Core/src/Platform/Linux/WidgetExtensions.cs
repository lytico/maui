using System;
using Gtk;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Graphics.Native.Gtk;

namespace Microsoft.Maui
{

	public static class WidgetExtensions
	{

		public static void UpdateIsEnabled(this Widget nativeView, bool isEnabled) =>
			nativeView.Sensitive = isEnabled;

		public static SizeRequest GetDesiredSize(
			this Widget? nativeView,
			double widthConstraint,
			double heightConstraint)
		{
			if (nativeView == null)
				return Graphics.Size.Zero;

			var widthConstrained = !double.IsPositiveInfinity(widthConstraint);
			var heightConstrained = !double.IsPositiveInfinity(heightConstraint);

			// https://developer.gnome.org/gtk3/stable/GtkWidget.html#gtk-widget-get-preferred-size
			nativeView.GetPreferredSize(out var minimumSize, out var req);

			var desW = widthConstrained ? (int)widthConstraint : -1;

			if (widthConstrained && desW > 0)
			{
				// https://developer.gnome.org/gtk3/stable/GtkWidget.html#gtk-widget-get-preferred-height-and-baseline-for-width
				nativeView.GetPreferredHeightAndBaselineForWidth(desW, out var minimumHeightB, out var naturalHeightB, out var minimumBaseline, out var naturalBaseline);
			}

			var desiredSize = new Gdk.Size(
				req.Width > 0 ? req.Width : 0,
				req.Height > 0 ? req.Height : 0);

			var widthFits = widthConstraint >= desiredSize.Width;
			var heightFits = heightConstraint >= desiredSize.Height;

			if (widthFits && heightFits) // Enough space with given constraints
			{
				return new SizeRequest(new Graphics.Size(desiredSize.Width, desiredSize.Height));
			}

			if (!widthFits)
			{
				nativeView.SetSize((int)widthConstraint, -1);

				nativeView.GetPreferredSize(out minimumSize, out req);

				desiredSize = new Gdk.Size(
					req.Width > 0 ? req.Width : 0,
					req.Height > 0 ? req.Height : 0);

				heightFits = heightConstraint >= desiredSize.Height;
			}

			heightFits = heightFits || heightConstraint == 0;
			var size = new Graphics.Size(desiredSize.Width, heightFits ? desiredSize.Height : (int)heightConstraint);

			return new SizeRequest(size);

		}

		public static void SetSize(this Gtk.Widget nativeView, double width, double height)
		{
			int calcWidth = (int)Math.Round(width);
			int calcHeight = (int)Math.Round(height);

			// Avoid negative values
			if (calcWidth < -1)
			{
				calcWidth = -1;
			}

			if (calcHeight < -1)
			{
				calcHeight = -1;
			}

			if (calcWidth != nativeView.WidthRequest || calcHeight != nativeView.HeightRequest)
			{
				nativeView.SetSizeRequest(calcWidth, calcHeight);
			}
		}

		public static void Arrange(this Widget? nativeView, Rectangle rect)
		{
			if (nativeView == null)
				return;

			if (rect.IsEmpty)
				return;

			if (rect != nativeView.Allocation.ToRectangle())
			{
				nativeView.SizeAllocate(rect.ToNative());
				nativeView.QueueResize();
			}
		}

		public static void InvalidateMeasure(this Widget nativeView, IView view)
		{
			;
		}

		static int Request(double viewSize) => viewSize >= 0 ? (int)viewSize : -1;

		public static void UpdateWidth(this Widget nativeView, IView view)
		{
			var widthRequest = Request(view.Width);

			if (widthRequest != -1 && widthRequest != nativeView.WidthRequest && widthRequest != nativeView.AllocatedWidth)
			{
				nativeView.WidthRequest = widthRequest;
			}

		}

		public static void UpdateHeight(this Widget nativeView, IView view)
		{
			var heightRequest = Request(view.Height);

			if (heightRequest != -1 && heightRequest != nativeView.HeightRequest && heightRequest != nativeView.AllocatedHeight)
			{
				nativeView.HeightRequest = heightRequest;
			}

		}

		public static void UpdateFont(this Widget nativeView, ITextStyle textStyle, IFontManager fontManager)
		{
			var font = textStyle.Font;

			var fontFamily = fontManager.GetFontFamily(font);
#pragma warning disable 612
			nativeView.ModifyFont(fontFamily);
#pragma warning restore 612

		}

		public static void ReplaceChild(this Gtk.Container cont, Gtk.Widget oldWidget, Gtk.Widget newWidget)
		{
			if (oldWidget.Parent != cont)
				return;

			switch (cont)
			{
				case IGtkContainer container:
					container.ReplaceChild(oldWidget, newWidget);

					break;
				case Gtk.Notebook notebook:
				{
					Gtk.Notebook.NotebookChild nc = (Gtk.Notebook.NotebookChild)notebook[oldWidget];
					var detachable = nc.Detachable;
					var pos = nc.Position;
					var reorderable = nc.Reorderable;
					var tabExpand = nc.TabExpand;
					var tabFill = nc.TabFill;
					var label = notebook.GetTabLabel(oldWidget);
					notebook.Remove(oldWidget);
					notebook.InsertPage(newWidget, label, pos);

					nc = (Gtk.Notebook.NotebookChild)notebook[newWidget];
					nc.Detachable = detachable;
					nc.Reorderable = reorderable;
					nc.TabExpand = tabExpand;
					nc.TabFill = tabFill;

					break;
				}
				case Gtk.Paned paned:
				{
					var pc = (Gtk.Paned.PanedChild)paned[oldWidget];
					var resize = pc.Resize;
					var shrink = pc.Shrink;
					var pos = paned.Position;

					if (paned.Child1 == oldWidget)
					{
						paned.Remove(oldWidget);
						paned.Pack1(newWidget, resize, shrink);
					}
					else
					{
						paned.Remove(oldWidget);
						paned.Pack2(newWidget, resize, shrink);
					}

					paned.Position = pos;

					break;
				}
				case Gtk.Bin bin:
					bin.Remove(oldWidget);
					bin.Child = newWidget;

					break;
			}
		}

	}

}