using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading;
using Gtk;
using Microsoft.Maui.Controls;
using Rect = Microsoft.Maui.Graphics.Rect;
using Size = Microsoft.Maui.Graphics.Size;
using CollectionViewSelectionMode = Microsoft.Maui.Controls.SelectionMode;

namespace Gtk.UIExtensions.NUI
{
	/// <summary>
	/// A View that contain a templated list of items.
	/// </summary>
	public partial class CollectionView : Gtk.Container, ICollectionViewController
	{
		RecyclerPool _pool = new RecyclerPool();
		Dictionary<ViewHolder, int> _viewHolderIndexTable = new Dictionary<ViewHolder, int>();

		HashSet<int> _selectedItems = new HashSet<int>();

		SynchronizationContext _mainloopContext;
		ICollectionViewLayoutManager? _layoutManager;
		ItemAdaptor? _adaptor;

		bool _requestLayoutItems = false;

		double _previousHorizontalOffset;
		double _previousVerticalOffset;
		Size _itemSize = new Size(-1, -1);

		Widget? _headerView;
		Widget? _footerView;

		Microsoft.Maui.Controls.SelectionMode _selectionMode;

		/// <summary>
		/// Initializes a new instance of the <see cref="CollectionView"/> class.
		/// </summary>
#pragma warning disable CS8618
		// dotnet compiler does not track a method that called on constructor to check non-nullable object
		// https://github.com/dotnet/roslyn/issues/32358
		public CollectionView()
#pragma warning restore CS8618
		{
			HasWindow = false;
			_mainloopContext = SynchronizationContext.Current ?? throw new InvalidOperationException("Must create on main thread");
			InitializationComponent();
		}

		/// <summary>
		/// Event that is raised after a scroll completes.
		/// </summary>
		public event EventHandler<CollectionViewScrolledEventArgs>? Scrolled;

		/// <summary>
		/// Gets a ScrollView instance that used in CollectionView
		/// </summary>
		public ScrollableBase ScrollView { get; private set; }

		/// <summary>
		/// The number of items on CollectionView
		/// </summary>
		public int Count => Adaptor?.Count ?? 0;

		/// <summary>
		/// Selected items
		/// </summary>
		public IEnumerable<int> SelectedItems { get => _selectedItems; }

		/// <summary>
		/// Gets or sets LayoutManager to organize position of item
		/// </summary>
		public ICollectionViewLayoutManager? LayoutManager
		{
			get => _layoutManager;
			set
			{
				OnLayoutManagerChanging();
				_layoutManager = value;
				OnLayoutManagerChanged();
			}
		}

		/// <summary>
		/// Gets or sets ItemAdaptor to adapt items source
		/// </summary>
		public ItemAdaptor? Adaptor
		{
			get => _adaptor;
			set
			{
				OnAdaptorChanging();
				_adaptor = value;
				OnAdaptorChanged();
			}
		}

		/// <summary>
		/// A size of scrolling area
		/// </summary>
		public Size ScrollCanvasSize => LayoutManager?.GetScrollCanvasSize() ?? new Size(0, 0);

		/// <summary>
		/// Gets or sets a value that controls whether and how many items can be selected.
		/// </summary>
		public Microsoft.Maui.Controls.SelectionMode SelectionMode
		{
			get => _selectionMode;
			set
			{
				_selectionMode = value;
				UpdateSelectionMode();
			}
		}

		/// <summary>
		/// Specifies the behavior of snap points when scrolling.
		/// </summary>
		public SnapPointsType SnapPointsType { get; set; }

		/// <summary>
		/// Specifies how snap points are aligned with items.
		/// </summary>
		public SnapPointsAlignment SnapPointsAlignment { get; set; }

		/// <summary>
		/// The size of the area that layout in advance before it is visible
		/// </summary>
		public float RedundancyLayoutBoundRatio { get; set; } = 2f;

		/// <summary>
		/// A size of allocated by Layout, it become viewport size on scrolling
		/// </summary>
		protected Size AllocatedSize { get; set; }

		internal Rect ViewPort => ScrollView.GetScrollBound();

