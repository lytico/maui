#nullable enable
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Maui.ApplicationModel;

namespace Microsoft.Maui.Devices
{
	partial class DeviceDisplayImplementation
	{
		protected override bool GetKeepScreenOn()
		{
			return true;
		}

		protected override void SetKeepScreenOn(bool keepScreenOn)
		{
		}

		protected override DisplayInfo GetMainDisplayInfo()
		{
			var windows = Gtk.Window.ListToplevels();
			var wwidth = 1;
			var wheight = 1;
			var wdensity = 1;
			var worientation = DisplayOrientation.Landscape;
			var wrotation = DisplayRotation.Rotation0;
			var wrefreshRate = 60.0f;

			if (windows != null)
			{
				if (windows.Length > 0)
				{
					var window = windows[0];
					wwidth = window.WidthRequest;
					wheight = window.HeightRequest;
				}
			}

			return new DisplayInfo(
				wwidth,
				wheight,
				wdensity,
				worientation,
				wrotation,
				wrefreshRate
				);
		}

		protected override void StartScreenMetricsListeners()
		{
		}

		protected override void StopScreenMetricsListeners()
		{
		}
	}
}
