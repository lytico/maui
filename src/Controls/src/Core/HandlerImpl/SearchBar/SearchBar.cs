#nullable disable
namespace Microsoft.Maui.Controls
{
	public partial class SearchBar
	{
		public static IPropertyMapper<ISearchBar, SearchBarHandler> ControlsSearchBarMapper =
			new PropertyMapper<SearchBar, SearchBarHandler>(SearchBarHandler.Mapper)
			{
#if WINDOWS && !__GTK__
				[PlatformConfiguration.WindowsSpecific.SearchBar.IsSpellCheckEnabledProperty.PropertyName] = MapIsSpellCheckEnabled,
#elif IOS
				[PlatformConfiguration.iOSSpecific.SearchBar.SearchBarStyleProperty.PropertyName] = MapSearchBarStyle,
#endif
#if !__GTK__
				[nameof(Text)] = MapText,
				[nameof(TextTransform)] = MapText,
#endif
			};

		static CommandMapper<ISearchBar, ISearchBarHandler> ControlsCommandMapper = new(SearchBarHandler.CommandMapper)
		{
#if ANDROID
			[nameof(ISearchBar.Focus)] = MapFocus
#endif
		};

		internal static new void RemapForControls()
		{
			// Adjust the mappings to preserve Controls.SearchBar legacy behaviors
			SearchBarHandler.Mapper = ControlsSearchBarMapper;
			SearchBarHandler.CommandMapper = ControlsCommandMapper;
		}
	}
}