		/// <summary>
		/// Scrolls the CollectionView to the index
		/// </summary>
		/// <param name="index">Index of item</param>
		/// <param name="position">How the item should be positioned on screen.</param>
		/// <param name="animate">Whether or not the scroll should be animated.</param>
		public void ScrollTo(int index, ScrollToPosition position = ScrollToPosition.MakeVisible, bool animate = true)
		{
			if (LayoutManager == null)
				throw new InvalidOperationException("No Layout manager");

			var itemBound = LayoutManager.GetItemBound(index);
			double itemStart;
			double itemEnd;
			double scrollStart;
			double scrollEnd;
			double itemPadding = 0;
			double itemSize;
			double viewportSize;
			var isHorizontal = LayoutManager.IsHorizontal;

			if (isHorizontal)
			{
				itemStart = itemBound.Left;
				itemEnd = itemBound.Right;
				itemSize = itemBound.Width;
				scrollStart = ViewPort.Left;
				scrollEnd = ViewPort.Right;
				viewportSize = ViewPort.Width;
			}
			else
			{
				itemStart = itemBound.Top;
				itemEnd = itemBound.Bottom;
				itemSize = itemBound.Height;
				scrollStart = ViewPort.Top;
				scrollEnd = ViewPort.Bottom;
				viewportSize = ViewPort.Height;
			}

			if (position == ScrollToPosition.MakeVisible)
			{
				if (itemStart < scrollStart)
				{
					position = ScrollToPosition.Start;
				}
				else if (itemEnd > scrollEnd)
				{
					position = ScrollToPosition.End;
				}
				else
				{
					// already visible
					return;
				}
			}

			if (itemSize < viewportSize)
			{
				switch (position)
				{
					case ScrollToPosition.Center:
						itemPadding = (viewportSize - itemSize) / 2;

						break;
					case ScrollToPosition.End:
						itemPadding = (viewportSize - itemSize);

						break;
				}
			}

			if (isHorizontal)
			{
				itemBound.X -= itemPadding;
			}
			else
			{
				itemBound.Y -= itemPadding;
			}

			ScrollView.ScrollTo(isHorizontal ? (float)itemBound.X : (float)itemBound.Y, animate);
		}

		/// <summary>
		/// Scrolls the CollectionView to the item
		/// </summary>
		/// <param name="item">Item to scroll</param>
		/// <param name="position">How the item should be positioned on screen.</param>
		/// <param name="animate">Whether or not the scroll should be animated.</param>
		public void ScrollTo(object item, ScrollToPosition position = ScrollToPosition.MakeVisible, bool animate = true)
		{
			if (Adaptor == null)
				throw new InvalidOperationException("No Adaptor");

			ScrollTo(Adaptor.GetItemIndex(item), position, animate);
		}

		/// <summary>
		/// Request item selection
		/// </summary>
		/// <param name="index">Index of item</param>
		public void RequestItemSelect(int index) => RequestItemSelect(index, null);

		/// <summary>
		/// Request item unselection
		/// </summary>
		/// <param name="index">Index of item</param>
		public void RequestItemUnselect(int index) => RequestItemUnselect(index, null);

		/// <summary>
		/// Create a ViewHolder, override it to customzie a decoration of view
		/// </summary>
		/// <returns>A ViewHolder instance</returns>
		protected virtual ViewHolder CreateViewHolder()
		{
			return new ViewHolder();
		}

		/// <summary>
		/// Create a ScrollView to use in CollectionView
		/// </summary>
		/// <returns>A ScrollView instance</returns>
		protected virtual ScrollableBase CreateScrollView()
		{
			return new SnappableScrollView(this) { UseCostomScrolling = true, MaximumVelocity = 8.5f, Friction = 0.015f };
		}

		/// <summary>
		/// Initialize internal components, such as ScrollView
		/// </summary>
		protected virtual void InitializationComponent()
		{
			//Layout = new AbsoluteLayout();
			this.WidthSpecification(LayoutParamPolicies.MatchParent);
			this.HeightSpecification(LayoutParamPolicies.MatchParent);

			ScrollView = CreateScrollView();
			ScrollView.WidthSpecification(LayoutParamPolicies.MatchParent);
			ScrollView.HeightSpecification(LayoutParamPolicies.MatchParent);
			ScrollView.WidthResizePolicy(ResizePolicyType.FillToParent);
			ScrollView.HeightResizePolicy(ResizePolicyType.FillToParent);

			ScrollView.ScrollingEventThreshold = 10;
			ScrollView.Scrolling += OnScrolling;
			ScrollView.ScrollAnimationEnded += OnScrollAnimationEnded;
			ScrollView.Relayout += OnLayout;

			if (ScrollView is SnappableScrollView snappable)
			{
				snappable.SnapRequestFinished += OnSnapRequestFinished;
			}

			// Add(ScrollView);
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && ScrollView is IDisposable d)
			{
				d.Dispose();
			}

