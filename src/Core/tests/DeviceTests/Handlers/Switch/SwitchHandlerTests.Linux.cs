using System;
using System.Threading.Tasks;
using Microsoft.Maui.DeviceTests.Stubs;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Handlers;

namespace Microsoft.Maui.DeviceTests
{

	public partial class SwitchHandlerTests
	{

		bool GetNativeIsChecked(SwitchHandler arg)
		{
			throw new System.NotImplementedException();
		}

		async Task ValidateTrackColor(SwitchStub switchStub, Color red)
		{
			await Task.CompletedTask;
		}

		async Task ValidateThumbColor(SwitchStub switchStub, Color blue)
		{
			await Task.CompletedTask;

		}

		async Task ValidateTrackColor(SwitchStub switchStub, Color red, Func<Color> func)
		{
			await Task.CompletedTask;

		}

		async Task ValidateThumbColor(SwitchStub switchStub, Color blue, Func<Color> func)
		{
			await Task.CompletedTask;

		}

	}

}