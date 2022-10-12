namespace Microsoft.Maui.Handlers
{
#if WINDOWS && !__GTK__
	public record OpenWindowRequest(IPersistedState? State = null, UI.Xaml.LaunchActivatedEventArgs? LaunchArgs = null);
#else
	public record OpenWindowRequest(IPersistedState? State = null);
#endif
}