			base.Dispose(disposing);
		}

		void ICollectionViewController.ItemMeasureInvalidated(int index)
		{
			if (index == -1)
			{
				UpdateHeaderFooter();
				RequestLayoutItems();

				return;
			}

			// If a first item size was updated, need to reset _itemSize
			if (index == 0)
			{
				_itemSize = new Size(-1, -1);
			}

			LayoutManager?.ItemMeasureInvalidated(index);
		}

		void ICollectionViewController.RequestLayoutItems() => RequestLayoutItems();

		void RequestLayoutItems()
		{
			if (AllocatedSize.Width <= 0 || AllocatedSize.Height <= 0)
				return;

			if (!_requestLayoutItems)
			{
				_requestLayoutItems = true;

				_mainloopContext.Post((s) =>
				{
					_requestLayoutItems = false;

					if (Adaptor != null && LayoutManager != null)
					{
						ContentSizeUpdated();
						LayoutManager.LayoutItems(ExtendViewPort(ViewPort), true);
					}
				}, null);
			}
		}

		void ICollectionViewController.ContentSizeUpdated() => ContentSizeUpdated();

		void ContentSizeUpdated()
		{
			ScrollView.ContentContainer.UpdateSize(LayoutManager?.GetScrollCanvasSize() ?? AllocatedSize);
		}

		Size ICollectionViewController.GetItemSize()
		{
			var widthConstraint = LayoutManager!.IsHorizontal ? double.PositiveInfinity : AllocatedSize.Width;
			var heightConstraint = LayoutManager!.IsHorizontal ? AllocatedSize.Height : double.PositiveInfinity;

			return GetItemSize(widthConstraint, heightConstraint);
		}

		Size ICollectionViewController.GetItemSize(double widthConstraint, double heightConstraint) => GetItemSize(widthConstraint, heightConstraint);

		Size GetItemSize(double widthConstraint, double heightConstraint)
		{
			if (Adaptor == null || Adaptor.Count == 0)
			{
				return new Size(0, 0);
			}

			if (_itemSize.Width > 0 && _itemSize.Height > 0)
			{
				return _itemSize;
			}

			_itemSize = Adaptor.MeasureItem(widthConstraint, heightConstraint);
			_itemSize.Width = Math.Max(_itemSize.Width, 10);
			_itemSize.Height = Math.Max(_itemSize.Height, 10);

			return _itemSize;
		}

		Size ICollectionViewController.GetItemSize(int index, double widthConstraint, double heightConstraint) => GetItemSize(index, widthConstraint, heightConstraint);

		Size GetItemSize(int index, double widthConstraint, double heightConstraint)
		{
			if (Adaptor == null)
			{
				return new Size(0, 0);
			}

			return Adaptor.MeasureItem(index, widthConstraint, heightConstraint);
		}

		ViewHolder ICollectionViewController.RealizeView(int index)
		{
			if (Adaptor == null)
				throw new InvalidOperationException("No Adaptor");

			var holder = _pool.GetRecyclerView(Adaptor.GetViewCategory(index));

			if (holder != null)
			{
				holder.Show();
			}
			else
			{
				var content = Adaptor.CreateNativeView(index);
				holder = CreateViewHolder();
				holder.RequestSelected += OnRequestItemSelected;
				holder.StateUpdated += OnItemStateUpdated;
				holder.Content = content;
				holder.ViewCategory = Adaptor.GetViewCategory(index);
				ScrollView.ContentContainer.Add(holder);
			}

			Adaptor.SetBinding(holder.Content!, index);
			_viewHolderIndexTable[holder] = index;

			if (_selectedItems.Contains(index))
			{
				holder.UpdateSelected();
			}

			return holder;
		}

