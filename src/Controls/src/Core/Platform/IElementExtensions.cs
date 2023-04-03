using System;
#if !__GTK__
using Microsoft.Maui.Controls.PlatformConfiguration.WindowsSpecific;
#endif

namespace Microsoft.Maui.Controls
{
	public static class IElementExtensions
	{
		internal static bool IsShimmed(this Microsoft.Maui.IElement? self)
		{
			string typeName = $"{self?.Handler?.GetType().Name}";

			return (typeName == "RendererToHandlerShim" ||
				typeName == "HandlerToRendererShim");
		}
	}
}