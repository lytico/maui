using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.Maui.Controls.Handlers.Items.Platform;
using Microsoft.Maui.Graphics;
using PlatformView = Gtk.Widget;
using Size = Microsoft.Maui.Graphics.Size;

namespace Microsoft.Maui.Controls.Handlers.Items
{
	public class CollectionViewSelectionChangedEventArgs : EventArgs
	{
		public IList<object>? SelectedItems { get; set; }
	}

	public class ItemTemplateAdaptor : ItemAdaptor
	{
		Dictionary<PlatformView, View> _platformTable = new();
		Dictionary<object, View?> _dataBindedViewTable = new();
		protected View? _headerCache;
		protected View? _footerCache;

		public ItemTemplateAdaptor(ItemsView itemsView) : this(itemsView, itemsView.ItemsSource, itemsView.ItemTemplate ?? new DefaultItemTemplate()) { }

		protected ItemTemplateAdaptor(Element itemsView, IEnumerable items, DataTemplate template) : base(items)
		{
			ItemTemplate = template;
			Element = itemsView;
			IsSelectable = itemsView is SelectableItemsView;
		}

		public event EventHandler<CollectionViewSelectionChangedEventArgs>? SelectionChanged;

		protected DataTemplate ItemTemplate { get; set; }

		protected Element Element { get; set; }

		protected virtual bool IsSelectable { get; }

		protected IMauiContext MauiContext => Element.Handler!.MauiContext!;

		public object GetData(int index)
		{
			if (this[index] == null)
				throw new InvalidOperationException("No data");

			return this[index]!;
		}

		public override void SendItemSelected(IEnumerable<int> selected)
		{
			var items = new List<object>();

			foreach (var idx in selected)
			{
				if (idx < 0 || Count <= idx)
					continue;

				var selectedObject = this[idx];

				if (selectedObject != null)
					items.Add(selectedObject);
			}

			SelectionChanged?.Invoke(this, new CollectionViewSelectionChangedEventArgs { SelectedItems = items });
		}

		public override void UpdateViewState(PlatformView view, ViewHolderState state)
		{
			base.UpdateViewState(view, state);

			if (_platformTable.TryGetValue(view, out View? formsView))
			{
				switch (state)
				{
					case ViewHolderState.Focused:
						VisualStateManager.GoToState(formsView, VisualStateManager.CommonStates.Focused);
						formsView.SetValue(VisualElement.IsFocusedPropertyKey, true);

						break;
					case ViewHolderState.Normal:
						VisualStateManager.GoToState(formsView, VisualStateManager.CommonStates.Normal);
						formsView.SetValue(VisualElement.IsFocusedPropertyKey, false);

						break;
					case ViewHolderState.Selected:
						if (IsSelectable)
							VisualStateManager.GoToState(formsView, VisualStateManager.CommonStates.Selected);

						break;
				}
			}
		}

		public override IView? GetTemplatedView(PlatformView view)
		{
			return _platformTable.TryGetValue(view, out View? value) ? value : null;
		}

		public override IView? GetTemplatedView(int index)
		{
			var item = this[index];

			if (item != null && Count > index && _dataBindedViewTable.TryGetValue(item, out View? view))
			{
				return view;
			}

			return null;
		}

		public override object GetViewCategory(int index)
		{
			if (ItemTemplate is DataTemplateSelector selector)
			{
				return selector.SelectTemplate(this[index], Element);
			}

			return base.GetViewCategory(index);
		}

		public override PlatformView CreatePlatformView(int index)
		{
			View view;

			if (ItemTemplate is DataTemplateSelector selector)
			{
				view = (View)selector.SelectTemplate(GetData(index), Element).CreateContent();
			}
			else
			{
				view = (View)ItemTemplate.CreateContent();
			}

			var platformView = view.ToPlatform(MauiContext);
			_platformTable[platformView] = view;

			return platformView;
		}

