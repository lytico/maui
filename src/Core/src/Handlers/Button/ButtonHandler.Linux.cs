using System;
using Gtk;
using Microsoft.Maui.Graphics.Native.Gtk;

namespace Microsoft.Maui.Handlers
{

	public partial class ButtonHandler : ViewHandler<IButton, Button>
	{

		protected override Button CreateNativeView()
		{
			return new Button();
		}

		protected override void ConnectHandler(Button nativeView)
		{
			nativeView.Clicked += OnButtonClicked;
			nativeView.ButtonPressEvent += OnButtonPressEvent;
			nativeView.ButtonReleaseEvent += OnButtonReleaseEvent;
		}

		protected override void DisconnectHandler(Button nativeView)
		{
			nativeView.Clicked -= OnButtonClicked;
			nativeView.ButtonPressEvent -= OnButtonPressEvent;
			nativeView.ButtonReleaseEvent -= OnButtonReleaseEvent;
		}

		public static void MapText(ButtonHandler handler, IButton button)
		{
			handler.NativeView?.UpdateText(button);
		}

		public static void MapTextColor(ButtonHandler handler, IButton button)
		{
			if (handler?.NativeView is { } nativeView)
			{
				nativeView.SetForegroundColor(button.TextColor);
			    nativeView.SetForegroundColor(Gtk.StateFlags.Prelight, button.TextColor);
			}

		}

		[MissingMapper]
		public static void MapCharacterSpacing(ButtonHandler handler, IButton button) { }

		[MissingMapper]
		public static void MapFont(ButtonHandler handler, IButton button) { }

		public static void MapPadding(ButtonHandler handler, IButton button)
		{
			handler.NativeView.WithMargin(button.Padding);
		}

		void OnButtonPressEvent(object? o, ButtonPressEventArgs args)
		{
			VirtualView?.Pressed();
		}

		void OnButtonReleaseEvent(object? o, ButtonReleaseEventArgs args)
		{
			VirtualView?.Released();
		}

		void OnButtonClicked(object? sender, EventArgs e)
		{
			VirtualView?.Clicked();
		}

	}

}