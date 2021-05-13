using System;

namespace Microsoft.Maui.Handlers
{
	public partial class PageHandler : ViewHandler<IPage, Gtk.Widget>
	{
		protected override Gtk.Widget CreateNativeView() => throw new NotImplementedException();

		[MissingMapper]
		public static void MapTitle(PageHandler handler, IPage page)
		{
		}
	}
}
