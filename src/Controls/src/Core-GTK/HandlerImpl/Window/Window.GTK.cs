using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Maui.Handlers;

namespace Microsoft.Maui.Controls
{
	public partial class Window
	{
		internal MauiGTKWindow NativeWindow =>
			(Handler?.PlatformView as MauiGTKWindow) ?? throw new InvalidOperationException("Window Handler should have a Window set.");
	}
}
