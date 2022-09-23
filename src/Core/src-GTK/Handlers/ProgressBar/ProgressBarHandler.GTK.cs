namespace Microsoft.Maui.Handlers
{
	public partial class ProgressBarHandler : ViewHandler<IProgress, CustomView>
	{
		protected override CustomView CreatePlatformView()
		{
			var plat = new CustomView();
			plat.Add(new Gtk.ProgressBar());

			return plat;
		}

		public static void MapProgress(IProgressBarHandler handler, IProgress progress)
		{
			//handler.PlatformView?.UpdateProgress(progress);
		}

		public static void MapProgressColor(IProgressBarHandler handler, IProgress progress)
		{
			//handler.PlatformView?.UpdateProgressColor(progress);
		}
	}
}