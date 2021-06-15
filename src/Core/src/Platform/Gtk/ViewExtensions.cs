using System;
using Gtk;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Graphics.Native.Gtk;

namespace Microsoft.Maui
{

	public static class ViewExtensions
	{

		public static void UpdateAutomationId(this Widget nativeView, IView view)
		{ }

		[PortHandler("implement drawing of other paints than solidpaint")]
		public static void UpdateBackground(this Widget nativeView, IView view)
		{
			var bkColor = view.Background?.BackgroundColor;

			if (view.Background is SolidPaint solidPaint)
			{
				bkColor = solidPaint.Color;
			}

			if (bkColor == null)
				return;

			switch (nativeView)
			{
				case ProgressBar:
					nativeView.SetColor(bkColor, "background-color", "trough > progress");

					break;
				case ComboBox box:
					// no effect: box.SetColor(bkColor, "border-color");
					box.GetCellRendererText().SetBackground(bkColor);

					break;
				default:
					nativeView.SetBackgroundColor(bkColor);

					break;
			}

		}

		public static void UpdateIsEnabled(this Widget nativeView, IView view) =>
			nativeView?.UpdateIsEnabled(view.IsEnabled);

		public static void UpdateVisibility(this Widget nativeView, IView view) =>
			nativeView?.UpdateVisibility(view.Visibility);

		public static void UpdateSemantics(this Widget nativeView, IView view)
		{ }

		public static void UpdateOpacity(this Widget nativeView, IView view) { }

	}

}