using System.Threading.Tasks;
using Microsoft.Maui.Graphics;

namespace Microsoft.Maui.Handlers
{
	public partial class ButtonHandler : ViewHandler<IButton, MauiImageButton>
	{
		public readonly static Thickness DefaultPadding = new Thickness(16, 8.5);

		//static ColorStateList TransparentColorStateList = Colors.Transparent.ToDefaultColorStateList();

		protected override MauiImageButton CreatePlatformView()
		{
			MauiImageButton platformButton = new MauiImageButton();

			return platformButton;
		}

		//protected override void ConnectHandler(Gtk.Button platformView)
		//{
		//	ClickListener.Handler = this;
		//	platformView.SetOnClickListener(ClickListener);

		//	TouchListener.Handler = this;
		//	platformView.SetOnTouchListener(TouchListener);

		//	platformView.FocusChange += OnNativeViewFocusChange;

		//	base.ConnectHandler(platformView);
		//}

		//protected override void DisconnectHandler(Gtk.Button platformView)
		//{
		//	ClickListener.Handler = null;
		//	platformView.SetOnClickListener(null);

		//	TouchListener.Handler = null;
		//	platformView.SetOnTouchListener(null);

		//	platformView.FocusChange -= OnNativeViewFocusChange;

		//	ImageSourceLoader.Reset();

		//	base.DisconnectHandler(platformView);
		//}

		// This is a Android-specific mapping
		public static void MapBackground(IButtonHandler handler, IButton button)
		{
			//handler.PlatformView?.UpdateBackground(button);
		}

		public static void MapStrokeColor(IButtonHandler handler, IButton button)
		{
			//handler.PlatformView?.UpdateStrokeColor(button);
		}

		public static void MapStrokeThickness(IButtonHandler handler, IButton button)
		{
			//handler.PlatformView?.UpdateStrokeThickness(button);
		}

		public static void MapCornerRadius(IButtonHandler handler, IButton button)
		{
			//handler.PlatformView?.UpdateCornerRadius(button);
		}

		public static void MapText(IButtonHandler handler, IText button)
		{
			// handler.PlatformView?.UpdateTextPlainText(button);
		}

		public static void MapTextColor(IButtonHandler handler, ITextStyle button)
		{
			// handler.PlatformView?.UpdateTextColor(button);
		}

		public static void MapCharacterSpacing(IButtonHandler handler, ITextStyle button)
		{
			// handler.PlatformView?.UpdateCharacterSpacing(button);
		}

		public static void MapFont(IButtonHandler handler, ITextStyle button)
		{
			// var fontManager = handler.GetRequiredService<IFontManager>();

			// handler.PlatformView?.UpdateFont(button, fontManager);
		}

		public static void MapPadding(IButtonHandler handler, IButton button)
		{
			// handler.PlatformView?.UpdatePadding(button, DefaultPadding);
		}

		public static void MapImageSource(IButtonHandler handler, IImage image) =>
			MapImageSourceAsync(handler, image).FireAndForget(handler);

		public static Task MapImageSourceAsync(IButtonHandler handler, IImage image)
		{
			return handler.ImageSourceLoader.UpdateImageSourceAsync();
		}

		void OnSetImageSource(Gdk.Pixbuf? obj)
		{
			// PlatformView.Icon = obj;
		}

		//bool NeedsExactMeasure()
		//{
		//	if (VirtualView.VerticalLayoutAlignment != Primitives.LayoutAlignment.Fill
		//		&& VirtualView.HorizontalLayoutAlignment != Primitives.LayoutAlignment.Fill)
		//	{
		//		// Layout Alignments of Start, Center, and End will be laying out the TextView at its measured size,
		//		// so we won't need another pass with MeasureSpecMode.Exactly
		//		return false;
		//	}

		//	if (VirtualView.Width >= 0 && VirtualView.Height >= 0)
		//	{
		//		// If the Width and Height are both explicit, then we've already done MeasureSpecMode.Exactly in 
		//		// both dimensions; no need to do it again
		//		return false;
		//	}

		//	// We're going to need a second measurement pass so TextView can properly handle alignments
		//	return true;
		//}

		//public override void PlatformArrange(Rect frame)
		//{
		//	var platformView = this.ToPlatform();

		//	if (platformView == null || Context == null)
		//	{
		//		return;
		//	}

		//	if (frame.Width < 0 || frame.Height < 0)
		//	{
		//		return;
		//	}

		//	// Depending on our layout situation, the TextView may need an additional measurement pass at the final size
		//	// in order to properly handle any TextAlignment properties.
		//	if (NeedsExactMeasure())
		//	{
		//		platformView.Measure(MakeMeasureSpecExact(frame.Width), MakeMeasureSpecExact(frame.Height));
		//	}

		//	base.PlatformArrange(frame);
		//}

		//void OnClick(IButton? button)
		//{
		//	button?.Clicked();
		//}
	}
}