		public override PlatformView CreatePlatformView()
		{
			return CreatePlatformView(0);
		}

#pragma warning disable CS8764
		public override PlatformView? GetHeaderView()
#pragma warning restore CS8764
		{
			if (_headerCache != null)
			{
				_headerCache.MeasureInvalidated -= OnHeaderFooterMeasureInvalidated;
			}

			_headerCache = CreateHeaderView();

			if (_headerCache != null)
			{
				_headerCache.Parent = Element;

				if (_headerCache.Handler is IDisposable nativeHandler)
					nativeHandler.Dispose();

				_headerCache.Handler = null;
				_headerCache.MeasureInvalidated += OnHeaderFooterMeasureInvalidated;

				return _headerCache.ToPlatform(MauiContext);
			}

			return null;
		}

#pragma warning disable CS8764
		public override PlatformView? GetFooterView()
#pragma warning restore CS8764
		{
			if (_footerCache != null)
			{
				_footerCache.MeasureInvalidated -= OnHeaderFooterMeasureInvalidated;
			}

			_footerCache = CreateFooterView();

			if (_footerCache != null)
			{
				_footerCache.Parent = Element;

				if (_footerCache.Handler is IDisposable nativeHandler)
					nativeHandler.Dispose();

				_footerCache.Handler = null;
				_footerCache.MeasureInvalidated += OnHeaderFooterMeasureInvalidated;

				return _footerCache.ToPlatform(MauiContext);
			}

			return null;
		}

		public override void RemovePlatformView(PlatformView native)
		{
			UnBinding(native);

			if (_platformTable.TryGetValue(native, out View? view))
			{
				if (view.Handler is IPlatformViewHandler handler)
				{
					_platformTable.Remove(handler.PlatformView!);

					if (handler is IDisposable d)
						d.Dispose();

					view.Handler = null;
				}
			}
		}

		public override void SetBinding(PlatformView native, int index)
		{
			if (_platformTable.TryGetValue(native, out View? view))
			{
				ResetBindedView(view);
				view.BindingContext = this[index];
				_dataBindedViewTable[this[index]!] = view;
				view.MeasureInvalidated += OnItemMeasureInvalidated;

				AddLogicalChild(view);
			}
		}

		public override void UnBinding(PlatformView native)
		{
			if (_platformTable.TryGetValue(native, out View? view))
			{
				view.MeasureInvalidated -= OnItemMeasureInvalidated;
				ResetBindedView(view);
			}
		}

		public override Size MeasureItem(double widthConstraint, double heightConstraint)
		{
			return MeasureItem(0, widthConstraint, heightConstraint);
		}

		public override Size MeasureItem(int index, double widthConstraint, double heightConstraint)
		{
			if (index < 0 || index >= Count || this[index] == null)
				return new Size(0, 0);

			widthConstraint = widthConstraint.ToScaledDP();
			heightConstraint = heightConstraint.ToScaledDP();

			if (_dataBindedViewTable.TryGetValue(GetData(index), out View? createdView) && createdView != null)
			{
				return (createdView as IView).Measure(widthConstraint, heightConstraint).ToPixel();
			}

			View view;

			if (ItemTemplate is DataTemplateSelector selector)
			{
				view = (View)selector.SelectTemplate(GetData(index), Element).CreateContent();
			}
			else
			{
				view = (View)ItemTemplate.CreateContent();
			}


			if (Count > index)
				view.BindingContext = this[index];

			view.Parent = Element;

			var platformView = view.ToPlatform(MauiContext);
			var handler = view.Handler!;

			try
			{
				var measured = (view as IView).Measure(widthConstraint, heightConstraint).ToPixel();
				return measured;
			}
			finally
			{
				if (handler is IDisposable d)
					d.Dispose();
			}
		}

		public override Size MeasureHeader(double widthConstraint, double heightConstraint)
		{
			// TODO. It is workaround code, if update Tizen.UIExtensions.NUI, this code will be removed
			if (CollectionView is Platform.CollectionView cv)
			{
				if (cv.LayoutManager != null)
				{
					if (cv.LayoutManager.IsHorizontal)
						widthConstraint = double.PositiveInfinity;
					else
						heightConstraint = double.PositiveInfinity;
				}
			}

			return (_headerCache as IView)?.Measure(widthConstraint.ToScaledDP(), heightConstraint.ToScaledDP()).ToPixel() ?? new Size(0, 0);
		}

