namespace Microsoft.Maui.Handlers
{
	public partial class ProgressBarHandler : ViewHandler<IProgress, MauiView>
	{
		protected override MauiView CreatePlatformView()
		{
			var plat = new MauiView();
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