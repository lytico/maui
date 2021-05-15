﻿using System;
using System.Threading.Tasks;
using Android.Views;
using Android.Widget;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Handlers;

namespace Microsoft.Maui.DeviceTests
{
	public partial class ActivityIndicatorHandlerTests
	{
		ProgressBar GetNativeActivityIndicator(ActivityIndicatorHandler activityIndicatorHandler) =>
			activityIndicatorHandler.NativeView;

		bool GetNativeIsRunning(ActivityIndicatorHandler activityIndicatorHandler) =>
			GetNativeActivityIndicator(activityIndicatorHandler).Visibility == ViewStates.Visible;

		Task ValidateHasColor(IActivityIndicator activityIndicator, Color color, Action action = null)
		{
			return InvokeOnMainThreadAsync(() =>
			{
				var nativeActivityIndicator = GetNativeActivityIndicator(CreateHandler(activityIndicator));
				action?.Invoke();
				nativeActivityIndicator.AssertContainsColor(color);
			});
		}
	}
}