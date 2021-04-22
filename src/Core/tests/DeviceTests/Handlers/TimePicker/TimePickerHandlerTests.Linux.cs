using System;
using System.Threading.Tasks;
using Microsoft.Maui.DeviceTests.Stubs;
using Microsoft.Maui.Handlers;

namespace Microsoft.Maui.DeviceTests
{

	public partial class TimePickerHandlerTests
	{

		async Task ValidateTime(TimePickerStub timePicker, Func<TimeSpan> func)
		{
			await Task.CompletedTask;
		}

		double GetNativeUnscaledFontSize(TimePickerHandler arg)
		{
			throw new NotImplementedException();
		}

		bool GetNativeIsBold(TimePickerHandler arg)
		{
			throw new NotImplementedException();
		}

		bool GetNativeIsItalic(TimePickerHandler arg)
		{
			throw new NotImplementedException();
		}

	}

}