		void ICollectionViewController.UnrealizeView(ViewHolder view)
		{
			if (Adaptor == null)
				throw new InvalidOperationException("No Adaptor");

			_viewHolderIndexTable.Remove(view);
			Adaptor.UnBinding(view.Content!);
			view.ResetState();
			view.Hide();

			if (_pool.Count < Math.Max(Count / 3, _viewHolderIndexTable.Count * 3))
			{
				_pool.AddRecyclerView(view);
			}
			else
			{
				var content = view.Content;
				view.Content = null;

				if (content != null)
					Adaptor.RemoveNativeView(content);

				view.Unparent();
				view.Dispose();
			}
		}

		void ICollectionViewController.RequestItemSelect(int index) => RequestItemSelect(index);

		void RequestItemSelect(int index, ViewHolder? viewHolder = null)
		{
			if (SelectionMode == Microsoft.Maui.Controls.SelectionMode.None)
				return;

			if (SelectionMode != Microsoft.Maui.Controls.SelectionMode.Multiple && _selectedItems.Any())
			{
				var selected = _selectedItems.First();

				if (selected == index)
				{
					// already selected
					if (SelectionMode == Microsoft.Maui.Controls.SelectionMode.Single)
						return;
				}
				else
				{
					// clear previous selection item
					var prevSelected = FindViewHolder(_selectedItems.First());
					prevSelected?.ResetState();
					_selectedItems.Clear();
				}
			}

			_selectedItems.Add(index);

			if (viewHolder != null)
			{
				viewHolder.UpdateSelected();
			}
			else
			{
				FindViewHolder(index)?.UpdateSelected();
			}

			Adaptor?.SendItemSelected(_selectedItems);
		}

		void RequestItemUnselect(int index, ViewHolder? viewHolder = null)
		{
			if (SelectionMode == Microsoft.Maui.Controls.SelectionMode.None)
				return;

			if (_selectedItems.Contains(index))
			{
				if (viewHolder == null)
				{
					viewHolder = FindViewHolder(index);
				}

				_selectedItems.Remove(index);
				viewHolder?.ResetState();
			}

			Adaptor?.SendItemSelected(_selectedItems);
		}

		void OnLayoutManagerChanging()
		{
			_layoutManager?.Reset();
		}

		void OnLayoutManagerChanged()
		{
			if (_layoutManager == null)
				return;

			_itemSize = new Size(-1, -1);
			_layoutManager.CollectionView = this;
			var orientation = _layoutManager.IsHorizontal ? ScrollableBase.Direction.Horizontal : ScrollableBase.Direction.Vertical;

			if (ScrollView.ScrollingDirection != orientation)
			{
				ScrollView.ScrollTo(0, false);
				ScrollView.ScrollingDirection = orientation;
			}

			_layoutManager.SizeAllocated(AllocatedSize);
			UpdateHeaderFooter();
			RequestLayoutItems();
		}

		void OnAdaptorChanging()
		{
			// reset header view
			if (_headerView != null)
			{
				_headerView.Unparent();
				Adaptor?.RemoveHeaderView(_headerView);
				_headerView = null;
			}

			// reset footer view
			if (_footerView != null)
			{
				_footerView.Unparent();
				Adaptor?.RemoveFooterView(_footerView);
				_footerView.Dispose();
				_footerView = null;
			}

			LayoutManager?.Reset();

			if (Adaptor != null)
			{
				_pool.Clear(Adaptor);
				(Adaptor as INotifyCollectionChanged).CollectionChanged -= OnCollectionChanged;
				Adaptor.CollectionView = null;
			}

			_selectedItems.Clear();
		}

		void OnAdaptorChanged()
		{
			if (Adaptor == null)
				return;

			_itemSize = new Size(-1, -1);
			Adaptor.CollectionView = this;
			(Adaptor as INotifyCollectionChanged).CollectionChanged += OnCollectionChanged;

			LayoutManager?.ItemSourceUpdated();
			RequestLayoutItems();

			_headerView = Adaptor.GetHeaderView();

			if (_headerView != null)
			{
				ScrollView.ContentContainer.Add(_headerView);
			}

			_footerView = Adaptor.GetFooterView();

			if (_footerView != null)
			{
				ScrollView.ContentContainer.Add(_footerView);
			}

			UpdateHeaderFooter();
		}

