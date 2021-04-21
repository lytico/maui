using Microsoft.Maui.Controls.Internals;
using Microsoft.Maui.Graphics;

namespace Microsoft.Maui.Controls.Compatibility
{

	public class GtkDeviceInfo : DeviceInfo
	{

		public override Size PixelScreenSize { get; }

		public override Size ScaledScreenSize { get; }

		public override double ScalingFactor { get; }

	}

}