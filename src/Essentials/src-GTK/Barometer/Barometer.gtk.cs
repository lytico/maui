using System;
using Windows.Devices.Sensors;

namespace Microsoft.Maui.Devices.Sensors
{
	partial class BarometerImplementation : IBarometer
	{
		public bool IsSupported => false;

		void PlatformStart(SensorSpeed sensorSpeed)
		{
		}
	}
}
