using System;
using System.Threading.Tasks;
using Android.Views;
using Android.Widget;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Graphics;

namespace Microsoft.Maui.DeviceTests
{
	public partial class ActivityIndiatorHandlerTests
	{
		ProgressBar GetNativeActivityIndicator(ActivityIndicatorHandler activityIndicatorHandler) =>
			(ProgressBar)activityIndicatorHandler.NativeView;

		bool GetNativeIsRunning(ActivityIndicatorHandler activityIndicatorHandler) =>
			GetNativeActivityIndicator(activityIndicatorHandler).Visibility == ViewStates.Visible;

		Task ValidateColor(IActivityIndicator activityIndicator, Color color, Action action = null) =>
			ValidateHasColor(activityIndicator, color, action);

		async Task ValidateHasColor(IActivityIndicator activityIndicator, Color color, Action action = null)
		{
			await Task.CompletedTask;
		}
	}
}