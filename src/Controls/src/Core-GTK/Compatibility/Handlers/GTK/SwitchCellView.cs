#nullable disable
namespace Microsoft.Maui.Controls.Handlers.Compatibility
{
	public class SwitchCellView : BaseCellView
	{
		public SwitchCellView(Cell cell) : base(cell)
		{
			var sw = new MauiSwitch();
			//sw.SetOnCheckedChangeListener(this);

			SetAccessoryView(sw);

			SetImageVisible(false);
		}

		public SwitchCell Cell { get; set; }

		//public void OnCheckedChanged(CompoundButton buttonView, bool isChecked)
		//{
		//	Cell.On = isChecked;
		//}
	}
}