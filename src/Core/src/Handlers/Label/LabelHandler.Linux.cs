using System;
using Gtk;
using Microsoft.Maui.Graphics;

namespace Microsoft.Maui.Handlers
{

	public partial class LabelHandler : ViewHandler<ILabel, Label>
	{

		// https://developer.gnome.org/gtk3/stable/GtkLabel.html
		protected override Label CreateNativeView()
		{
			return new Label()
			{
				// Hexpand = true,
				// MaxWidthChars = 1,
			};
		}

		private SizeRequest _perfectSize;

		private bool _perfectSizeValid;
		// private bool _allocated = false;

		private SizeRequest GetPerfectSize(int widthConstraint = -1)
		{
			if (NativeView is not { } nativeView)
				return default;

			nativeView.Layout.Width = Pango.Units.FromPixels(widthConstraint);
			nativeView.Layout.GetPixelSize(out int w, out int h);

			return new SizeRequest(new Size(w, h));
		}

		public virtual Size GetDesiredSize1(double widthConstraint, double heightConstraint)
		{
			if (VirtualView is not { } virtualView)
				return default;

			// if (!_allocated && PlatformHelper.GetGTKPlatform() == GTKPlatform.Windows)
			// {
			// 	return default(SizeRequest);
			// }

			if (!_perfectSizeValid)
			{
				_perfectSize = GetPerfectSize();
				_perfectSize.Minimum = new Size(Math.Min(10, _perfectSize.Request.Width), _perfectSize.Request.Height);
				_perfectSizeValid = true;
			}

			var widthFits = widthConstraint >= _perfectSize.Request.Width;
			var heightFits = heightConstraint >= _perfectSize.Request.Height;

			if (widthFits && heightFits)
				return _perfectSize;

			var result = GetPerfectSize((int)widthConstraint);
			var tinyWidth = Math.Min(10, result.Request.Width);
			result.Minimum = new Size(tinyWidth, result.Request.Height);

			if (widthFits || virtualView.LineBreakMode == LineBreakMode.NoWrap)
			{
				return new SizeRequest(
					new Size(result.Request.Width, _perfectSize.Request.Height),
					new Size(result.Minimum.Width, _perfectSize.Request.Height));
			}

			bool containerIsNotInfinitelyWide = !double.IsInfinity(widthConstraint);

			if (containerIsNotInfinitelyWide)
			{
				bool textCouldHaveWrapped = virtualView.LineBreakMode == LineBreakMode.WordWrap || virtualView.LineBreakMode == LineBreakMode.CharacterWrap;
				bool textExceedsContainer = result.Request.Width > widthConstraint;

				if (textExceedsContainer || textCouldHaveWrapped)
				{
					var expandedWidth = Math.Max(tinyWidth, widthConstraint);
					result.Request = new Size(expandedWidth, result.Request.Height);
				}
			}

			return result;
		}

		public static void MapText(LabelHandler handler, ILabel label)
		{
			handler.NativeView?.UpdateText(label);
		}

		public static void MapTextColor(LabelHandler handler, ILabel label)
		{
			handler.NativeView?.UpdateTextColor(label.TextColor);
		}

		public static void MapFont(LabelHandler handler, ILabel label)
		{
			handler.MapFont(label);
		}

		public static void MapHorizontalTextAlignment(LabelHandler handler, ILabel label)
		{
			handler.NativeView?.UpdateTextAlignment(label);
		}

		public static void MapLineBreakMode(LabelHandler handler, ILabel label)
		{
			handler.NativeView?.UpdateLineBreakMode(label);
		}

		public static void MapMaxLines(LabelHandler handler, ILabel label)
		{
			handler.NativeView?.UpdateMaxLines(label);
		}

		public static void MapPadding(LabelHandler handler, ILabel label)
		{
			handler.NativeView.WithPadding(label.Padding);

		}

		[MissingMapper]
		public static void MapCharacterSpacing(LabelHandler handler, ILabel label)
		{ }

		[MissingMapper]
		public static void MapTextDecorations(LabelHandler handler, ILabel label)
		{ }

		[MissingMapper]
		public static void MapLineHeight(LabelHandler handler, ILabel label)
		{
			// there is no LineHeight for label in gtk3:
			// https://gitlab.gnome.org/GNOME/gtk/-/issues/2379
		}

	}

}