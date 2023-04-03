#nullable disable
namespace Microsoft.Maui.Controls
{
	public partial class Editor
	{
		public static IPropertyMapper<IEditor, EditorHandler> ControlsEditorMapper =
			new PropertyMapper<Editor, EditorHandler>(EditorHandler.Mapper)
			{
#if WINDOWS && !__GTK__
				[PlatformConfiguration.WindowsSpecific.InputView.DetectReadingOrderFromContentProperty.PropertyName] = MapDetectReadingOrderFromContent,
#endif
#if !__GTK__
				[nameof(Text)] = MapText,
				[nameof(TextTransform)] = MapText,
#endif
			};

		static CommandMapper<IEditor, IEditorHandler> ControlsCommandMapper = new(EditorHandler.CommandMapper)
		{
#if ANDROID
			[nameof(IEditor.Focus)] = MapFocus
#endif
		};

		internal static new void RemapForControls()
		{
			// Adjust the mappings to preserve Controls.Editor legacy behaviors
			EditorHandler.Mapper = ControlsEditorMapper;
			EditorHandler.CommandMapper = ControlsCommandMapper;
		}
	}
}