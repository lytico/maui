using System;
using Gtk;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Graphics.Platform.Gtk;
using Microsoft.Maui.Platform;

namespace Microsoft.Maui.Handlers
{

	public partial class LabelHandler : ViewHandler<ILabel, LabelView>
	{

		// https://docs.gtk.org/gtk3/class.Label.html
		protected override LabelView CreatePlatformView()
		{
			return new()
			{
				LineWrap = true,
				Halign = Align.Fill,
				Xalign = 0,
			};
		}

		private SizeRequest _perfectSize;

		private SizeRequest GetPerfectSize(LabelView platformView, int widthConstraint = -1)
		{

			int w, h;
			platformView.Layout.Width = Pango.Units.FromPixels(widthConstraint);
			platformView.Layout.GetPixelSize(out w, out h);

			return new SizeRequest(new Size(w, h));
		}

		public Size _GetDesiredSize(double widthConstraint, double heightConstraint)
		{
			if (PlatformView is not { } platformView)
				return default;

			if (VirtualView is not { } virtualView)
				return default;

			if (!platformView._perfectSizeValid)
			{
				_perfectSize = GetPerfectSize(platformView);
				_perfectSize.Minimum = new Size(Math.Min(10, _perfectSize.Request.Width), _perfectSize.Request.Height);
				platformView._perfectSizeValid = true;
			}

			var widthFits = widthConstraint >= _perfectSize.Request.Width;
			var heightFits = heightConstraint >= _perfectSize.Request.Height;

			if (widthFits && heightFits)
				return _perfectSize;

			var result = GetPerfectSize(platformView, (int)widthConstraint);
			var tinyWidth = Math.Min(10, result.Request.Width);
			result.Minimum = new Size(tinyWidth, result.Request.Height);

			if (widthFits || platformView.LineBreakMode == LineBreakMode.NoWrap)
			{
				return new SizeRequest(
					new Size(result.Request.Width, _perfectSize.Request.Height),
					new Size(result.Minimum.Width, _perfectSize.Request.Height));
			}

			bool containerIsNotInfinitelyWide = !double.IsInfinity(widthConstraint);

			if (containerIsNotInfinitelyWide)
			{
				bool textCouldHaveWrapped = platformView.LineBreakMode == LineBreakMode.WordWrap || platformView.LineBreakMode == LineBreakMode.CharacterWrap;
				bool textExceedsContainer = result.Request.Width > widthConstraint;

				if (textExceedsContainer || textCouldHaveWrapped)
				{
					var expandedWidth = Math.Max(tinyWidth, widthConstraint);
					result.Request = new Size(expandedWidth, result.Request.Height);
				}
			}

			return result;
		}

		public override Size GetDesiredSize(double widthConstraint, double heightConstraint)
		{
			if (PlatformView is not { } platformView)
				return default;

			if (VirtualView is not { } virtualView)
				return default;

			double explicitWidth = (virtualView.Width >= 0) ? virtualView.Width : widthConstraint;
			double explicitHeight = (virtualView.Height >= 0) ? virtualView.Height : heightConstraint;

			var widthConstrained = !double.IsPositiveInfinity(explicitWidth);
			var heightConstrained = !double.IsPositiveInfinity(explicitHeight);
			var size = platformView.GetDesiredSize(explicitWidth, explicitHeight);

			platformView.GetPreferredSize(out var min, out var nat);

			if (!widthConstrained && !heightConstrained)
			{

				return new Size(min.Width, min.Height);
			}

			return size;
		}

		public static void MapText(ILabelHandler handler, ILabel label)
		{
			handler.PlatformView?.UpdateText(label);
		}

		public static void MapTextColor(ILabelHandler handler, ILabel label)
		{
			handler.PlatformView?.UpdateTextColor(label.TextColor);
		}

		public static void MapFont(ILabelHandler handler, ILabel label)
		{
			var fontManager = handler.GetRequiredService<IFontManager>();

			handler.PlatformView?.UpdateFont(label, fontManager);
		}

		public static void MapHorizontalTextAlignment(ILabelHandler handler, ILabel label)
		{
			handler.PlatformView?.UpdateHorizontalTextAlignment(label);
		}

		public static void MapVerticalTextAlignment(ILabelHandler handler, ILabel label)
		{
			handler.PlatformView?.UpdateVerticalTextAlignment(label);
		}

		public static void MapLineBreakMode(ILabelHandler handler, ILabel label)
		{
			handler.PlatformView?.UpdateLineBreakMode(label);
		}

		public static void MapMaxLines(ILabelHandler handler, ILabel label)
		{
			handler.PlatformView?.UpdateMaxLines(label);
		}

		public static void MapPadding(ILabelHandler handler, ILabel label)
		{
			handler.PlatformView.WithPadding(label.Padding);
		}

		public static void MapCharacterSpacing(ILabelHandler handler, ILabel label)
		{
			if (handler.PlatformView is not { } nativeView)
				return;

			nativeView.Attributes = nativeView.Attributes.AttrListFor(label.TextDecorations, label.CharacterSpacing);
		}

		public static void MapTextDecorations(ILabelHandler handler, ILabel label)
		{
			if (handler.PlatformView is not { } nativeView)
				return;

			nativeView.Attributes = nativeView.Attributes.AttrListFor(label.TextDecorations, label.CharacterSpacing);
		}

		public static void MapLineHeight(ILabelHandler handler, ILabel label)
		{
			if (handler.PlatformView is not { } nativeView)
				return;

			if (handler.VirtualView is not { } virtualView)
				return;

			if (label.LineHeight > 1)
			{
				// there is no LineHeight for label in gtk3:
				// https://gitlab.gnome.org/GNOME/gtk/-/issues/2379

				// try to set it over css: not working: exception thrown: 'line-height' is not a valid property name
				// nativeView.SetStyleValue($"{(int)label.LineHeight}","line-height");

				// try to set it over https://docs.gtk.org/Pango/method.Layout.set_line_spacing.html

				// no effect: https://docs.gtk.org/gtk3/method.Label.get_layout.html
				// The label is free to recreate its layout at any time, so it should be considered read-only
				// nativeView.Layout.LineSpacing = (float)label.LineHeight;

				// so we use LabelView, this sets it before OnDrawn:
				nativeView.LineHeight = (float)label.LineHeight;
			}
		}

	}

}