using System;
using Gtk;
using static Microsoft.Maui.Controls.Button;

namespace Microsoft.Maui.Controls.Compatibility.Platform.GTK.Extensions
{
	public static class ButtonContentLayoutExtensions
	{
		public static PositionType AsPositionType(this Button.ButtonContentLayout.ImagePosition position)
		{
			switch (position)
			{
				case Button.ButtonContentLayout.ImagePosition.Bottom:
					return PositionType.Bottom;
				case Button.ButtonContentLayout.ImagePosition.Left:
					return PositionType.Left;
				case Button.ButtonContentLayout.ImagePosition.Right:
					return PositionType.Right;
				case Button.ButtonContentLayout.ImagePosition.Top:
					return PositionType.Top;
				default:
					throw new ArgumentOutOfRangeException(nameof(position));
			}
		}
	}
}
