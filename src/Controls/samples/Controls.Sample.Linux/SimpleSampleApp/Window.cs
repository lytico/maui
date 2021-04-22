using Microsoft.Maui;

namespace Maui.SimpleSampleApp
{
	public class Window : IWindow
	{
		public IPage Page { get; set; }

		public IMauiContext MauiContext { get; set; }
	}
}
