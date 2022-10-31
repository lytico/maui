using System.ComponentModel;

namespace Microsoft.Maui.Controls.Handlers.Compatibility
{
	public class ImageCellRenderer : TextCellRenderer
	{
		protected override System.Object GetCellCore(Cell item, System.Object parent)
		{
			var result = (BaseCellView)base.GetCellCore(item, parent);

			UpdateImage();
			UpdateFlowDirection();

			return result;
		}

		protected override void OnCellPropertyChanged(object sender, PropertyChangedEventArgs args)
		{
			base.OnCellPropertyChanged(sender, args);
			if (args.PropertyName == ImageCell.ImageSourceProperty.PropertyName)
				UpdateImage();
			else if (args.PropertyName == VisualElement.FlowDirectionProperty.PropertyName)
				UpdateFlowDirection();
		}

		void UpdateImage()
		{
			var cell = (ImageCell)Cell;
			if (cell.ImageSource != null)
			{
				View.SetImageVisible(true);
				View.SetImageSource(cell.ImageSource);
			}
			else
				View.SetImageVisible(false);
		}

		void UpdateFlowDirection()
		{
			View.UpdateFlowDirection(ParentView);
		}
	}
}