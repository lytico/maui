using System;
using Gtk;

namespace Microsoft.Maui.Handlers
{
	public partial class SliderHandler : ViewHandler<ISlider, MauiSlider>
	{
		protected override MauiSlider CreatePlatformView(IView slider)
		{
			double stepping = Math.Min((VirtualView.Maximum - VirtualView.Minimum) / 10, 1);

			var aSlider = new MauiSlider(VirtualView.Minimum, VirtualView.Maximum, stepping);
			aSlider.CustomValueChanged += ASlider_CustomValueChanged;

			Gtk.Widget widget = aSlider;
			SetMargins(slider, ref widget);

			return aSlider;
		}

		protected override void ConnectHandler(MauiSlider platformView)
		{
		}

		protected override void DisconnectHandler(MauiSlider platformView)
		{
		}

		public static void MapMinimum(ISliderHandler handler, ISlider slider)
		{
			handler.PlatformView?.UpdateMinimum(slider);
		}

		public static void MapMaximum(ISliderHandler handler, ISlider slider)
		{
			handler.PlatformView?.UpdateMaximum(slider);
		}

		public static void MapValue(ISliderHandler handler, ISlider slider)
		{
			handler.PlatformView?.UpdateValue(slider);
		}

		public static void MapMinimumTrackColor(ISliderHandler handler, ISlider slider)
		{
			//handler.PlatformView?.UpdateMinimumTrackColor(slider);
		}

		public static void MapMaximumTrackColor(ISliderHandler handler, ISlider slider)
		{
			//handler.PlatformView?.UpdateMaximumTrackColor(slider);
		}

		public static void MapThumbColor(ISliderHandler handler, ISlider slider)
		{
			//handler.PlatformView?.UpdateThumbColor(slider);
		}

		public static void MapThumbImageSource(ISliderHandler handler, ISlider slider)
		{
			//var provider = handler.GetRequiredService<IImageSourceServiceProvider>();

			//handler.PlatformView?.UpdateThumbImageSourceAsync(slider, provider)
			//	.FireAndForget(handler);
		}

		private void ASlider_CustomValueChanged(object? sender, MauiSliderEventArgs e)
		{
			if (VirtualView == null)
				return;

			var min = VirtualView.Minimum;
			var max = VirtualView.Maximum;

			var value = min + (max - min) * (e.CustomValue / int.MaxValue);

			VirtualView.Value = value;
		}

		void OnStartTrackingTouch(MauiSlider seekBar) =>
			VirtualView?.DragStarted();

		void OnStopTrackingTouch(MauiSlider seekBar) =>
			VirtualView?.DragCompleted();
	}
}