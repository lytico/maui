namespace Microsoft.Maui.LifecycleEvents
{
	public static class GTKLifecycleBuilderExtensions
	{
		public static IGTKLifecycleBuilder OnApplicationCreating(this IGTKLifecycleBuilder lifecycle, GTKLifecycle.OnApplicationCreating del) => lifecycle.OnEvent(del);
		public static IGTKLifecycleBuilder OnApplicationCreate(this IGTKLifecycleBuilder lifecycle, GTKLifecycle.OnApplicationCreate del) => lifecycle.OnEvent(del);
	}
}