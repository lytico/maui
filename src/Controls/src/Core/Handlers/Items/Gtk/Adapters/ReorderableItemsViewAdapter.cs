﻿#nullable disable
using System;
using System.Collections;


namespace Microsoft.Maui.Controls.Handlers.Items
{
	public class ReorderableItemsViewAdapter<TItemsView, TItemsViewSource> : GroupableItemsViewAdapter<TItemsView, TItemsViewSource>, IItemTouchHelperAdapter
		where TItemsView : ReorderableItemsView
		where TItemsViewSource : IGroupableItemsViewSource
	{
		public ReorderableItemsViewAdapter(TItemsView reorderableItemsView,
			Func<View, ItemContentView> createView = null) : base(reorderableItemsView, createView)
		{
		}

		public bool OnItemMove(int fromPosition, int toPosition)
		{
			var itemsSource = ItemsSource;
			var itemsView = ItemsView;

			if (itemsSource == null || itemsView == null)
			{
				return false;
			}

			if (itemsView.IsGrouped)
			{
				var (fromGroupIndex, fromIndex) = itemsSource.GetGroupAndIndex(fromPosition);
				var fromList = itemsSource.GetGroup(fromGroupIndex) as IList;
				var fromItemsSource = itemsSource.GetGroupItemsViewSource(fromGroupIndex);
				var fromItemIndex = fromIndex - (fromItemsSource?.HasHeader == true ? 1 : 0);

				var (toGroupIndex, toIndex) = itemsSource.GetGroupAndIndex(toPosition);
				var toList = itemsSource.GetGroup(toGroupIndex) as IList;
				var toItemsSource = itemsSource.GetGroupItemsViewSource(toGroupIndex);
				var toItemIndex = toIndex - (toItemsSource?.HasHeader == true ? 1 : 0);

				if (toGroupIndex > fromGroupIndex)
				{
					toItemIndex = toItemIndex + 1;
				}

				if (toGroupIndex != fromGroupIndex && !itemsView.CanMixGroups)
				{
					return false;
				}

				if (fromList != null && toList != null)
				{
					var fromItem = fromList[fromItemIndex];
					SetObserveChanges(fromItemsSource, false);
					SetObserveChanges(toItemsSource, false);
					fromList.RemoveAt(fromItemIndex);
					toList.Insert(toItemIndex, fromItem);
					NotifyItemMoved(fromPosition, toPosition);
					SetObserveChanges(fromItemsSource, true);
					SetObserveChanges(toItemsSource, true);
					itemsView.SendReorderCompleted();
					return true;
				}
			}
			else if (itemsView.ItemsSource is IList list)
			{
				var fromItem = list[fromPosition];
				SetObserveChanges(itemsSource, false);
				list.RemoveAt(fromPosition);
				list.Insert(toPosition, fromItem);
				NotifyItemMoved(fromPosition, toPosition);
				SetObserveChanges(itemsSource, true);
				itemsView.SendReorderCompleted();
				return true;
			}
			return false;
		}

		void SetObserveChanges(IItemsViewSource itemsSource, bool enable)
		{
			if (itemsSource is IObservableItemsViewSource observableSource)
			{
				observableSource.ObserveChanges = enable;
			}
		}
	}
}