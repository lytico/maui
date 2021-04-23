using Cairo;
using Gtk;
using Rectangle = Gdk.Rectangle;

namespace Microsoft.Maui
{
	public class LayoutView : Fixed
	{
#if DEBUG
		protected override bool OnDrawn(Context cr)
		{
			var r = base.OnDrawn(cr);

			cr.Save();
			cr.ShowText($"{nameof(LayoutView)}");
			cr.Restore();
			return r;
		}
#endif
		protected override void OnAdjustSizeRequest(Orientation orientation, out int minimum_size, out int natural_size)
		{
			base.OnAdjustSizeRequest(orientation, out minimum_size, out natural_size);
		}

		protected override void OnSizeAllocated(Rectangle allocation)
		{
			base.OnSizeAllocated(allocation);
	
		}
		
		
	}
}