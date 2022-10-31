namespace Microsoft.Maui
{
	public interface IPlatformViewHandler : IViewHandler
	{
		new System.Object? PlatformView { get; }

		new System.Object? ContainerView { get; }
	}
}