		public override Size MeasureFooter(double widthConstraint, double heightConstraint)
		{
			return (_footerCache as IView)?.Measure(widthConstraint.ToScaledDP(), heightConstraint.ToScaledDP()).ToPixel() ?? new Size(0, 0);
		}

		protected virtual View? CreateHeaderView()
		{
			if (Element is StructuredItemsView structuredItemsView)
			{
				if (structuredItemsView.Header != null)
				{
					View? header = null;

					if (structuredItemsView.Header is View view)
					{
						header = view;
					}
					else if (structuredItemsView.HeaderTemplate != null)
					{
						header = (View)structuredItemsView.HeaderTemplate.CreateContent();
						header.BindingContext = structuredItemsView.Header;
					}
					else if (structuredItemsView.Header is string str)
					{
						header = new Label { Text = str, };
					}

					return header;
				}
			}

			return null;
		}

		protected virtual View? CreateFooterView()
		{
			if (Element is StructuredItemsView structuredItemsView)
			{
				if (structuredItemsView.Footer != null)
				{
					View? footer = null;

					if (structuredItemsView.Footer is View view)
					{
						footer = view;
					}
					else if (structuredItemsView.FooterTemplate != null)
					{
						footer = (View)structuredItemsView.FooterTemplate.CreateContent();
						footer.BindingContext = structuredItemsView.Footer;
					}
					else if (structuredItemsView.Footer is string str)
					{
						footer = new Label { Text = str, };
					}

					return footer;
				}
			}

			return null;
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (_headerCache != null)
				{
					_headerCache.MeasureInvalidated -= OnHeaderFooterMeasureInvalidated;
				}

				if (_footerCache != null)
				{
					_footerCache.MeasureInvalidated -= OnHeaderFooterMeasureInvalidated;
				}
			}

			base.Dispose(disposing);
		}

		void ResetBindedView(View view)
		{
			if (view.BindingContext != null && _dataBindedViewTable.ContainsKey(view.BindingContext))
			{
				_dataBindedViewTable[view.BindingContext] = null;
				RemoveLogicalChild(view);
				view.BindingContext = null;
			}
		}

		void OnItemMeasureInvalidated(object? sender, EventArgs e)
		{
			var data = (sender as View)?.BindingContext ?? null;
			int index = data != null ? GetItemIndex(data) : -1;

			if (index != -1)
			{
				CollectionView?.ItemMeasureInvalidated(index);
			}
		}

		void OnHeaderFooterMeasureInvalidated(object? sender, EventArgs e)
		{
			CollectionView?.ItemMeasureInvalidated(-1);
		}

		void AddLogicalChild(Element element)
		{
			if (Element is ItemsView iv)
			{
				iv.AddLogicalChild(element);
			}
			else
			{
				element.Parent = Element;
			}
		}

		void RemoveLogicalChild(Element element)
		{
			if (Element is ItemsView iv)
			{
				iv.RemoveLogicalChild(element);
			}
			else
			{
				element.Parent = null;
			}
		}
	}

	public class CarouselViewItemTemplateAdaptor : ItemTemplateAdaptor
	{
		public CarouselViewItemTemplateAdaptor(ItemsView itemsView) : base(itemsView) { }

		public override Size MeasureItem(double widthConstraint, double heightConstraint)
		{
			return MeasureItem(0, widthConstraint, heightConstraint);
		}

		public override Size MeasureItem(int index, double widthConstraint, double heightConstraint)
		{
			return (CollectionView as PlatformView)!.Size();
		}
	}

	class DefaultItemTemplate : DataTemplate
	{
		public DefaultItemTemplate() : base(CreateView) { }

		class ToTextConverter : IValueConverter
		{
			public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
			{
				return value?.ToString() ?? string.Empty;
			}

			public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
				=> throw new NotImplementedException();
		}

		static View CreateView()
		{
			var label = new Label { TextColor = Colors.Black, };

			label.SetBinding(Label.TextProperty, new Binding(".", converter: new ToTextConverter()));

			return new Controls.StackLayout { BackgroundColor = Colors.White, Padding = 30, Children = { label } };
		}
	}
}