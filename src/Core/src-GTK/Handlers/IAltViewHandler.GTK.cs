namespace Microsoft.Maui
{
	public interface IAltPlatformViewHandler : IAltViewHandler
	{
		new CustomAltView? PlatformView { get; }

		new CustomAltView? ContainerView { get; }
	}
}