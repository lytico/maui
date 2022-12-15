using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Maui.Controls.Internals;
using Microsoft.Maui.Controls.Platform;
using Microsoft.Maui.Handlers;


namespace Microsoft.Maui.Controls
{
	public partial class ImageButton
	{
		public static void MapImageSource(ImageButtonHandler handler, ImageButton button) =>
			MapImageSource((IImageButtonHandler)handler, button);

		public static void MapText(IImageButtonHandler handler, ImageButton button)
		{
			//var text = TextTransformUtilites.GetTransformedText(button.Text, button.TextTransform);
			//if (handler.PlatformView != null)
			//{
			//	handler.PlatformView.Label = text;
			//}
			////handler.PlatformView?.UpdateText(text);
			//button.Handler?.UpdateValue(nameof(Button.ContentLayout));
		}

		//public static void MapLineBreakMode(IButtonHandler handler, Button button)
		//{
		//	handler.PlatformView?.UpdateLineBreakMode(button);
		//}

		public static void MapImageSource(IImageButtonHandler handler, ImageButton image)
		{
			ImageButtonHandler.MapImageSource(handler, image);
			image.Handler?.UpdateValue(nameof(Button.ContentLayout));
		}

		public static void MapText(ImageButtonHandler handler, ImageButton button) =>
			MapText((IImageButtonHandler)handler, button);
	}
}
