using System;
using Microsoft.Maui.Graphics.Win2D;

namespace Microsoft.Maui.Platform
{
	internal class WindowRootViewContainer : Gtk.EventBox
	{
		// Gtk.Fixed? _topPage;

		internal void AddPage(Gtk.Widget pageView)
		{
			Add(pageView);
			//if (!Children.Contains(pageView))
			//{
			//	int indexOFTopPage = 0;
			//	if (_topPage != null)
			//		indexOFTopPage = Children.IndexOf(_topPage) + 1;

			//	Children.Insert(indexOFTopPage, pageView);
			//	_topPage = pageView;
			//}
		}

		internal void RemovePage(Gtk.Widget pageView)
		{
			Remove(pageView);
			//int indexOFTopPage = -1;
			//if (_topPage != null)
			//	indexOFTopPage = Children.IndexOf(_topPage) - 1;

			//Children.Remove(pageView);

			//if (indexOFTopPage >= 0)
			//	_topPage = (FrameworkElement)Children[indexOFTopPage];
			//else
			//	_topPage = null;
		}

		//internal void AddOverlay(FrameworkElement overlayView)
		//{
		//	if (!Children.Contains(overlayView))
		//		Children.Add(overlayView);
		//}

		//internal void RemoveOverlay(FrameworkElement overlayView)
		//{
		//	Children.Remove(overlayView);
		//}
	}
}