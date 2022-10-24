using Gdk;
using Gtk;

namespace Microsoft.Maui.Platform
{
	public class ContentViewGroup : System.Object
	{
		EventBox _box;
		IBorderStroke? _clip;

		public ContentViewGroup()
		{
			_box = new EventBox();
			//Add(_fixed);
		}

		public void AddChild(System.Object widget)
		{
			_box.Add((Gtk.Widget)widget);
			//_box.Put((Gtk.Widget)widget, 0, 0);
			//_fixed.Add((Gtk.Widget)widget);
		}

		public Gtk.EventBox GetView()
		{
			return _box;
		}

		//protected override bool OnExposeEvent(EventExpose evnt)
		//{
		//	using (var cr = CairoHelper.Create(GdkWindow))
		//	{
		//		if (Clip != null)
		//			ClipChild(cr);
		//	}

		//	return base.OnExposeEvent(evnt);
		//}

		internal IBorderStroke? Clip
		{
			get => _clip;
			set
			{
				_clip = value;
			}
		}

		//void ClipChild(Cairo.Context? cr)
		//{
		//	if (Clip == null || cr == null)
		//		return;

		//	float strokeThickness = (float)Clip.StrokeThickness;
		//	float offset = strokeThickness / 2;
		//	float w = Allocation.Width - strokeThickness;
		//	float h = Allocation.Height - strokeThickness;

		//	// Draw Background
		//	cr.Rectangle(offset, offset, w, h);
		//	cr.FillPreserve();

		//	// Draw Border
		//	cr.Rectangle(offset, offset, w, h);
		//	cr.StrokePreserve();
		//}
	}
}