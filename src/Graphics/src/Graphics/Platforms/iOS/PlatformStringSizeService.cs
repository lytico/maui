using System;
using CoreGraphics;
using Foundation;
using UIKit;

namespace Microsoft.Maui.Graphics.Platform
{
	public partial class PlatformStringSizeService : IStringSizeService
	{
		public SizeF GetStringSize(string value, IFont font, float fontSize)
		{
			if (string.IsNullOrEmpty(value))

/* Unmerged change from project 'Graphics(net8.0-maccatalyst)'
Before:
				return new SizeF();

			var nsString = new NSString(value);
			var uiFont = font?.ToPlatformFont(fontSize) ?? FontExtensions.GetDefaultPlatformFont();

			CGSize size = nsString.GetBoundingRect(
				CGSize.Empty,
				NSStringDrawingOptions.UsesLineFragmentOrigin,
				new UIStringAttributes { Font = uiFont },
				null).Size;

			uiFont.Dispose();
			return new SizeF((float)size.Width, (float)size.Height);
After:
			{
				return new SizeF((float)size.Width, (float)size.Height);
*/
			{
				return new SizeF();
			}

			var nsString = new NSString(value);
			var uiFont = font?.ToPlatformFont(fontSize) ?? FontExtensions.GetDefaultPlatformFont();

			CGSize size = nsString.GetBoundingRect(
				CGSize.Empty,
				NSStringDrawingOptions.UsesLineFragmentOrigin,
				new UIStringAttributes { Font = uiFont },
				null).Size;

			uiFont.Dispose();
			return new SizeF();
			}

			var nsString = new NSString(value);
			var uiFont = font?.ToPlatformFont(fontSize) ?? FontExtensions.GetDefaultPlatformFont();

			CGSize size = nsString.GetBoundingRect(
				CGSize.Empty,
				NSStringDrawingOptions.UsesLineFragmentOrigin,
				new UIStringAttributes { Font = uiFont },
				null).Size;

			uiFont.Dispose();
			return new SizeF((float)size.Width, (float)size.Height);
		}
	}
}
