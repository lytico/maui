﻿#nullable disable
using System;
using static Microsoft.Maui.Controls.Handlers.Items.RecyclerView;

namespace Microsoft.Maui.Controls.Handlers.Items
{
	internal class DataChangeObserver : RecyclerView.AdapterDataObserver
	{
		readonly Action _onDataChange;

		public bool Observing { get; private set; }

		public DataChangeObserver(Action onDataChange) : base()
		{
			_onDataChange = onDataChange;
		}

		public void Start(Adapter adapter)
		{
			if (Observing)
			{
				return;
			}

			adapter.RegisterAdapterDataObserver(this);

			Observing = true;
		}

		public void Stop(Adapter adapter)
		{
			if (Observing && IsValidAdapter(adapter))
			{
				adapter.UnregisterAdapterDataObserver(this);
			}

			Observing = false;
		}

		public override void OnChanged()
		{
			base.OnChanged();
			_onDataChange?.Invoke();
		}

		public override void OnItemRangeInserted(int positionStart, int itemCount)
		{
			base.OnItemRangeInserted(positionStart, itemCount);
			_onDataChange?.Invoke();
		}

		public override void OnItemRangeChanged(int positionStart, int itemCount)
		{
			base.OnItemRangeChanged(positionStart, itemCount);
			_onDataChange?.Invoke();
		}

		public override void OnItemRangeChanged(int positionStart, int itemCount, object payload)
		{
			base.OnItemRangeChanged(positionStart, itemCount, payload);
			_onDataChange?.Invoke();
		}

		public override void OnItemRangeRemoved(int positionStart, int itemCount)
		{
			base.OnItemRangeRemoved(positionStart, itemCount);
			_onDataChange?.Invoke();
		}

		public override void OnItemRangeMoved(int fromPosition, int toPosition, int itemCount)
		{
			base.OnItemRangeMoved(fromPosition, toPosition, itemCount);
			_onDataChange?.Invoke();
		}

		bool IsValidAdapter(Adapter adapter)
		{
			return adapter != null && adapter.HasObservers;
		}
	}
}