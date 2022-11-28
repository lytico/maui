#nullable enable
using System;
using Microsoft.Maui.Controls.Internals;
using Microsoft.Maui.Platform.GTK;

namespace Microsoft.Maui.Controls.Platform
{
	internal static class LabelExtensions
	{
		public static void UpdateLineBreakMode(this MauiGTKLabel textBlock, Label label) =>
			textBlock.UpdateLineBreakMode(label.LineBreakMode);

		public static void UpdateLineBreakMode(this MauiGTKLabel textBlock, LineBreakMode lineBreakMode)
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

		static void DetermineTruncatedTextWrapping(MauiGTKLabel textBlock) { }
			// textBlock.TextWrapping = textBlock.MaxLines > 1 ? TextWrapping.Wrap : TextWrapping.NoWrap;

		public static void UpdateText(this MauiGTKLabel platformControl, Label label)
		{
			switch (label.TextType)
			{
				case TextType.Html:
					platformControl.UpdateTextHtml(label.Text);
					break;

				default:
					platformControl.Text = label.Text;
					break;
					//	if (label.FormattedText != null)
					//		platformControl.UpdateInlines(label);
					//	else
					//		platformControl.Text = TextTransformUtilites.GetTransformedText(label.Text, label.TextTransform);
					//	break;
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

		public static void UpdateMaxLines(this MauiGTKLabel platformControl, Label label)
		{
			//if (label.MaxLines >= 0)
			//	platformControl.MaxLines = label.MaxLines;
			//else
			//	platformControl.MaxLines = 0;
		}

		public static void UpdateDetectReadingOrderFromContent(this MauiGTKLabel platformControl, Label label)
		{
			//if (label.IsSet(Specifics.DetectReadingOrderFromContentProperty))
			//	platformControl.SetTextReadingOrder(label.OnThisPlatform().GetDetectReadingOrderFromContent());
		}

		internal static void SetTextReadingOrder(this MauiGTKLabel platformControl, bool detectReadingOrderFromContent) { }
			//platformControl.TextReadingOrder = detectReadingOrderFromContent
			//	? TextReadingOrder.DetectFromContent
			//	: TextReadingOrder.UseFlowDirection;
	}
}