#nullable enable
using System;
using Microsoft.Maui.Controls.Internals;

namespace Microsoft.Maui.Controls.Platform
{
	internal static class LabelExtensions
	{
		public static void UpdateLineBreakMode(this MauiView textBlock, Label label) =>
			textBlock.UpdateLineBreakMode(label.LineBreakMode);

		public static void UpdateLineBreakMode(this MauiView textBlock, LineBreakMode lineBreakMode)
		{
			//if (textBlock == null)
			//	return;

			//switch (lineBreakMode)
			//{
			//	case LineBreakMode.NoWrap:
			//		textBlock.TextTrimming = TextTrimming.Clip;
			//		textBlock.TextWrapping = TextWrapping.NoWrap;
			//		break;
			//	case LineBreakMode.WordWrap:
			//		textBlock.TextTrimming = TextTrimming.None;
			//		textBlock.TextWrapping = TextWrapping.Wrap;
			//		break;
			//	case LineBreakMode.CharacterWrap:
			//		textBlock.TextTrimming = TextTrimming.WordEllipsis;
			//		textBlock.TextWrapping = TextWrapping.Wrap;
			//		break;
			//	case LineBreakMode.HeadTruncation:
			//		// TODO: This truncates at the end.
			//		textBlock.TextTrimming = TextTrimming.WordEllipsis;
			//		DetermineTruncatedTextWrapping(textBlock);
			//		break;
			//	case LineBreakMode.TailTruncation:
			//		textBlock.TextTrimming = TextTrimming.CharacterEllipsis;
			//		DetermineTruncatedTextWrapping(textBlock);
			//		break;
			//	case LineBreakMode.MiddleTruncation:
			//		// TODO: This truncates at the end.
			//		textBlock.TextTrimming = TextTrimming.WordEllipsis;
			//		DetermineTruncatedTextWrapping(textBlock);
			//		break;
			//	default:
			//		throw new ArgumentOutOfRangeException();
			//}
		}

		static void DetermineTruncatedTextWrapping(MauiView textBlock) { }
			// textBlock.TextWrapping = textBlock.MaxLines > 1 ? TextWrapping.Wrap : TextWrapping.NoWrap;

		public static void UpdateText(this MauiView platformControl, Label label)
		{
			if (platformControl.GetChildWidget() is Gtk.Label platformLabel)
			{
				switch (label.TextType)
				{
					case TextType.Html:
						platformControl.UpdateTextHtml(label);
						break;

					default:
						platformLabel.Text = label.Text;
						break;
						//	if (label.FormattedText != null)
						//		platformControl.UpdateInlines(label);
						//	else
						//		platformControl.Text = TextTransformUtilites.GetTransformedText(label.Text, label.TextTransform);
						//	break;
				}
			}
		}

		//public static double FindDefaultLineHeight(this MauiView control, Inline inline)
		//{
		//	control.Inlines.Add(inline);

		//	control.Measure(new WSize(double.PositiveInfinity, double.PositiveInfinity));

		//	var height = control.DesiredSize.Height;

		//	control.Inlines.Remove(inline);

		//	return height;
		//}

		public static void UpdateMaxLines(this MauiView platformControl, Label label)
		{
			//if (label.MaxLines >= 0)
			//	platformControl.MaxLines = label.MaxLines;
			//else
			//	platformControl.MaxLines = 0;
		}

		public static void UpdateDetectReadingOrderFromContent(this MauiView platformControl, Label label)
		{
			//if (label.IsSet(Specifics.DetectReadingOrderFromContentProperty))
			//	platformControl.SetTextReadingOrder(label.OnThisPlatform().GetDetectReadingOrderFromContent());
		}

		internal static void SetTextReadingOrder(this MauiView platformControl, bool detectReadingOrderFromContent) { }
			//platformControl.TextReadingOrder = detectReadingOrderFromContent
			//	? TextReadingOrder.DetectFromContent
			//	: TextReadingOrder.UseFlowDirection;
	}
}