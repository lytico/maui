using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Maui.Core;

namespace Microsoft.Maui.Handlers.BoxView
{
	public interface IBoxViewHandler : IViewHandler
	{
		new IBoxView VirtualView { get; }
		new Gtk.Box PlatformView { get; }
	}
}