		void OnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
		{
			if (sender != Adaptor)
			{
				return;
			}

			if (e.Action == NotifyCollectionChangedAction.Add && e.NewItems != null)
			{
				int idx = e.NewStartingIndex;

				if (idx == -1)
				{
					idx = Adaptor!.Count - e.NewItems.Count;
				}

				foreach (var item in e.NewItems)
				{
					foreach (var viewHolder in _viewHolderIndexTable.Keys.ToList())
					{
						if (_viewHolderIndexTable[viewHolder] >= idx)
						{
							_viewHolderIndexTable[viewHolder]++;
						}
					}

					var updated = new HashSet<int>();

					foreach (var selected in _selectedItems)
					{
						if (selected >= idx)
						{
							updated.Add(selected + 1);
						}
					}

					_selectedItems = updated;
					LayoutManager?.ItemInserted(idx++);
				}
			}
			else if (e.Action == NotifyCollectionChangedAction.Remove && e.OldItems != null)
			{
				int idx = e.OldStartingIndex;

				// Can't tracking remove if there is no data of old index
				if (idx == -1)
				{
					LayoutManager?.ItemSourceUpdated();
				}
				else
				{
					foreach (var item in e.OldItems)
					{
						LayoutManager?.ItemRemoved(idx);

						foreach (var viewHolder in _viewHolderIndexTable.Keys.ToList())
						{
							if (_viewHolderIndexTable[viewHolder] > idx)
							{
								_viewHolderIndexTable[viewHolder]--;
							}
						}

						if (_selectedItems.Contains(idx))
						{
							_selectedItems.Remove(idx);
						}

						var updated = new HashSet<int>();

						foreach (var selected in _selectedItems)
						{
							if (selected > idx)
							{
								updated.Add(selected - 1);
							}
						}

						_selectedItems = updated;
					}
				}
			}
			else if (e.Action == NotifyCollectionChangedAction.Move)
			{
				LayoutManager?.ItemRemoved(e.OldStartingIndex);
				LayoutManager?.ItemInserted(e.NewStartingIndex);
			}
			else if (e.Action == NotifyCollectionChangedAction.Replace && e.OldItems != null)
			{
				// Can't tracking if there is no information old data
				if (e.OldItems.Count > 1 || e.NewStartingIndex == -1)
				{
					LayoutManager?.ItemSourceUpdated();
				}
				else
				{
					LayoutManager?.ItemUpdated(e.NewStartingIndex);
				}
			}
			else if (e.Action == NotifyCollectionChangedAction.Reset)
			{
				LayoutManager?.Reset();
				LayoutManager?.ItemSourceUpdated();
			}

			RequestLayoutItems();
		}

		void OnScrollAnimationEnded(object? sender, ScrollEventArgs e)
		{
			SendScrolledEvent();
		}

		void OnSnapRequestFinished(object? sender, EventArgs e)
		{
			SendScrolledEvent();
		}

		void OnLayout(object? sender, EventArgs e)
		{
			//called when resized
			AllocatedSize = this.Size();
			_itemSize = new Size(-1, -1);

			if (Adaptor != null && LayoutManager != null)
			{
				LayoutManager.SizeAllocated(AllocatedSize);
				UpdateHeaderFooter();
				ContentSizeUpdated();
				LayoutManager.LayoutItems(ExtendViewPort(ViewPort));
			}
		}

		void OnScrolling(object? sender, ScrollEventArgs e)
		{
			if (LayoutManager == null)
				return;

			if (sender is IScrollable sa)
			{
				;
			}

			var viewport = ViewPort;
			var viewportFromEvent = new Rect(-e.Event.X, -e.Event.Y, viewport.Width, viewport.Height);

			LayoutManager?.LayoutItems(ExtendViewPort(viewportFromEvent));
		}

