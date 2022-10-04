using Gtk;

namespace Microsoft.Maui.LifecycleEvents
{
	public static class GTKLifecycle
	{
		// Events called by the Application
		public delegate void OnApplicationCreating(Application application);
		public delegate void OnApplicationCreate(Application application);
		public delegate void OnWindowCreated(Window window);

		// Events called by the ActivityLifecycleCallbacks

		// Events called by Activity overrides (always call base)

		// Events called by Activity overrides (calling base is optional)

		// Internal events
		internal delegate void OnMauiContextCreated(IMauiContext mauiContext);
	}
}