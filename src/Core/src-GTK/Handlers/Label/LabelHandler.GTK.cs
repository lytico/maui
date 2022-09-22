using System;
using Microsoft.Maui.Graphics;

namespace Microsoft.Maui.Handlers
{
	public partial class LabelHandler : AltViewHandler<ILabel, CustomAltView>
	{
		protected override CustomAltView CreatePlatformView()
		{
			var plat = new CustomAltView();
			plat.Add(new Gtk.Label());

			return plat;
		}

		public override bool NeedsContainer =>
			VirtualView?.Background != null ||
			base.NeedsContainer;

		public override void PlatformArrange(Rect frame)
		{
			//var platformView = this.ToPlatform();

			//if (platformView == null || Context == null)
			//{
			//	return;
			//}

			//if (frame.Width < 0 || frame.Height < 0)
			//{
			//	return;
			//}

			//// Depending on our layout situation, the TextView may need an additional measurement pass at the final size
			//// in order to properly handle any TextAlignment properties.
			//if (NeedsExactMeasure())
			//{
			//	platformView.Measure(MakeMeasureSpecExact(frame.Width), MakeMeasureSpecExact(frame.Height));
			//}

			// base.PlatformArrange(frame);
		}

		public static void MapBackground(ILabelHandler handler, ILabel label)
		{
			handler.UpdateValue(nameof(IViewHandler.ContainerView));

			//handler.ToPlatform().UpdateBackground(label);
		}

		public static void MapText(ILabelHandler handler, ILabel label)
		{
			//handler.PlatformView?.UpdateTextPlainText(label);

			// Any text update requires that we update any attributed string formatting
			MapFormatting(handler, label);
		}

		public static void MapTextColor(ILabelHandler handler, ILabel label)
		{
			//handler.PlatformView?.UpdateTextColor(label);
		}

		public static void MapCharacterSpacing(ILabelHandler handler, ILabel label)
		{
			//handler.PlatformView?.UpdateCharacterSpacing(label);
		}

		public static void MapHorizontalTextAlignment(ILabelHandler handler, ILabel label)
		{
			//handler.PlatformView?.UpdateHorizontalTextAlignment(label);
		}

		public static void MapVerticalTextAlignment(ILabelHandler handler, ILabel label)
		{
			//handler.PlatformView?.UpdateVerticalTextAlignment(label);
		}

		public static void MapPadding(ILabelHandler handler, ILabel label)
		{
			//handler.PlatformView?.UpdatePadding(label);
		}

		public static void MapTextDecorations(ILabelHandler handler, ILabel label)
		{
			//handler.PlatformView?.UpdateTextDecorations(label);
		}

		public static void MapFont(ILabelHandler handler, ILabel label)
		{
			var fontManager = handler.GetRequiredService<IFontManager>();

			//handler.PlatformView?.UpdateFont(label, fontManager);
		}

		public static void MapLineHeight(ILabelHandler handler, ILabel label)
		{
			//handler.PlatformView?.UpdateLineHeight(label);
		}

		public static void MapFormatting(ILabelHandler handler, ILabel label)
		{
			// Update all of the attributed text formatting properties
			//handler.PlatformView?.UpdateLineHeight(label);
			//handler.PlatformView?.UpdateTextDecorations(label);
			//handler.PlatformView?.UpdateCharacterSpacing(label);

			// Setting any of those may have removed text alignment settings,
			// so we need to make sure those are applied, too
			//handler.PlatformView?.UpdateHorizontalTextAlignment(label);
		}

		public override Size GetDesiredSize(double widthConstraint, double heightConstraint)
		{
			// throw new NotImplementedException();
			return new Size(widthConstraint, heightConstraint);
		}
	}
}