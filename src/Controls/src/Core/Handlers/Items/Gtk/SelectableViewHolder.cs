#nullable disable
using System;
using System.Linq;
using Microsoft.Maui.Graphics;

namespace Microsoft.Maui.Controls.Handlers.Items
{

	public abstract class SelectableViewHolder : RecyclerView.ViewHolder
	{

		bool _isSelected;
		readonly bool _isSelectionEnabled;

		protected SelectableViewHolder(Gtk.Widget itemView, bool isSelectionEnabled = true) : base(itemView)
		{
			if (isSelectionEnabled)
				itemView.SetOnClickListener(this);

			_isSelectionEnabled = isSelectionEnabled;
		}

		public bool IsSelected
		{
			get => _isSelected;
			set
			{
				_isSelected = value;

				SetSelectionStates(_isSelected);

				ItemView.Sensitive = _isSelected;
				OnSelectedChanged();
			}
		}

		protected virtual bool UseDefaultSelectionColor => true;

		public void OnClick(Gtk.Widget view)
		{
			if (_isSelectionEnabled)
			{
				OnViewHolderClicked(BindingAdapterPosition);
			}
		}

		public event EventHandler<int> Clicked;

		protected virtual void OnSelectedChanged()
		{ }

		protected virtual void OnViewHolderClicked(int adapterPosition)
		{
			Clicked?.Invoke(this, adapterPosition);
		}

		void SetSelectionStates(bool isSelected)
		{
			if (!UseDefaultSelectionColor)
			{
				return;
			}

			ItemView.Sensitive = isSelected;

		}

	}

}