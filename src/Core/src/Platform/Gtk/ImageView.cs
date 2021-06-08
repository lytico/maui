namespace Microsoft.Maui
{

	public class ImageView : Gtk.Image
	{

		public Gdk.Pixbuf? Image
		{
			get => Pixbuf;
			set => Pixbuf = value;
		}

	}

}