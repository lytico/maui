using Gdk;
using Gtk;
using Microsoft.UI.Xaml.Controls;

namespace Microsoft.Maui.Platform
{
	public class ContentViewGroup : MauiView
	{
		Fixed _fixed;
		IBorderStroke? _clip;

		public ContentViewGroup()
		{
			_fixed = new Fixed();
			Add(_fixed);
		}

		protected override bool OnExposeEvent(EventExpose evnt)
		{
			using (var cr = CairoHelper.Create(GdkWindow))
			{
				if (Clip != null)
					ClipChild(cr);
			}

			return base.OnExposeEvent(evnt);
		}

		internal IBorderStroke? Clip
		{
			get => _clip;
			set
			{
				_clip = value;
			}
		}

		void ClipChild(Cairo.Context? cr)
		{
			if (Clip == null || cr == null)
				return;

			float strokeThickness = (float)Clip.StrokeThickness;
			float offset = strokeThickness / 2;
			float w = Allocation.Width - strokeThickness;
			float h = Allocation.Height - strokeThickness;

			// Draw Background
			cr.Rectangle(offset, offset, w, h);
			cr.FillPreserve();

			// Draw Border
			cr.Rectangle(offset, offset, w, h);
			cr.StrokePreserve();
		}
	}
}