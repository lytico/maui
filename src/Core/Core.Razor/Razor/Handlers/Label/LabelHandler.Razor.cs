using System;
using Microsoft.Maui.Razor;
using Microsoft.Maui.Razor.Components;

namespace Microsoft.Maui.Handlers.Razor
{

	public partial class LabelHandler : ViewHandler<ILabel, Label>
	{

		protected override Label CreateNativeView() => new();

		public static void MapText(LabelHandler handler, ILabel label)
		{
			if (handler?.NativeView is not { } nativeView)
				return;

			nativeView.SetText(label.Text);

		}

		public static void MapTextColor(LabelHandler handler, ILabel label) { }

		public static void MapCharacterSpacing(LabelHandler handler, ILabel label) { }

		public static void MapFont(LabelHandler handler, ILabel label) { }

		public static void MapHorizontalTextAlignment(LabelHandler handler, ILabel label) { }

		public static void MapVerticalTextAlignment(LabelHandler handler, ILabel label) { }

		public static void MapLineBreakMode(LabelHandler handler, ILabel label) { }

		public static void MapTextDecorations(LabelHandler handler, ILabel label) { }

		public static void MapMaxLines(LabelHandler handler, ILabel label) { }

		public static void MapPadding(LabelHandler handler, ILabel label) { }

		public static void MapLineHeight(LabelHandler handler, ILabel label) { }

	}

}