namespace Microsoft.Maui.Handlers
{
	public partial class ProgressBarHandler : ViewHandler<IProgress, MauiView>
	{
		protected override MauiView CreatePlatformView(IView progressBar)
		{
			var plat = new MauiView();
			plat.AddChildWidget(new Gtk.ProgressBar());

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