using System;

namespace Microsoft.Maui.Platform
{
	internal class WindowRootViewContainer : Gtk.Window
	{
		Gtk.Window? _topPage;

		public WindowRootViewContainer() : base("No Title")
		{
			_topPage = this;
			Title = "No Title";
		}

		internal void AddPage(Gtk.Window pageView)
		{
			if (_topPage != pageView)
			{
				_topPage = pageView;
			}
			//Add(pageView);
			//if (!Children.Contains(pageView))
			//{
			//	int indexOFTopPage = 0;
			//	if (_topPage != null)
			//		indexOFTopPage = Children.IndexOf(_topPage) + 1;

			//	Children.Insert(indexOFTopPage, pageView);
			//	_topPage = pageView;
			//}
		}

		internal void RemovePage(Gtk.Window pageView)
		{
			//Remove(pageView);
			if (_topPage == pageView)
			{
				_topPage.Dispose();
				_topPage = null;
			}

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