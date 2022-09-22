namespace Microsoft.Maui.Handlers
{
	public partial class ProgressBarHandler : AltViewHandler<IProgress, CustomAltView>
	{
		protected override CustomAltView CreatePlatformView()
		{
			var plat = new CustomAltView();
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