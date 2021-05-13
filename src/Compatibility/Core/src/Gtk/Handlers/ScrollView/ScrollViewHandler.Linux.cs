using System;
using Gtk;
using Microsoft.Maui.Controls;

namespace Microsoft.Maui.Handlers.ScrollView
{

	public class ScrollViewHandler:ViewHandler<Controls.ScrollView,Gtk.ScrolledWindow>
	{
		public static PropertyMapper<Controls.ScrollView, ScrollViewHandler> ScrollViewMapper = new(ViewHandler.ViewMapper)
		{

		};

		public ScrollViewHandler() : base(ScrollViewMapper)
		{

		}

		public ScrollViewHandler(PropertyMapper mapper=null) : base(mapper) { }

		protected override ScrolledWindow CreateNativeView()
		{
			if (VirtualView == null)
				throw new ArgumentException();
			
			var s = new ScrolledWindow();
			s.Child = VirtualView.Content.ToNative(MauiContext);
			return s;
		}

	}

}