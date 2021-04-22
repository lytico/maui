using Microsoft.Maui.Hosting;

namespace Microsoft.Maui.Controls.Compatibility
{

	public static class AppHostBuilderExtensions
	{

		public static IAppHostBuilder UseCompatibilityRenderers(this IAppHostBuilder builder)
		{

			return builder;
		}

	}

}

namespace Microsoft.Maui.Controls.Compatibility
{

	public static class Forms
	{

		public static void Init(IActivationState state)
		{ }

	}

}