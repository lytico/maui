#nullable disable

namespace Microsoft.Maui.Controls.Handlers.Items
{
	internal class TextViewHolder : SelectableViewHolder
	{
		public LabelView TextView { get; }

		public TextViewHolder(LabelView itemView, bool isSelectionEnabled = true) : base(itemView, isSelectionEnabled)
		{
			TextView = itemView;
			TextView.Selectable = isSelectionEnabled;
		}
	}
}