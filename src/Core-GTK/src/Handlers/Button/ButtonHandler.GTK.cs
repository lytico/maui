using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using GLib;
using Gtk;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Platform.GTK;

namespace Microsoft.Maui.Handlers
{
	public partial class ButtonHandler : ViewHandler<IButton, MauiGTKButton>
	{
		public readonly static Thickness DefaultPadding = new Thickness(16, 8.5);

		//static ColorStateList TransparentColorStateList = Colors.Transparent.ToDefaultColorStateList();

		protected override MauiGTKButton CreatePlatformView(IView button)
		{
			MauiGTKButton platformButton = new MauiGTKButton(button);

			Gtk.Widget widget = platformButton;
			SetMargins(button, ref widget);

			platformButton.Show();

			return platformButton;
		}

		private protected override void OnConnectHandler(object platformView)
		{
			if (platformView is MauiGTKButton imageButton)
			{
				//imageButton.ButtonWidget.Clicked += ButtonWidget_Clicked;
				imageButton.Clicked += ButtonWidget_Clicked;
			}

			base.OnConnectHandler(platformView);
		}

		private void ButtonWidget_Clicked(object? sender, System.EventArgs e)
		{
			// Debug.WriteLine("Clicked");
			VirtualView?.Clicked();
		}

		private protected override void OnDisconnectHandler(object platformView)
		{
			if (platformView is MauiGTKButton button)
			{
				button.Clicked -= ButtonWidget_Clicked;
			}

			base.OnDisconnectHandler(platformView);
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
			// handler.PlatformView?.UpdateBackground(button);
			if (handler.PlatformView != null && button.Background != null)
			{
				if (button.Background.ToColor() != null)
				{
					byte r, g, b;
					r = 0;
					g = 0;
					b = 0;
					button.Background.ToColor()?.ToRgb(out r, out g, out b);
					if (r != 0 && g != 0 && b != 0)
					{
						handler.PlatformView.SetBackgroundColor(new Gdk.Color(r, g, b));
					}
				}
			}
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
			if (handler.PlatformView is MauiGTKButton handlerBox)
			{
				// need to attach Attributes after setting text again, so get it ...
				var attrs = handlerBox.GetInternalLabel.Attributes;
				// var attrs = handlerBox.LabelWidget.Attributes;

				// handlerBox.ButtonWidget.Label = button.Text;

				handlerBox.Label = button.Text;
				//handlerBox.LabelWidget.Text = button.Text;
				MauiGTKText.SetMarkupAttributes(handler, button, button.Text);
				handlerBox.GetInternalLabel.Attributes = attrs;

				// handlerBox.SetBackgroundColor(new Gdk.Color(200, 0, 200));
				// handlerBox.ModifyBg(Gtk.StateType.Normal, new Gdk.Color(200, 0, 200));
				//if (handlerBox.Children.Length > 0)
				//{
				//	if (handlerBox.Children[0] is Gtk.Label childLabel)
				//	{
				//		childLabel.Text = label.Text;
				//	}
				//}
			}
		}

		public static void MapTextColor(IButtonHandler handler, ITextStyle button)
		{
			// handler.PlatformView?.UpdateTextColor(button);
			MauiGTKText.SetMarkupAttributes(handler, button);
		}


		public static void MapCharacterSpacing(IButtonHandler handler, ITextStyle button)
		{
			// handler.PlatformView?.UpdateCharacterSpacing(button);
		}

		public static void MapFont(IButtonHandler handler, ITextStyle button)
		{
			// label.set_markup("<span font_desc='unifont medium'>%s</span>" % 'some text')
			MauiGTKText.SetMarkupAttributes(handler, button);

			//if (handler.PlatformView is MauiImageButton handlerBox)
			//{
			//	Pango.FontDescription fontdesc = new Pango.FontDescription();
			//	fontdesc.Family = button.Font.Family;
			//	fontdesc.Size = (int)(button.Font.Size * Pango.Scale.PangoScale);

			//	var style = new Gtk.RcStyle();
			//	style.FontDesc = fontdesc;

			//	// handlerBox.ModifyStyle(style);
			//	handlerBox.LabelWidget.ModifyStyle(style);
			//	//handlerBox.LabelWidget.ModifyBg(Gtk.StateType.Normal, new Gdk.Color(200, 0, 200));
			//	//handlerBox.BoxWidget.ModifyBg(Gtk.StateType.Normal, new Gdk.Color(200, 0, 200));
			//	// handlerBox.SetBackgroundColor(new Gdk.Color(200, 0, 200));
			//	// handlerBox.ModifyBg(Gtk.StateType.Normal, new Gdk.Color(200, 0, 200));
			//	//if (handlerBox.ButtonWidget.Child is Gtk.Alignment alignChild)
			//	//{
			//	//	if (alignChild.Children.Length > 0)
			//	//	{
			//	//		if (alignChild.Children[0] is Gtk.HBox horizBox)
			//	//		{
			//	//			if (horizBox.Children.Length > 1)
			//	//			{
			//	//				if (horizBox.Children[0] is Gtk.Label label0Child)
			//	//				{
			//	//					label0Child.ModifyFont(fontdesc);
			//	//				}
			//	//				if (horizBox.Children[1] is Gtk.Label label1Child)
			//	//				{
			//	//					//label1Child.ModifyFont(fontdesc);
			//	//					label1Child.Style.FontDesc.Family = button.Font.Family;
			//	//					label1Child.Style.FontDesc.Size = (int)(button.Font.Size * Pango.Scale.PangoScale);
			//	//				}
			//	//			}
			//	//		}
			//	//	}
			//	//}

			//	//handlerBox.ButtonWidget.ModifyFont(fontdesc);
			//	//handlerBox.LabelWidget.ModifyFont(fontdesc);
			//}

			// var fontManager = handler.GetRequiredService<IFontManager>();

			// handler.PlatformView?.UpdateFont(button, fontManager);
		}

		public static void MapPadding(IButtonHandler handler, IButton button)
		{
			// handler.PlatformView?.UpdatePadding(button, DefaultPadding);
		}

		//public static void MapImageSource(IButtonHandler handler, IImage image) =>
		//	MapImageSourceAsync(handler, image).FireAndForget(handler);

		//public static Task MapImageSourceAsync(IButtonHandler handler, IImage image)
		//{
		//	return handler.ImageSourceLoader.UpdateImageSourceAsync();
		//}

		//public static void MapImageSource(IButtonHandler handler, string source)
		//{
		//	if (handler.PlatformView is MauiGTKButton buttonHandler)
		//	{
		//		buttonHandler.Image = new Gtk.Image(source);
		//	}
		//}

		public static void MapImageSource(IButtonHandler handler, IImage image)
		{
			if (handler.PlatformView is MauiGTKButton buttonHandler)
			{
				// buttonHandler.Image = new Gtk.Image(source);
				if (image.Source == null)
				{
					buttonHandler.Image = null!;
				}
				else
				{
					var fileImageSource = (IFileImageSource)image.Source;

					if (fileImageSource != null)
					{
						// Console.WriteLine("MapImageSource Image: " + fileImageSource.File);
						buttonHandler.Image = new Gtk.Image(fileImageSource.File);
					}
				}
			}
		}

		void OnSetImageSource(Gdk.Pixbuf? obj)
		{
			// PlatformView.Icon = obj;
			PlatformView.Image = new Gtk.Image(obj);
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