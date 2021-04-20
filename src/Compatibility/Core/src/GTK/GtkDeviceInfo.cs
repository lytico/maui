using Microsoft.Maui.Controls.Compatibility.Internals;
using Microsoft.Maui.Controls.Internals;
using Microsoft.Maui.Graphics;

namespace Microsoft.Maui.Controls.Compatibility.Platform.GTK
{
	public class GtkDeviceInfo : DeviceInfo
	{
		public override Size PixelScreenSize
		{
			get
			{
				return new Size(800, 600);
			}
		}

		public override Size ScaledScreenSize
		{
			get
			{
				return new Size(800, 600);
			}
		}

		public override double ScalingFactor
		{
			get
			{
				return 1.0d;
			}
		}
	}
}
