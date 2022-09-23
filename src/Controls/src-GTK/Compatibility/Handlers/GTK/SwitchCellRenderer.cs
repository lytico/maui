using System.ComponentModel;

namespace Microsoft.Maui.Controls.Handlers.Compatibility
{
	public class SwitchCellRenderer : CellRenderer
	{
		SwitchCellView _view;
		//Drawable _defaultTrackDrawable;

		protected override MauiView GetCellCore(Cell item, MauiView parent)
		{
			var cell = (SwitchCell)Cell;

			_view = new SwitchCellView(item);

			_view.Cell = cell;

			//var aSwitch = _view.AccessoryView as MauiSwitch;
			//if (aSwitch != null)
			//	_defaultTrackDrawable = aSwitch.TrackDrawable;

			UpdateText();
			UpdateChecked();
			UpdateHeight();
			UpdateIsEnabled(_view, cell);
			UpdateFlowDirection();
			UpdateOnColor(_view, cell);

			return _view;
		}

		protected override void OnCellPropertyChanged(object sender, PropertyChangedEventArgs args)
		{
			if (args.PropertyName == SwitchCell.TextProperty.PropertyName)
				UpdateText();
			else if (args.PropertyName == SwitchCell.OnProperty.PropertyName)
			{
				UpdateChecked();
				UpdateOnColor(_view, (SwitchCell)sender);
			}
			else if (args.PropertyName == "RenderHeight")
				UpdateHeight();
			else if (args.PropertyName == Cell.IsEnabledProperty.PropertyName)
				UpdateIsEnabled(_view, (SwitchCell)sender);
			else if (args.PropertyName == VisualElement.FlowDirectionProperty.PropertyName)
				UpdateFlowDirection();
			else if (args.PropertyName == SwitchCell.OnColorProperty.PropertyName)
				UpdateOnColor(_view, (SwitchCell)sender);
		}

		void UpdateChecked()
		{
			//((MauiSwitch)_view.AccessoryView).Checked = ((SwitchCell)Cell).On;
		}

		void UpdateIsEnabled(SwitchCellView cell, SwitchCell switchCell)
		{
			//cell.Enabled = switchCell.IsEnabled;
			//var aSwitch = cell.AccessoryView as ASwitch;
			//if (aSwitch != null)
			//	aSwitch.Enabled = switchCell.IsEnabled;
		}

		void UpdateFlowDirection()
		{
			_view.UpdateFlowDirection(ParentView);
		}

		void UpdateHeight()
		{
			_view.SetRenderHeight(Cell.RenderHeight);
		}

		void UpdateText()
		{
			_view.MainText = ((SwitchCell)Cell).Text;
		}

		void UpdateOnColor(SwitchCellView cell, SwitchCell switchCell)
		{
			//var aSwitch = cell.AccessoryView as MauiSwitch;
			//if (aSwitch != null)
			//{
			//	if (switchCell.On)
			//	{
			//		if (switchCell.OnColor == null)
			//		{
			//			aSwitch.TrackDrawable = _defaultTrackDrawable;
			//		}
			//		else
			//		{
			//			aSwitch.TrackDrawable.SetColorFilter(switchCell.OnColor, FilterMode.Multiply);
			//		}
			//	}
			//	else
			//	{
			//		aSwitch.TrackDrawable.ClearColorFilter();
			//	}
			//}
		}
	}
}