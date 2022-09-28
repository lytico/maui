#nullable enable
using System;
using System.Threading;

namespace Microsoft.Maui.ApplicationModel
{
	public static partial class MainThread
	{
		static bool PlatformIsMainThread => !Thread.CurrentThread.IsBackground;

		static void PlatformBeginInvokeOnMainThread(Action action)
		{
			GLib.Idle.Add(delegate { action(); return false; });
		}
	}
}
