using System;
using System.Threading.Tasks;
using Microsoft.Maui.DeviceTests.Stubs;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Handlers;

namespace Microsoft.Maui.DeviceTests
{

	public partial class ActivityIndiatorHandlerTests
	{

		bool GetNativeIsRunning(ActivityIndicatorHandler arg)
		{
			throw new System.NotImplementedException();
		}

		async Task ValidateColor(ActivityIndicatorStub activityIndicator, Color yellow, Func<Color> func)
		{
			await Task.CompletedTask;
		}

	}

}