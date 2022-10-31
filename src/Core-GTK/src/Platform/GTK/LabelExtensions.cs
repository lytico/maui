using System;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace Microsoft.Maui.Platform
{
	public static class LabelExtensions
	{
		public static void UpdateFont(this MauiView platformControl, Font font, IFontManager fontManager)
		{
			if (platformControl.GetChildWidget() is Gtk.Label platformLabel)
			{
				Pango.FontDescription fontdesc = new Pango.FontDescription();
				fontdesc.Family = fontManager.GetFontFamily(font);
				fontdesc.Size = (int)(fontManager.GetFontSize(font) * Pango.Scale.PangoScale);

				platformLabel.ModifyFont(fontdesc);
			}
		}

		public static void UpdateFont(this MauiView platformControl, IText text, IFontManager fontManager) =>
			platformControl.UpdateFont(text.Font, fontManager);

		public static void UpdateText(this MauiView platformControl, ILabel label)
		{
			platformControl.UpdateTextPlainText(label);
		}

		public static void UpdateTextColor(this MauiView platformControl, ITextStyle text) { }
			//platformControl.UpdateProperty(TextBlock.ForegroundProperty, text.TextColor);

		public static void UpdatePadding(this MauiView platformControl, ILabel label) { }
			//platformControl.UpdateProperty(TextBlock.PaddingProperty, label.Padding.ToPlatform());

		public static void UpdateCharacterSpacing(this MauiView platformControl, ITextStyle label)
		{
			// platformControl.CharacterSpacing = label.CharacterSpacing.ToEm();
		}

		public static void UpdateTextDecorations(this MauiView platformControl, ILabel label)
		{
			//var elementTextDecorations = label.TextDecorations;

			//if ((elementTextDecorations & TextDecorations.Underline) == 0)
			//	platformControl.TextDecorations &= ~global::Windows.UI.Text.TextDecorations.Underline;
			//else
			//	platformControl.TextDecorations |= global::Windows.UI.Text.TextDecorations.Underline;

			//if ((elementTextDecorations & TextDecorations.Strikethrough) == 0)
			//	platformControl.TextDecorations &= ~global::Windows.UI.Text.TextDecorations.Strikethrough;
			//else
			//	platformControl.TextDecorations |= global::Windows.UI.Text.TextDecorations.Strikethrough;

			//// TextDecorations are not updated in the UI until the text changes
			//if (platformControl.Inlines != null && platformControl.Inlines.Count > 0)
			//{
			//	foreach (var inline in platformControl.Inlines)
			//	{
			//		if (inline is Run run)
			//		{
			//			run.Text = run.Text;
			//		}
			//		else if (inline is Span span)
			//		{
			//			foreach (var inline2 in span.Inlines)
			//			{
			//				if (inline2 is Run run2)
			//				{
			//					run2.Text = run2.Text;
			//				}
			//			}
			//		}
			//	}
			//}
			//else
			//{
			//	platformControl.Text = platformControl.Text;
			//}
		}

		public static void UpdateLineHeight(this MauiView platformControl, ILabel label)
		{
			//if (label.LineHeight >= 0)
			//{
			//	platformControl.LineHeight = label.LineHeight * platformControl.FontSize;
			//}
		}

		public static void UpdateHorizontalTextAlignment(this MauiView platformControl, ILabel label)
		{
			//// We don't have a FlowDirection yet, so there's nothing to pass in here. 
			//// TODO: Update this when FlowDirection is available 
			//platformControl.TextAlignment = label.HorizontalTextAlignment.ToPlatform(true);
		}

		public static void UpdateVerticalTextAlignment(this MauiView platformControl, ILabel label)
		{
			//platformControl.VerticalAlignment = label.VerticalTextAlignment.ToPlatformVerticalAlignment();
		}

		internal static void UpdateTextHtml(this MauiView platformControl, ILabel label)
		{
			//var text = label.Text ?? string.Empty;

			//// Just in case we are not given text with elements.
			//var modifiedText = string.Format("<div>{0}</div>", text);
			//modifiedText = Regex.Replace(modifiedText, "<br>", "<br></br>", RegexOptions.IgnoreCase);

			//// Reset the text because we will add to it.
			//platformControl.Inlines.Clear();

			//try
			//{
			//	var element = XElement.Parse(modifiedText);
			//	LabelHtmlHelper.ParseText(element, platformControl.Inlines, label);
			//}
			//catch (Exception)
			//{
			//	// If anything goes wrong just show the html
			//	platformControl.Text = global::Windows.Data.Html.HtmlUtilities.ConvertToText(label.Text);
			//}
		}

		internal static void UpdateTextPlainText(this MauiView platformControl, IText label)
		{
			if (platformControl.GetChildWidget() is Gtk.Label platformLabel)
			{
				platformLabel.Text = label.Text;
			}
		}
	}
}