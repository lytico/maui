﻿using System.Collections.Generic;

namespace Microsoft.Maui
{
	public partial class VisualDiagnosticsOverlay
	{
		readonly Dictionary<IScrollView, Gtk.Widget> _scrollViews = new();

		/// <inheritdoc/>
		public void AddScrollableElementHandler(IScrollView scrollBar)
		{
		}

		/// <inheritdoc/>
		public void RemoveScrollableElementHandler()
		{
		}
	}
}