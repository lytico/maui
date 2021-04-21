using Gtk;

namespace Microsoft.Maui
{
	public static class ProgressBarExtensions
	{
		public static void UpdateProgress(this ProgressBar nativeProgressBar, IProgress progress)
		{
			nativeProgressBar.PulseStep = progress.Progress;
			nativeProgressBar.TooltipText = $"{progress.Progress * 100}%";
		}
	}
}