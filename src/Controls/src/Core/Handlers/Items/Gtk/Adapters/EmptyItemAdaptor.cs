using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Maui.Controls.Handlers.Items.Platform;
using PlatformView = Gtk.Widget;
using Size = Microsoft.Maui.Graphics.Size;
using XLabel = Microsoft.Maui.Controls.Label;

namespace Microsoft.Maui.Controls.Handlers.Items
{
	public class EmptyItemAdaptor : ItemTemplateAdaptor
	{
		static DataTemplate s_defaultEmptyTemplate = new(typeof(EmptyView));

		View? _createdEmptyView;

		public EmptyItemAdaptor(ItemsView itemsView, IEnumerable items, DataTemplate template) : base(itemsView, items, template)
		{
		}

		public static EmptyItemAdaptor Create(ItemsView itemsView)
		{
			DataTemplate template;
			if (itemsView.EmptyView is View emptyView)
			{
				template = new DataTemplate(() =>
				{
					return emptyView;
				});
			}
			else
			{
				template = itemsView.EmptyViewTemplate ?? s_defaultEmptyTemplate;
			}

			return new EmptyItemAdaptor(itemsView, new List<object>(), template);
		}

		public override PlatformView? GetFooterView()
		{
			return null;
		}

		public override PlatformView? GetHeaderView()
		{
			View emptyView = (View)ItemTemplate.CreateContent();

			if (emptyView.Handler is IDisposable platformHandler)
				platformHandler.Dispose();

			emptyView.Handler = null;
			if (emptyView != (Element as ItemsView)?.EmptyView)
				emptyView.BindingContext = (Element as ItemsView)?.EmptyView;

			var header = CreateHeaderView();
			if (header != null)
			{
				if (header.Handler is IDisposable nativeHandler)
					nativeHandler.Dispose();
				header.Handler = null;
			}

			var footer = CreateFooterView();
			if (footer != null)
			{
				if (footer.Handler is IDisposable nativeHandler)
					nativeHandler.Dispose();
				footer.Handler = null;
			}

			bool isHorizontal = false;

			if (CollectionView is Platform.CollectionView cv)
			{
				if (cv.LayoutManager != null)
				{
					isHorizontal = cv.LayoutManager.IsHorizontal;
				}
			}

			var layout = new Grid();

			if (isHorizontal)
			{
				layout.ColumnDefinitions.Add(new ColumnDefinition
				{
					Width = GridLength.Auto,
				});
				layout.ColumnDefinitions.Add(new ColumnDefinition
				{
					Width = GridLength.Star,
				});
				layout.ColumnDefinitions.Add(new ColumnDefinition
				{
					Width = GridLength.Auto,
				});
			}
			else
			{
				layout.RowDefinitions.Add(new RowDefinition
				{
					Height = GridLength.Auto,
				});
				layout.RowDefinitions.Add(new RowDefinition
				{
					Height = GridLength.Star,
				});
				layout.RowDefinitions.Add(new RowDefinition
				{
					Height = GridLength.Auto,
				});
			}

			if (header != null)
			{
				layout.Add(header, 0, 0);
			}

			if (isHorizontal)
				layout.Add(emptyView, 1, 0);
			else
				layout.Add(emptyView, 0, 1);

			if (footer != null)
			{
				if (isHorizontal)
					layout.Add(footer, 2, 0);
				else
					layout.Add(footer, 0, 2);
			}

			layout.Parent = Element;
			_createdEmptyView = layout;

			return layout.ToPlatform(MauiContext);
		}

		public override Size MeasureHeader(double widthConstraint, double heightConstraint)
		{
			return (CollectionView as PlatformView)!.Size();
		}

		public override Size MeasureItem(double widthConstraint, double heightConstraint)
		{
			return new Size(widthConstraint, heightConstraint);
		}

		class EmptyView : StackLayout
		{
			public EmptyView()
			{
				HorizontalOptions = LayoutOptions.Fill;
				VerticalOptions = LayoutOptions.Fill;
				Children.Add(
					new XLabel
					{
						Text = "No items found",
						VerticalOptions = LayoutOptions.Center,
						HorizontalOptions = LayoutOptions.Center,
						HorizontalTextAlignment = TextAlignment.Center,
						VerticalTextAlignment = TextAlignment.Center,
					}
				);
			}
		}
	}
}
