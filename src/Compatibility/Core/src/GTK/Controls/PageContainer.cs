namespace Microsoft.Maui.Controls.Compatibility.Platform.GTK.Controls
{
	public class PageContainer : Gtk.Object
	{
		public PageContainer(Microsoft.Maui.Controls.Page element, int index)
		{
			Page = element;
			Index = index;
		}

		public Microsoft.Maui.Controls.Page Page { get; }

		public int Index { get; set; }
	}
}
