using System;
using Microsoft.Maui.Graphics;

namespace Microsoft.Maui.Handlers
{
	public partial class LabelHandler : ViewHandler<ILabel, MauiView>
	{
		protected override MauiView CreatePlatformView()
		{
			var plat = new MauiView();
			plat.AddChildWidget(new Gtk.Label());

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

			//if (handler.PlatformView.GetChildWidget() is Gtk.Label platformLabel)
			//{
			//	if (label.Background != null)
			//	{
			//		if (label.Background.ToColor() != null)
			//		{
			//			byte r, g, b;
			//			r = 0;
			//			g = 0;
			//			b = 0;
			//			label.Background.ToColor()?.ToRgb(out r, out g, out b);
			//			if (r != 0 && g != 0 && b != 0)
			//			{
			//				// handler.PlatformView.SetBackgroundColor(new Gdk.Color(r, g, b));
			//				var color = new Gdk.Color(r, g, b);
			//				handler.PlatformView.ModifyBg(Gtk.StateType.Normal, color);
			//				handler.PlatformView.ModifyBg(Gtk.StateType.Prelight, color);
			//				handler.PlatformView.ModifyBg(Gtk.StateType.Active, color);
			//			}
			//		}
			//	}
			//}
		}

		public static void MapText(ILabelHandler handler, ILabel label)
		{
			//handler.PlatformView?.UpdateTextPlainText(label);

			if (handler.PlatformView.GetChildWidget() is Gtk.Label platformLabel)
			{
				platformLabel.Text = label.Text;
				SetMarkupAttributes(handler, label);
			}

			// Any text update requires that we update any attributed string formatting
			MapFormatting(handler, label);
		}

		public static void MapTextColor(ILabelHandler handler, ILabel label)
		{
			SetMarkupAttributes(handler, label);
			//handler.PlatformView?.UpdateTextColor(label);
			//if (handler.PlatformView.GetChildWidget() is Gtk.Label platformLabel)
			//{
			//	if (label.TextColor != null)
			//	{
			//		byte r, g, b;
			//		r = 0;
			//		g = 0;
			//		b = 0;
			//		label.TextColor.ToRgb(out r, out g, out b);
			//		if (r != 0 && g != 0 && b != 0)
			//		{
			//			// handler.PlatformView.SetBackgroundColor(new Gdk.Color(r, g, b));
			//			var color = new Gdk.Color(r, g, b);
			//			platformLabel.ModifyFg(Gtk.StateType.Normal, color);
			//			platformLabel.ModifyFg(Gtk.StateType.Prelight, color);
			//			platformLabel.ModifyFg(Gtk.StateType.Active, color);
			//		}
			//	}
			//}
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
			SetMarkupAttributes(handler, label);

			//// var fontManager = handler.GetRequiredService<IFontManager>();
			//if (handler.PlatformView.GetChildWidget() is Gtk.Label platformLabel)
			//{
			//	Pango.FontDescription fontdesc = new Pango.FontDescription();
			//	fontdesc.Family = label.Font.Family;
			//	fontdesc.Size = (int)(label.Font.Size * Pango.Scale.PangoScale);

			//	platformLabel.ModifyFont(fontdesc);
			//}

			////handler.PlatformView?.UpdateFont(label, fontManager);
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

		static void SetMarkupAttributes(ILabelHandler handler, ILabel label)
		{
			if (handler.PlatformView.GetChildWidget() is Gtk.Label platformLabel)
			{
				var text = platformLabel.Text;
				var markup = string.Empty;

				if (label.TextColor == null)
				{
					markup += "<span";
				}
				else
				{
					markup += "<span foreground='";
					markup += label.TextColor.ToColorString();
					markup += "'";
				}

				if (label.Font.Family == null)
				{
					if (label.TextColor != null)
					{
						markup += "'";
					}
				}
				else
				{
					if (label.TextColor != null)
					{
						markup += "'";
					}
					markup += " font_desc='";
					markup += label.Font.Family;
					markup += "'";
				}

				if (label.Font.Size > 0)
				{
					if (label.TextColor != null || label.Font.Family != null)
					{
						markup += "'";
					}
					markup += " font_size='";
					markup += Convert.ToInt32(label.Font.Size).ToString("D");
					markup += "'";
				}

				markup += ">";
				markup += text;
				markup += "</span>";
				platformLabel.Markup = markup;
			}
		}
	}
}