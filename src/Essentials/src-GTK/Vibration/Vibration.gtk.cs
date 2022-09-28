using System;

namespace Microsoft.Maui.Devices
{
	partial class VibrationImplementation : IVibration
	{
		public bool IsSupported
			=> false;

		void PlatformVibrate()
			=> throw new NotImplementedException("GTK");

		void PlatformVibrate(TimeSpan duration) =>
			throw new NotImplementedException("GTK");

		void PlatformCancel() =>
			throw new NotImplementedException("GTK");
	}
}
