using System;
using System.Linq;
using System.Threading.Tasks;
using Gtk;

namespace Microsoft.Maui.Platform
{
	public class MauiView : EventBox
	{
		private Gtk.Widget _child = null!;
		private Gtk.Entry _childEntry = null!;

		public MauiView()
		{
		}

		public void AddChildWidget(Gtk.Widget widget)
		{
			_child = widget;
			Add(_child);
		}

		public void AddChildEntry(Gtk.Entry entry)
		{
			_childEntry = entry;
		}

		public Gtk.Widget GetChildWidget()
		{
			return _child;
		}

		public Gtk.Entry GetChildEntry()
		{
			return _childEntry;
		}

		public void UpdateBackground(IView handler)
		{
		}

		public void UpdateWidth(IView handler)
		{
		}

		public void UpdateHeight(IView handler)
		{
		}

		public void UpdateMinimumHeight(IView handler)
		{
		}

		public void UpdateMaximumHeight(IView handler)
		{
		}

		public void UpdateMinimumWidth(IView handler)
		{
		}

		public void UpdateMaximumWidth(IView handler)
		{
		}

		public void UpdateIsEnabled(IView handler)
		{
		}

		public Task UpdateBackgroundImageSourceAsync(IImageSource? source, IImageSourceServiceProvider provider)
		{
			return Task.FromResult(0);
		}

		public void UpdateFlowDirection(IView handler)
		{
		}

		public void UpdateOpacity(IView handler)
		{
		}

		public void UpdateAutomationId(IView handler)
		{
		}

		public void UpdateClip(IView handler)
		{
		}

		public void UpdateShadow(IView handler)
		{
		}

		public void UpdateSemantics(IView handler)
		{
		}

		public void InvalidateMeasure(IView handler)
		{
		}

		public void UpdateBorder(IView handler)
		{
		}

		public void Focus(FocusRequest request)
		{
		}

		public void UpdateInputTransparent(IViewHandler handler, IView view)
		{
		}

		public void Unfocus(IView handler)
		{
		}

		public void SetBackgroundColor(Gdk.Color color)
		{
		}
	}
}