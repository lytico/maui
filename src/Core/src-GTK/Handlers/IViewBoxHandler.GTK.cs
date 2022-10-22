namespace Microsoft.Maui
{
	public interface IPlatformViewBoxHandler : IViewHandler
	{
		new Gtk.VBox? PlatformView { get; }

		new Gtk.VBox? ContainerView { get; }
	}
}