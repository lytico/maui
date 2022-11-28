namespace Microsoft.Maui.Handlers
{
	public partial class RadioButtonHandler : ViewHandler<IRadioButton, MauiRadioButton>
	{
		static MauiRadioButton? GetPlatformRadioButton(IRadioButtonHandler handler) => handler.PlatformView as MauiRadioButton;

		protected override MauiRadioButton CreatePlatformView(IView radioButton)
		{
			return new MauiRadioButton();
		}

		protected override void ConnectHandler(MauiRadioButton platformView)
		{
			//RadioButton? platformRadioButton = GetPlatformRadioButton(this);
			//if (platformRadioButton != null)
			//	platformRadioButton.cli
			//	platformRadioButton.CheckedChange += OnCheckChanged;
		}

		protected override void DisconnectHandler(MauiRadioButton platformView)
		{
			//if (platformView is AppCompatRadioButton platformRadioButton)
			//	platformRadioButton.CheckedChange -= OnCheckChanged;
		}

		public static void MapBackground(IRadioButtonHandler handler, IRadioButton radioButton)
		{
			//GetPlatformRadioButton(handler)?.UpdateBackground(radioButton);
		}

		public static void MapIsChecked(IRadioButtonHandler handler, IRadioButton radioButton)
		{
			//GetPlatformRadioButton(handler)?.UpdateIsChecked(radioButton);
		}

		public static void MapContent(IRadioButtonHandler handler, IRadioButton radioButton)
		{
			//GetPlatformRadioButton(handler)?.UpdateContent(radioButton);
		}

		public static void MapTextColor(IRadioButtonHandler handler, ITextStyle textStyle)
		{
			//GetPlatformRadioButton(handler)?.UpdateTextColor(textStyle);
		}

		public static void MapCharacterSpacing(IRadioButtonHandler handler, ITextStyle textStyle)
		{
			//GetPlatformRadioButton(handler)?.UpdateCharacterSpacing(textStyle);
		}

		public static void MapFont(IRadioButtonHandler handler, ITextStyle textStyle)
		{
			//var fontManager = handler.GetRequiredService<IFontManager>();

			//GetPlatformRadioButton(handler)?.UpdateFont(textStyle, fontManager);
		}

		public static void MapStrokeColor(IRadioButtonHandler handler, IRadioButton radioButton)
		{
			//GetPlatformRadioButton(handler)?.UpdateStrokeColor(radioButton);
		}

		public static void MapStrokeThickness(IRadioButtonHandler handler, IRadioButton radioButton)
		{
			//GetPlatformRadioButton(handler)?.UpdateStrokeThickness(radioButton);
		}

		public static void MapCornerRadius(IRadioButtonHandler handler, IRadioButton radioButton)
		{
			//GetPlatformRadioButton(handler)?.UpdateCornerRadius(radioButton);
		}

		//void OnCheckChanged(object? sender, CompoundButton.CheckedChangeEventArgs e)
		//{
		//	if (VirtualView == null)
		//		return;

		//	VirtualView.IsChecked = e.IsChecked;
		//}
	}
}