		void SendScrolledEvent()
		{
			if (LayoutManager == null)
				return;

			var args = new CollectionViewScrolledEventArgs();
			args.FirstVisibleItemIndex = LayoutManager.GetVisibleItemIndex(ViewPort.X, ViewPort.Y);
			args.CenterItemIndex = LayoutManager.GetVisibleItemIndex(ViewPort.X + (ViewPort.Width / 2), ViewPort.Y + (ViewPort.Height / 2));
			args.LastVisibleItemIndex = LayoutManager.GetVisibleItemIndex(ViewPort.X + ViewPort.Width, ViewPort.Y + ViewPort.Height);
			args.HorizontalOffset = ViewPort.X;
			args.HorizontalDelta = ViewPort.X - _previousHorizontalOffset;
			args.VerticalOffset = ViewPort.Y;
			args.VerticalDelta = ViewPort.Y - _previousVerticalOffset;
			Scrolled?.Invoke(this, args);

			_previousHorizontalOffset = ViewPort.X;
			_previousVerticalOffset = ViewPort.Y;
		}

		void UpdateHeaderFooter()
		{
			if (LayoutManager != null)
			{
				double widthConstraint = LayoutManager.IsHorizontal ? double.PositiveInfinity : AllocatedSize.Width;
				double heightConstraint = LayoutManager.IsHorizontal ? AllocatedSize.Height : double.PositiveInfinity;

				LayoutManager.SetHeader(_headerView,
					_headerView != null ? Adaptor!.MeasureHeader(widthConstraint, heightConstraint) : new Size(0, 0));

				LayoutManager.SetFooter(_footerView,
					_footerView != null ? Adaptor!.MeasureFooter(widthConstraint, heightConstraint) : new Size(0, 0));
			}
		}

		void UpdateSelectionMode()
		{
			if (_selectionMode == Microsoft.Maui.Controls.SelectionMode.None)
			{
				if (_selectedItems.Count > 0)
				{
					foreach (var item in _viewHolderIndexTable)
					{
						if (_selectedItems.Contains(item.Value))
						{
							item.Key.ResetState();
						}
					}
				}

				_selectedItems.Clear();
			}
			else if (_selectionMode == Microsoft.Maui.Controls.SelectionMode.Single)
			{
				if (_selectedItems.Count > 1)
				{
					var first = _selectedItems.First();

					foreach (var item in _viewHolderIndexTable)
					{
						if (_selectedItems.Contains(item.Value) && first != item.Value)
						{
							item.Key.ResetState();
						}
					}

					_selectedItems.Clear();
					_selectedItems.Add(first);
				}
			}

			Adaptor?.SendItemSelected(_selectedItems);
		}

		void OnRequestItemSelected(object? sender, EventArgs e)
		{
			if (sender == null)
				return;

			// Selection request from UI
			var viewHolder = (ViewHolder)sender;

			if (_viewHolderIndexTable.ContainsKey(viewHolder))
			{
				var index = _viewHolderIndexTable[viewHolder];

				if (_selectedItems.Contains(index) && SelectionMode != Microsoft.Maui.Controls.SelectionMode.Single)
				{
					RequestItemUnselect(index, viewHolder);
				}
				else
				{
					RequestItemSelect(index, viewHolder);
				}
			}
		}

		void OnItemStateUpdated(object? sender, EventArgs e)
		{
			if (sender == null)
				return;

			ViewHolder holder = (ViewHolder)sender;

			// Hack, in NUI, equal was override and even though not null, if it has no Body , it treat as null
			if (!object.ReferenceEquals(holder.Content, null))
			{
				Adaptor?.UpdateViewState(holder.Content, holder.State);

				if (_viewHolderIndexTable.ContainsKey(holder) && holder.State == ViewHolderState.Focused)
				{
					var index = _viewHolderIndexTable[holder];
					ScrollTo(index, ScrollToPosition.MakeVisible, true);
				}
			}
		}

		ViewHolder? FindViewHolder(int index)
		{
			return _viewHolderIndexTable.Where(d => d.Value == index).Select(d => d.Key).FirstOrDefault();
		}

		Rect ExtendViewPort(Rect viewport)
		{
			if (LayoutManager == null)
				return viewport;

			if (LayoutManager.IsHorizontal)
			{
				viewport.X = Math.Max(0, viewport.X - viewport.Width * RedundancyLayoutBoundRatio / 2f);
				viewport.Width += viewport.Width * RedundancyLayoutBoundRatio;
			}
			else
			{
				viewport.Y = Math.Max(0, viewport.Y - viewport.Height * RedundancyLayoutBoundRatio / 2f);
				viewport.Height += viewport.Height * RedundancyLayoutBoundRatio;
			}

			return viewport;
		}
	}
}