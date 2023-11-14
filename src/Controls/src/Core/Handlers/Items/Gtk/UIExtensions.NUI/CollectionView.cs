using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading;
using Gtk;
using Microsoft.Maui.Controls;
using Layout = Gtk.Layout;
using Rect = Microsoft.Maui.Graphics.Rect;
using Size = Microsoft.Maui.Graphics.Size;

namespace Gtk.UIExtensions.NUI
{

	/// <summary>
	/// A View that contain a templated list of items.
	/// </summary>
	public class CollectionView : Gtk.Container, ICollectionViewController
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

		CollectionViewSelectionMode _selectionMode;

		/// <summary>
		/// Initializes a new instance of the <see cref="CollectionView"/> class.
		/// </summary>
#pragma warning disable CS8618
		// dotnet compiler does not track a method that called on constructor to check non-nullable object
		// https://github.com/dotnet/roslyn/issues/32358
		public CollectionView()
#pragma warning restore CS8618
		{
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
		public CollectionViewSelectionMode SelectionMode
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

		Rect ViewPort => ScrollView.GetScrollBound();

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
			return new SnappableScrollView(this)
			{
				UseCostomScrolling = true,
				MaximumVelocity = 8.5f,
				Friction = 0.015f
			};
		}

		/// <summary>
		/// Initialize internal components, such as ScrollView
		/// </summary>
		protected virtual void InitializationComponent()
		{
			Layout = new AbsoluteLayout();
			WidthSpecification = LayoutParamPolicies.MatchParent;
			HeightSpecification = LayoutParamPolicies.MatchParent;

			ScrollView = CreateScrollView();
			ScrollView.WidthSpecification = LayoutParamPolicies.MatchParent;
			ScrollView.HeightSpecification = LayoutParamPolicies.MatchParent;
			ScrollView.WidthResizePolicy = ResizePolicyType.FillToParent;
			ScrollView.HeightResizePolicy = ResizePolicyType.FillToParent;

			ScrollView.ScrollingEventThreshold = 10;
			ScrollView.Scrolling += OnScrolling;
			ScrollView.ScrollAnimationEnded += OnScrollAnimationEnded;
			ScrollView.Relayout += OnLayout;

			if (ScrollView is SnappableScrollView snappable)
			{
				snappable.SnapRequestFinished += OnSnapRequestFinished;
			}

			Add(ScrollView);
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				ScrollView.Dispose();
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
			if (SelectionMode == CollectionViewSelectionMode.None)
				return;

			if (SelectionMode != CollectionViewSelectionMode.Multiple && _selectedItems.Any())
			{
				var selected = _selectedItems.First();

				if (selected == index)
				{
					// already selected
					if (SelectionMode == CollectionViewSelectionMode.Single)
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
			if (SelectionMode == CollectionViewSelectionMode.None)
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
			AllocatedSize = Size.ToCommon();
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

			var viewport = ViewPort;
			var viewportFromEvent = new Rect(-e.Position.X, -e.Position.Y, viewport.Width, viewport.Height);

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
			if (_selectionMode == CollectionViewSelectionMode.None)
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
			else if (_selectionMode == CollectionViewSelectionMode.Single)
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

				if (_selectedItems.Contains(index) && SelectionMode != CollectionViewSelectionMode.SingleAlways)
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

		/// <summary>
		/// A ScrollView that implemented snap points
		/// </summary>
		class SnappableScrollView : ScrollableBase
		{

			delegate float UserAlphaFunctionDelegate(float progress);

			UserAlphaFunctionDelegate? _customScrollingAlphaFunctionDelegate;
			Func<float, float>? _scrollingAlpha;
			AlphaFunction? _customScrollingAlphaFunction;
			Animation? _snapAnimation;

			int _currentItemIndex = -1;

			public SnappableScrollView(CollectionView cv)
			{
				CollectionView = cv;

				_customScrollingAlphaFunctionDelegate = new UserAlphaFunctionDelegate(ScrollingAlpha);
				_customScrollingAlphaFunction = new AlphaFunction(_customScrollingAlphaFunctionDelegate);

				ScrollDragStarted += OnDragStart;

				ScrollAnimationEnded += OnAnimationEnd;
			}

			public event EventHandler? SnapRequestFinished;

			public bool UseCostomScrolling { get; set; }

			public float MaximumVelocity { get; set; } = 8.5f;

			public float Friction { get; set; } = 0.015f;

			CollectionView CollectionView { get; }

			ICollectionViewLayoutManager LayoutManager => CollectionView.LayoutManager!;

			Rect ViewPort => CollectionView.ViewPort;

			double ViewPortStart => IsHorizontal ? ViewPort.X : ViewPort.Y;

			double ViewPortEnd => IsHorizontal ? ViewPort.Right : ViewPort.Bottom;

			double ViewPortSize => IsHorizontal ? ViewPort.Width : ViewPort.Height;

			bool IsHorizontal => ScrollingDirection == Direction.Horizontal;

			protected override void Decelerating(float velocity, Animation animation)
			{
				if (CollectionView.SnapPointsType == SnapPointsType.MandatorySingle)
				{
					// Only one item should be passed when scrolling by snap
					HandleMandatorySingle(velocity);
				}
				else
				{
					HandleNonMandatorySingle(velocity, animation);
				}
			}

			void CustomScrolling(float velocity, Animation animation)
			{
				float absVelocity = Math.Abs(velocity);
				float friction = Friction;

				float totalTime = Math.Abs(velocity / friction);
				float totalDistance = absVelocity * totalTime - (friction * (float)Math.Pow(totalTime, 2) / 2f);

				float currentPosition = (ScrollingDirection == Direction.Horizontal ? ContentContainer.PositionX : ContentContainer.PositionY);
				float targetPosition = currentPosition + (velocity > 0 ? totalDistance : -totalDistance);
				float maximumScrollableSize = IsHorizontal ? ContentContainer.SizeWidth - SizeWidth : ContentContainer.SizeHeight - SizeHeight;

				if (targetPosition > 0)
				{
					totalDistance -= targetPosition;
					targetPosition = 0;
				}
				else if (targetPosition < -maximumScrollableSize)
				{
					var overlapped = -maximumScrollableSize - targetPosition;
					totalDistance -= overlapped;
					targetPosition = -maximumScrollableSize;
				}

				if (totalDistance < 1)
				{
					base.Decelerating(0, animation);

					return;
				}

				_scrollingAlpha = (progress) =>
				{
					if (totalDistance == 0)
						return 1;

					var time = totalTime * progress;
					var distance = absVelocity * time - (friction * (float)Math.Pow(time, 2) / 2f);

					return Math.Min(distance / totalDistance, 1);
				};

				animation.Duration = (int)totalTime;
				animation.AnimateTo(ContentContainer, (ScrollingDirection == Direction.Horizontal) ? "PositionX" : "PositionY", targetPosition, _customScrollingAlphaFunction);
				animation.Play();
			}

			float ScrollingAlpha(float progress)
			{
				return _scrollingAlpha?.Invoke(progress) ?? 1.0f;
			}

			void HandleNonMandatorySingle(float velocity, Animation animation)
			{
				if (Math.Abs(velocity) > MaximumVelocity)
				{
					velocity = MaximumVelocity * (velocity > 0 ? 1 : -1);
				}

				if (UseCostomScrolling)
				{
					CustomScrolling(velocity, animation);
				}
				else
				{
					if (CollectionView.SnapPointsType == SnapPointsType.None)
					{
						DecelerationRate = 0.998f;
					}
					else
					{
						// Adjust DecelerationRate to stop more quickly because it will be moved again by OnSnapRequest
						DecelerationRate = 0.992f;
					}

					base.Decelerating(velocity, animation);
				}
			}

			void HandleMandatorySingle(float velocity)
			{
				if (_currentItemIndex == -1)
					return;

				int currentItem = _currentItemIndex;

				if (Math.Abs(velocity) > 0.5)
				{
					if (velocity < 0)
					{
						currentItem = LayoutManager.NextRowItemIndex(currentItem);
					}
					else
					{
						currentItem = LayoutManager.PreviousRowItemIndex(currentItem);
					}
				}

				var itemBound = LayoutManager.GetItemBound(currentItem);
				var target = IsHorizontal ? itemBound.X : itemBound.Y;
				var itemSize = IsHorizontal ? itemBound.Width : itemBound.Height;
				var scrollingSize = IsHorizontal ? ContentContainer.SizeWidth : ContentContainer.SizeHeight;

				// adjust align
				if (CollectionView.SnapPointsAlignment == SnapPointsAlignment.Center)
				{
					target -= (ViewPortSize - itemSize) / 2;
				}
				else if (CollectionView.SnapPointsAlignment == SnapPointsAlignment.End)
				{
					target -= (ViewPortSize - itemSize);
				}

				// adjust end of scroll area
				if (scrollingSize - target < ViewPortSize)
				{
					target = scrollingSize - ViewPortSize;
				}

				if (target < 0)
				{
					target = 0;
				}

				ScrollTo(target);
			}

			void OnDragStart(object? sender, ScrollEventArgs e)
			{
				if (CollectionView.SnapPointsType == SnapPointsType.MandatorySingle)
				{
					MarkCurrentItem();
				}
			}

			void MarkCurrentItem()
			{
				if (CollectionView.SnapPointsAlignment == SnapPointsAlignment.Start)
				{
					_currentItemIndex = CollectionView.LayoutManager!.GetVisibleItemIndex(CollectionView.ViewPort.X, CollectionView.ViewPort.Y);
					var bound = CollectionView.LayoutManager!.GetItemBound(_currentItemIndex);
					var padding = IsHorizontal ? bound.Width / 2 : bound.Height / 2;

					_currentItemIndex = CollectionView.LayoutManager!.GetVisibleItemIndex(
						(IsHorizontal ? padding : 0) + CollectionView.ViewPort.X,
						(IsHorizontal ? 0 : padding) + CollectionView.ViewPort.Y);

				}
				else if (CollectionView.SnapPointsAlignment == SnapPointsAlignment.Center)
				{
					_currentItemIndex = CollectionView.LayoutManager!.GetVisibleItemIndex(CollectionView.ViewPort.X + (CollectionView.ViewPort.Width / 2), CollectionView.ViewPort.Y + (CollectionView.ViewPort.Height / 2));
				}
				else
				{
					_currentItemIndex = CollectionView.LayoutManager!.GetVisibleItemIndex(CollectionView.ViewPort.X + CollectionView.ViewPort.Width, CollectionView.ViewPort.Y + CollectionView.ViewPort.Height);
					var bound = CollectionView.LayoutManager!.GetItemBound(_currentItemIndex);
					var padding = IsHorizontal ? bound.Width / 2 : bound.Height / 2;

					_currentItemIndex = CollectionView.LayoutManager!.GetVisibleItemIndex(
						(IsHorizontal ? -padding : 0) + CollectionView.ViewPort.X + CollectionView.ViewPort.Width,
						(IsHorizontal ? 0 : -padding) + CollectionView.ViewPort.Y + CollectionView.ViewPort.Height);
				}
			}

			void OnAnimationEnd(object? sender, ScrollEventArgs e)
			{
				OnSnapRequest();
			}

			void OnSnapRequest()
			{
				if (CollectionView.SnapPointsType == SnapPointsType.None)
					return;

				double target;

				if (CollectionView.SnapPointsAlignment == SnapPointsAlignment.Start)
				{
					var index = LayoutManager.GetVisibleItemIndex(ViewPort.X, ViewPort.Y);
					var bound = LayoutManager.GetItemBound(index);
					var itemSize = IsHorizontal ? bound.Width : bound.Height;
					var itemStart = IsHorizontal ? bound.X : bound.Y;

					if (ViewPortStart - itemStart > itemSize / 2)
					{
						index = LayoutManager.NextRowItemIndex(index);
					}

					bound = LayoutManager.GetItemBound(index);
					target = IsHorizontal ? bound.X : bound.Y;
				}
				else if (CollectionView.SnapPointsAlignment == SnapPointsAlignment.Center)
				{
					var index = LayoutManager.GetVisibleItemIndex(ViewPort.X + (ViewPort.Width / 2), ViewPort.Y + (ViewPort.Height / 2));
					var bound = LayoutManager.GetItemBound(index);
					var itemSize = IsHorizontal ? bound.Width : bound.Height;
					var itemStart = IsHorizontal ? bound.X : bound.Y;

					if (ViewPortStart + (ViewPortSize / 2) - (itemStart + itemSize / 2) > (itemSize / 2))
					{
						index = LayoutManager.NextRowItemIndex(index);
					}

					bound = LayoutManager.GetItemBound(index);
					itemSize = IsHorizontal ? bound.Width : bound.Height;
					target = IsHorizontal ? bound.X : bound.Y;
					target -= (ViewPortSize - itemSize) / 2;
				}
				else
				{
					var index = LayoutManager.GetVisibleItemIndex(ViewPort.Right, ViewPort.Bottom);
					var bound = LayoutManager.GetItemBound(index);
					var itemSize = IsHorizontal ? bound.Width : bound.Height;
					var itemEnd = IsHorizontal ? bound.Right : bound.Bottom;

					if (itemEnd - ViewPortEnd > itemSize / 2)
					{
						index = LayoutManager.PreviousRowItemIndex(index);
					}

					bound = LayoutManager.GetItemBound(index);
					itemSize = IsHorizontal ? bound.Width : bound.Height;

					target = IsHorizontal ? bound.X : bound.Y;
					target -= (ViewPortSize - itemSize);
				}

				ScrollTo(target);
			}

			void ScrollTo(double target)
			{
				// it is a ScrollTo api that do not raise ScrollAnimationStarted/Ended event
				var scrollingSize = IsHorizontal ? ContentContainer.SizeWidth : ContentContainer.SizeHeight;

				if (scrollingSize - target < ViewPortSize)
				{
					target = scrollingSize - ViewPortSize;
				}

				if (target < 0)
				{
					target = 0;
				}

				var animation = new Animation();
				animation.Duration = 200;
				animation.AnimateTo(ContentContainer, IsHorizontal ? "PositionX" : "PositionY", -(float)target);
				animation.Finished += (s, e) => SnapRequestFinished?.Invoke(this, EventArgs.Empty);
				animation.Play();
				_snapAnimation = animation;
			}

		}

	}

}