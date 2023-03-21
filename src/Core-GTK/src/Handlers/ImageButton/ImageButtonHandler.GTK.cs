using System;
using System.Diagnostics;
using Microsoft.Maui.Platform.GTK;

namespace Microsoft.Maui.Handlers
{
	public partial class ImageButtonHandler : ViewHandler<IImageButton, Gtk.Button>
	{
		protected override Gtk.Button CreatePlatformView(IView imageButton)
		{
			Gtk.Button platformView = null!;

			var _view = imageButton;
			//if ((_view is ITextButton virtualTextButton) && (_view is IImageButton virtualImageButton))
			if (_view is IImageButton virtualImageButton)
			{
				//_fontSize = Convert.ToInt32(virtualTextButton.Font.Size);
				//if (virtualTextButton.TextColor != null)
				//{
				//	var red = Convert.ToByte(virtualTextButton.TextColor.Red * 255);
				//	var green = Convert.ToByte(virtualTextButton.TextColor.Green * 255);
				//	var blue = Convert.ToByte(virtualTextButton.TextColor.Blue * 255);
				//	FontColor = new Gdk.Color(red, green, blue);
				//}
				//else if (virtualTextButton.StrokeColor != null)
				//{
				//	var red = Convert.ToByte(virtualTextButton.StrokeColor.Red * 255);
				//	var green = Convert.ToByte(virtualTextButton.StrokeColor.Green * 255);
				//	var blue = Convert.ToByte(virtualTextButton.StrokeColor.Blue * 255);
				//	FontColor = new Gdk.Color(red, green, blue);
				//}

				if (virtualImageButton.Source != null)
				{
					var fileImageSource = (IFileImageSource)virtualImageButton.Source;

					if (fileImageSource != null)
					{
						var imageElement = new Gtk.Image(fileImageSource.File);
						// imageElement.Show();
						platformView = new Gtk.Button();
						platformView.Image = imageElement;
						// Console.WriteLine("Image: " + fileImageSource.File);
						//if (string.IsNullOrEmpty(virtualTextButton.Text))
						//{
						//	imageElement = new Gtk.Image(fileImageSource.File);
						//	//Initialize(string.Empty, fileImageSource.File, string.Empty, name);
						//}
						//else
						//{
						//	Initialize(virtualTextButton.Text, fileImageSource.File, string.Empty, name);
						//}
						//return;
					}
				}
				//Initialize(virtualTextButton.Text, string.Empty, string.Empty, name);

				//return;
			}
			//else if (!(_view is ITextButton) && (_view is IImageButton virtualImageButtonButton))
			//{
			//	if (virtualImageButtonButton.Source != null)
			//	{
			//		var fileImageSource = (IFileImageSource)virtualImageButtonButton.Source;

			//		if (fileImageSource != null)
			//		{
			//			// Console.WriteLine("Image: " + fileImageSource.File);
			//			imageElement = new Gtk.Image(fileImageSource.File);
			//			// Initialize(string.Empty, fileImageSource.File, string.Empty, name);

			//			// return;
			//		}
			//	}
			//}

			if (platformView == null!) {
				return null!;
			}

			if ((_view != null) && (_view.Width > 0))
			{
				platformView.WidthRequest = (int)_view.Width;
			}
			if ((_view != null) && (_view.Height > 0))
			{
				platformView.HeightRequest = (int)_view.Height;
			}

			Gtk.Widget widget = platformView;
			SetMargins(imageButton, ref widget);

			if (imageButton is IImageButton imageView)
			{
				if (imageView.Visibility == Visibility.Visible)
				{
					platformView.Show();
				}
			}

			//Gtk.Widget widget = platformView;
			//SetMargins(imageButton, ref widget);

			//if (imageButton is IImageButton imageButtonView)
			//{
			//	if (imageButtonView.Visibility == Visibility.Visible)
			//	{
			//		platformView.Show();
			//	}
			//}

			return platformView;
		}

		void OnSetImageSource(Gdk.Pixbuf? obj)
		{
			Console.WriteLine("OnSetImageSource");
		}

		//protected override void DisconnectHandler(MauiGTKButton platformView)
		//{
		//	platformView.ButtonWidget.Clicked -= PlatformView_Clicked;

		//	base.DisconnectHandler(platformView);

		//	SourceLoader.Reset();
		//}

		//protected override void ConnectHandler(MauiGTKButton platformView)
		//{
		//	platformView.ButtonWidget.Clicked += PlatformView_Clicked;

		//	base.ConnectHandler(platformView);
		//}

		private protected override void OnConnectHandler(object platformView)
		{
			if (platformView is Gtk.Button imageButton)
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
			if (platformView is Gtk.Button button)
			{
				button.Clicked -= ButtonWidget_Clicked;
			}

			base.OnDisconnectHandler(platformView);
		}

		//private void PlatformView_Clicked(object? sender, EventArgs e)
		//{
		//	VirtualView?.Clicked();
		//}

		// TODO: NET7 make this public
		internal static void MapBackground(IImageButtonHandler handler, IImageButton imageButton)
		{
			//(handler.PlatformView as ShapeableImageView)?.UpdateBackground(imageButton);
		}

		public static void MapStrokeColor(IImageButtonHandler handler, IButtonStroke buttonStroke)
		{
			//(handler.PlatformView as ShapeableImageView)?.UpdateStrokeColor(buttonStroke);
		}

		public static void MapStrokeThickness(IImageButtonHandler handler, IButtonStroke buttonStroke)
		{
			//(handler.PlatformView as ShapeableImageView)?.UpdateStrokeThickness(buttonStroke);
		}

		public static void MapCornerRadius(IImageButtonHandler handler, IButtonStroke buttonStroke)
		{
			//(handler.PlatformView as ShapeableImageView)?.UpdateCornerRadius(buttonStroke);
		}

		public static void MapPadding(IImageButtonHandler handler, IImageButton imageButton)
		{
			//(handler.PlatformView as ShapeableImageView)?.UpdatePadding(imageButton);
		}

		public static void MapImageSource(IImageButtonHandler handler, IImageButton image)
		{
			if (handler.PlatformView is Gtk.Button buttonHandler)
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

		//void OnFocusChange(object? sender, View.FocusChangeEventArgs e)
		//{
		//	if (VirtualView != null)
		//		VirtualView.IsFocused = e.HasFocus;
		//}

		//void OnTouch(object? sender, View.TouchEventArgs e)
		//{
		//	var motionEvent = e.Event;
		//	switch (motionEvent?.ActionMasked)
		//	{
		//		case MotionEventActions.Down:
		//			VirtualView?.Pressed();
		//			break;
		//		case MotionEventActions.Up:
		//			VirtualView?.Released();
		//			break;
		//	}

		//	e.Handled = false;
		//}

		//void OnClick(object? sender, EventArgs e)
		//{
		//	VirtualView?.Clicked();
		//}
	}
}