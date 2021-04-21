using Gtk;

namespace Microsoft.Maui
{

	public static class WidgetExtensions
	{

		public static void UpdateIsEnabled(this Widget native, bool isEnabled) =>
			native.Sensitive = isEnabled;

	}

}