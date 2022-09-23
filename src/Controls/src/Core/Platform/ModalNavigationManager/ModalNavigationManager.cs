#nullable enable

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Controls.Internals;

namespace Microsoft.Maui.Controls.Platform
{
	internal partial class ModalNavigationManager
	{
		Window _window;
		public IReadOnlyList<Page> ModalStack => _navModel.Modals;
		IMauiContext WindowMauiContext => _window.MauiContext;
		NavigationModel _navModel = new NavigationModel();
		NavigationModel? _previousNavModel = null;
		Page? _previousPage;

		public ModalNavigationManager(Window window)
		{
			_window = window;
		}

		public Task<Page> PopModalAsync()
		{
#if __GTK__
			return PopModalAsync();
#else
			return PopModalAsync(true);
#endif
		}


		public Task PushModalAsync(Page modal)
		{
#if __GTK__
			return PushModalAsync(modal);
#else
			return PushModalAsync(modal, true);
#endif
		}

		internal void SettingNewPage()
		{
			if (_previousPage != null)
			{
				// if _previousNavModel has been set than _navModel has already been reinitialized
				if (_previousNavModel != null)
				{
					_previousNavModel = null;
					if (_navModel == null)
						_navModel = new NavigationModel();
				}
				else
					_navModel = new NavigationModel();
			}

			if (_window.Page == null)
			{
				_previousPage = null;

				return;
			}

			_navModel.Push(_window.Page, null);
			_previousPage = _window.Page;
		}

		partial void OnPageAttachedHandler();

		public void PageAttachedHandler() => OnPageAttachedHandler();
	}
}
