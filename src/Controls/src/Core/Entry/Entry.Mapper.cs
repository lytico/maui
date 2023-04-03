#nullable disable
namespace Microsoft.Maui.Controls
{
	public partial class Entry
	{
		public static IPropertyMapper<IEntry, EntryHandler> ControlsEntryMapper =
			new PropertyMapper<Entry, EntryHandler>(EntryHandler.Mapper)
			{
#if ANDROID
				[PlatformConfiguration.AndroidSpecific.Entry.ImeOptionsProperty.PropertyName] = MapImeOptions,
#elif WINDOWS && !__GTK__
				[PlatformConfiguration.WindowsSpecific.InputView.DetectReadingOrderFromContentProperty.PropertyName] = MapDetectReadingOrderFromContent,
#elif IOS
				[PlatformConfiguration.iOSSpecific.Entry.CursorColorProperty.PropertyName] = MapCursorColor,
				[PlatformConfiguration.iOSSpecific.Entry.AdjustsFontSizeToFitWidthProperty.PropertyName] = MapAdjustsFontSizeToFitWidth,
#endif
#if !__GTK__
				[nameof(Text)] = MapText,
				[nameof(TextTransform)] = MapText,
#endif
			};

		static CommandMapper<IEntry, IEntryHandler> ControlsCommandMapper = new(EntryHandler.CommandMapper)
		{
#if ANDROID
			[nameof(IEntry.Focus)] = MapFocus
#endif
		};

		internal static new void RemapForControls()
		{
			// Adjust the mappings to preserve Controls.Entry legacy behaviors
			EntryHandler.Mapper = ControlsEntryMapper;
			EntryHandler.CommandMapper = ControlsCommandMapper;
		}
	}
}
