using Gtk;
using Microsoft.Maui;
using Microsoft.Maui.Graphics;

namespace Maui.SimpleSampleApp
{

	public class SimpleSampleGtkApplication : MauiGtkApplication<Startup>
	{

		public SimpleSampleGtkApplication() : base()
		{
			TopContainerOverride = OnTopContainerOverride;
		}

		Widget OnTopContainerOverride(Widget nativePage)
		{
			var b = new Box(Orientation.Vertical, 0)
			{
				Fill = true,
				Expand = true,
				Margin = 5,

			};

			var txt = $"{typeof(Startup).Namespace} {nameof(TopContainerOverride)}";

			var but = new Button() { Label = "Gtk Test" };
			var filename = "dotnet_bot.png";
			var pix = new Gdk.Pixbuf(filename);
			var css = pix.CssImage();
// css =  $"url('{filename}')";
			but.SetStyleImage(css, "background-image");
			b.PackStart(but, false, false, 0);

			b.PackStart(nativePage, true, true, 0);

			return b;
		}

	}

}