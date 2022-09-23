namespace Microsoft.Maui
{
	public interface IPlatformViewHandler : IViewHandler
	{
		new MauiView? PlatformView { get; }

		new MauiView? ContainerView { get; }
	}
}