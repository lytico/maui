using System;

namespace Microsoft.Maui.Handlers
{
	public partial class SliderHandler : AbstractViewHandler<ISlider, Limaki.Extensions.SliderNativeView>
	{
		protected override Limaki.Extensions.SliderNativeView CreateNativeView() => throw new NotImplementedException();

		public static void MapMinimum(IViewHandler handler, ISlider slider) { }
		public static void MapMaximum(IViewHandler handler, ISlider slider) { }
		public static void MapValue(IViewHandler handler, ISlider slider) { }
		public static void MapMinimumTrackColor(IViewHandler handler, ISlider slider) { }
		public static void MapMaximumTrackColor(IViewHandler handler, ISlider slider) { }
		public static void MapThumbColor(IViewHandler handler, ISlider slider) { }
	}
}