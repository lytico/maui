using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Maui.Handlers
{
#if __GTK__
	public partial interface ITabbedViewHandler : IAltViewHandler
#else
	public partial interface ITabbedViewHandler : IViewHandler
#endif
	{
		new ITabbedView VirtualView { get; }
	}
}
