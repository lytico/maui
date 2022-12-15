using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Maui.Controls.Internals;
using Microsoft.Maui.Controls.Platform;
using Microsoft.Maui.Handlers;


namespace Microsoft.Maui.Controls
{
	public partial class CheckBox
	{
		public static void MapText(ICheckBoxHandler handler, CheckBox button)
		{
			//var text = TextTransformUtilites.GetTransformedText(button.Text, button.TextTransform);
			//if (handler.PlatformView != null)
			//{
			//	handler.PlatformView.Label = text;
			//}
			////handler.PlatformView?.UpdateText(text);
			// button.Handler?.UpdateValue(nameof(CheckBox.ContentLayout));
		}

		public static void MapText(CheckBoxHandler handler, CheckBox button) =>
			MapText((ICheckBoxHandler)handler, button);
	}
}
