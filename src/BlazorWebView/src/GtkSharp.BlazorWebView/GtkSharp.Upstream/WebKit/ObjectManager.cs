namespace WebKit.Upstream;

public partial class ObjectManager
{

	static bool initialized = false;

	// Call this method from the appropriate module init function.
	public static void Initialize()
	{
		if (initialized)
			return;

		initialized = true;
		
		GLib.GType.Register(WebKit.Upstream.JavascriptResult.GType, typeof(WebKit.Upstream.JavascriptResult));

		GLib.GType.Register(WebKit.Upstream.JavaScriptValue.GType, typeof(WebKit.Upstream.JavaScriptValue));
	}

}