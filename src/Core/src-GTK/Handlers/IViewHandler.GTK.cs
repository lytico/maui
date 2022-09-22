namespace Microsoft.Maui
{
	public interface IPlatformViewHandler : IViewHandler
	{
		new CustomView? PlatformView { get; }

		new CustomView? ContainerView { get; }
	}
}