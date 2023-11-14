using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Maui.Handlers;
using TCollectionView = Gtk.UIExtensions.NUI.CollectionView;

namespace Microsoft.Maui.Controls.Handlers.Items
{
	public abstract partial class ItemsViewHandler<TItemsView> : ViewHandler<TItemsView, TCollectionView> where TItemsView : ItemsView
	{
		protected override TCollectionView CreatePlatformView()
		{
			return new();
		}

		[MissingMapper]
		public static void MapItemsSource(ItemsViewHandler<TItemsView> handler, ItemsView itemsView)
		{
			;
		}

		[MissingMapper]
		public static void MapHorizontalScrollBarVisibility(ItemsViewHandler<TItemsView> handler, ItemsView itemsView)
		{
		}

		[MissingMapper]
		public static void MapVerticalScrollBarVisibility(ItemsViewHandler<TItemsView> handler, ItemsView itemsView)
		{
		}

		[MissingMapper]
		public static void MapItemTemplate(ItemsViewHandler<TItemsView> handler, ItemsView itemsView)
		{
			var template = itemsView.ItemTemplate;
			var rview = template.CreateContent();

			if (rview is IView view)
			{
				if (handler.MauiContext != null && view.ToPlatform(handler.MauiContext) is { } p)
				{
					;
				}
			}
		}

		[MissingMapper]
		public static void MapEmptyView(ItemsViewHandler<TItemsView> handler, ItemsView itemsView)
		{
			;
		}

		[MissingMapper]
		public static void MapEmptyViewTemplate(ItemsViewHandler<TItemsView> handler, ItemsView itemsView)
		{
			;
		}

		[MissingMapper]
		public static void MapFlowDirection(ItemsViewHandler<TItemsView> handler, ItemsView itemsView)
		{
		}

		[MissingMapper]
		public static void MapIsVisible(ItemsViewHandler<TItemsView> handler, ItemsView itemsView)
		{
		}

		[MissingMapper]
		public static void MapItemsUpdatingScrollMode(ItemsViewHandler<TItemsView> handler, ItemsView itemsView)
		{
		}
	}
}
