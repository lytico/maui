using System;
using System.Collections.Generic;
using System.Text;
using Gtk;
using Microsoft.Maui.Core;

namespace Microsoft.Maui.Handlers.BoxView
{
	public class BoxViewHandler : ViewHandler<IBoxView, Gtk.Box>, IBoxViewHandler
	{
		public static IPropertyMapper<IBoxView, IBoxViewHandler> Mapper = new PropertyMapper<IBoxView, IBoxViewHandler>(ViewHandler.ViewMapper)
		{
		};

		public static CommandMapper<IBoxView, BoxViewHandler> CommandMapper = new(ViewCommandMapper)
		{
		};

		public BoxViewHandler() : base(Mapper, CommandMapper)
		{
		}

		IBoxView IBoxViewHandler.VirtualView => VirtualView;

		Gtk.Box IBoxViewHandler.PlatformView => PlatformView;

		protected override Gtk.Box CreatePlatformView(IView boxView)
		{
			var plat = new Gtk.Box(Gtk.Orientation.Vertical, 0);

			Gtk.Widget widget = plat;
			SetMargins(boxView, ref widget);

			plat.Show();

			return plat;
		}

		protected override void ConnectHandler(Gtk.Box platformView)
		{
		}

		protected override void DisconnectHandler(Gtk.Box platformView)
		{
		}
	}
}
