using System;

namespace Microsoft.Maui.Handlers
{
	public partial class TimePickerHandler : ViewHandler<ITimePicker, TimePicker>
	{
		TimePicker? _timePicker;

		protected override TimePicker CreatePlatformView(IView picker)
		{
			_timePicker = new TimePicker();

			Gtk.Widget widget = _timePicker;
			SetMargins(picker, ref widget);

			if (picker is ITimePicker pickerView)
			{
				if (pickerView.Visibility == Visibility.Visible)
				{
					_timePicker.Show();
				}
			}
			return _timePicker;
		}

		protected override void DisconnectHandler(TimePicker platformView)
		{
		}

		public static void MapBackground(ITimePickerHandler handler, ITimePicker timePicker)
		{
			//handler.PlatformView?.UpdateBackground(timePicker);
		}

		public static void MapFormat(ITimePickerHandler handler, ITimePicker timePicker)
		{
			//handler.PlatformView?.UpdateFormat(timePicker);
		}

		public static void MapTime(ITimePickerHandler handler, ITimePicker timePicker)
		{
			//handler.PlatformView?.UpdateTime(timePicker);
		}

		public static void MapCharacterSpacing(ITimePickerHandler handler, ITimePicker timePicker)
		{
			//handler.PlatformView?.UpdateCharacterSpacing(timePicker);
		}

		public static void MapFont(ITimePickerHandler handler, ITimePicker timePicker)
		{
			//var fontManager = handler.GetRequiredService<IFontManager>();

			//handler.PlatformView?.UpdateFont(timePicker, fontManager);
		}

		public static void MapTextColor(ITimePickerHandler handler, ITimePicker timePicker)
		{
			//handler.PlatformView?.UpdateTextColor(timePicker);
		}
	}
}