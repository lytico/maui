namespace Microsoft.Maui
{
	public interface IPlatformViewHandler : IViewHandler
	{
		new Gtk.Fixed? PlatformView { get; }

		new Gtk.Fixed? ContainerView { get; }
	}
}