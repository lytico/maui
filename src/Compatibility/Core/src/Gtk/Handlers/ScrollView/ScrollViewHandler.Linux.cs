using Gtk;
using Microsoft.Maui.Controls;

namespace Microsoft.Maui.Handlers.ScrollView
{

	public class ScrollViewHandler:ViewHandler<Controls.ScrollView,Gtk.Widget>
	{
		public static PropertyMapper<Controls.ScrollView, ScrollViewHandler> ScrollViewMapper = new(ViewHandler.ViewMapper)
		{
			// [nameof(ISlider.Maximum)] = MapMaximum,
			// [nameof(ISlider.MaximumTrackColor)] = MapMaximumTrackColor,
			// [nameof(ISlider.Minimum)] = MapMinimum,
			// [nameof(ISlider.MinimumTrackColor)] = MapMinimumTrackColor,
			// [nameof(ISlider.ThumbColor)] = MapThumbColor,
			// [nameof(ISlider.Value)] = MapValue,
		};

		public ScrollViewHandler() : base(ScrollViewMapper)
		{

		}

		public ScrollViewHandler(PropertyMapper mapper=null) : base(mapper) { }

		protected override Widget CreateNativeView()
		{
			throw new System.NotImplementedException();
		}

	}

}