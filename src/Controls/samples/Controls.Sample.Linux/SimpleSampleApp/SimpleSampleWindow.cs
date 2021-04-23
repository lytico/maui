using Microsoft.Maui;

namespace Maui.SimpleSampleApp
{
	public class SimpleSampleWindow : IWindow
	{
		public IPage Page { get; set; }

		public IMauiContext MauiContext { get; set; }
	}
}
