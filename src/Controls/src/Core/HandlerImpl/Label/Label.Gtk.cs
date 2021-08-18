using Microsoft.Maui.Handlers;

namespace Microsoft.Maui.Controls
{

	public partial class Label
	{

		public static void RemapForControls()
		{
			IPropertyMapper<ILabel, LabelHandler> ControlsLabelMapper = new PropertyMapper<Label, LabelHandler>(LabelHandler.LabelMapper)
			{
				[nameof(TextType)] = MapTextType,
				[nameof(Text)] = MapText,
			};

			LabelHandler.LabelMapper = ControlsLabelMapper;
		}

		public static void MapTextType(LabelHandler handler, Label label)
		{
			handler.NativeView?.UpdateText(label, label.TextType);
		}

		public static void MapText(LabelHandler handler, Label label)
		{
			handler.NativeView?.UpdateText(label, label.TextType);
		}

	}

}