#nullable disable
using System;
using ARect = Gdk.Rectangle;
using AViewCompat = Gtk.Widget;
using AView = Gtk.Widget;
using static Microsoft.Maui.Controls.Handlers.Items.RecyclerView;

namespace Microsoft.Maui.Controls.Handlers.Items;

public class RecyclerView : Gtk.Container
{

	public bool VerticalScrollBarEnabled { get; set; }

	public bool HorizontalScrollBarEnabled { get; set; }

	public class OnScrollListener : IDisposable
	{

		public void Dispose()
		{ }

		public virtual void OnScrolled(RecyclerView recyclerView, int dx, int dy)
		{
			throw new NotImplementedException();
		}

		protected virtual void Dispose(bool disposing)
		{
			throw new NotImplementedException();
		}

	}

	public void RemoveItemDecoration(ItemDecoration itemDecoration)
	{
		throw new NotImplementedException();
	}

	public void SetLayoutManager(LayoutManager linearLayoutManager)
	{
		throw new NotImplementedException();
	}

	public LayoutManager GetLayoutManager()
	{
		throw new NotImplementedException();
	}

	public class Adapter : IDisposable
	{

		public void Dispose()
		{
			throw new NotImplementedException();
		}

		public void RegisterAdapterDataObserver(AdapterDataObserver dataChangeObserver)
		{
			throw new NotImplementedException();
		}

		public void UnregisterAdapterDataObserver(AdapterDataObserver dataChangeObserver)
		{
			throw new NotImplementedException();
		}

		public void NotifyDataSetChanged()
		{
			throw new NotImplementedException();
		}

		public void NotifyItemChanged(int startIndex)
		{
			throw new NotImplementedException();
		}

		public void NotifyItemInserted(int startIndex)
		{
			throw new NotImplementedException();
		}

		public virtual int ItemCount { get; set; }

		public bool HasObservers { get; set; }

		public void NotifyItemRangeChanged(int startIndex, int changedCount)
		{
			throw new NotImplementedException();
		}

		public void NotifyItemMoved(int fromPosition, int toPosition)
		{
			throw new NotImplementedException();
		}

		public void NotifyItemRangeInserted(int startIndex, int count)
		{
			throw new NotImplementedException();
		}

		public void NotifyItemRangeRemoved(int startIndex, int count)
		{
			throw new NotImplementedException();
		}

		public void NotifyItemRemoved(int startIndex)
		{
			throw new NotImplementedException();
		}

		public bool IsDisposed()
		{
			throw new NotImplementedException();
		}

		public virtual void OnViewRecycled(RecyclerView.ViewHolder holder)
		{
			throw new NotImplementedException();
		}

		public virtual int GetItemViewType(int position)
		{
			throw new NotImplementedException();
		}

		protected virtual void Dispose(bool disposing)
		{
			throw new NotImplementedException();
		}

		public virtual long GetItemId(int position)
		{
			throw new NotImplementedException();
		}

		public virtual void OnBindViewHolder(ViewHolder holder, int position)
		{
			throw new NotImplementedException();
		}

		public virtual ViewHolder OnCreateViewHolder(AViewCompat parent, int viewType)
		{
			throw new NotImplementedException();
		}

	}

	public void SwapAdapter(Adapter adapter, bool p1)
	{
		throw new NotImplementedException();
	}

	public Adapter GetAdapter()
	{
		throw new NotImplementedException();
	}

	public void SetAdapter(Adapter o)
	{
		throw new NotImplementedException();
	}

	public void AddOnScrollListener(OnScrollListener recyclerViewScrollListener)
	{
		throw new NotImplementedException();
	}

	public void ClearOnScrollListeners()
	{
		throw new NotImplementedException();
	}

	public void AddItemDecoration(ItemDecoration itemDecoration)
	{
		throw new NotImplementedException();
	}

	public void SetPadding(int horizontalPadding, int verticalPadding, int i, int verticalPadding1)
	{
		throw new NotImplementedException();
	}

	public class AdapterDataObserver
	{

		public virtual void OnChanged()
		{
			throw new NotImplementedException();
		}

		public virtual void OnItemRangeInserted(int positionStart, int itemCount)
		{
			throw new NotImplementedException();
		}

		public virtual void OnItemRangeChanged(int positionStart, int itemCount, object payload)
		{
			throw new NotImplementedException();
		}

		public virtual void OnItemRangeChanged(int positionStart, int itemCount)
		{
			throw new NotImplementedException();
		}

		public virtual void OnItemRangeMoved(int fromPosition, int toPosition, int itemCount)
		{
			throw new NotImplementedException();
		}

		public virtual void OnItemRangeRemoved(int positionStart, int itemCount)
		{
			throw new NotImplementedException();
		}

	}

	public class ItemDecoration
	{

		public virtual void GetItemOffsets(ARect outRect, AView view, RecyclerView parent, State state)
		{
			throw new NotImplementedException();
		}


	}
	public new class State
	{ }

	protected virtual void OnLayout(bool changed, int i, int i1, int i2, int i3)
	{
		throw new NotImplementedException();


	}

	object GetRecycledViewPool()
	{
		throw new NotImplementedException();
	}

	public class ViewHolder
	{


		public ViewHolder() { }

		public ViewHolder(Gtk.Widget widget) { }

		public int ItemViewType { get; set; }

		public int BindingAdapterPosition { get; set; }

	}

	public class LayoutManager
	{

		public int ItemCount { get; set; }

		public LayoutDirection LayoutDirection { get; set; }

		public bool CanScrollHorizontally()
		{
			throw new NotImplementedException();
		}

		public bool CanScrollVertically()
		{
			throw new NotImplementedException();
		}

	}

	public class LinearLayoutManager : LayoutManager
	{

		public static object Horizontal { get; set; }

		public static object Vertical { get; set; }

		public bool ReverseLayout { get; set; }

		public object Orientation { get; set; }



		public ItemContentView FindViewByPosition(int i)
		{
			throw new NotImplementedException();
		}

	}
	
	public class GridLayoutManager : LayoutManager
	{

		public GridLayoutManager(int span, object horizontal, bool b)
		{
			throw new NotImplementedException();
		}

		public int SpanCount { get; set; }

		internal void SetSpanSizeLookup(GridLayoutSpanSizeLookup gridLayoutSpanSizeLookup)
		{
			throw new NotImplementedException();
		}

		internal class SpanSizeLookup
		{

			public virtual int GetSpanSize(int position)
			{
				throw new NotImplementedException();
			}

		}

	}

	public enum LayoutDirection
	{

		Rtl

	}
	
	public class OrientationHelper
	{

		public static OrientationHelper CreateHorizontalHelper(LayoutManager layoutManager)
		{
			throw new NotImplementedException();
		}

		public static OrientationHelper CreateVerticalHelper(LayoutManager layoutManager)
		{
			throw new NotImplementedException();
		}

		public int GetDecoratedMeasurement(AViewCompat view)
		{
			throw new NotImplementedException();
		}

		public int GetDecoratedEnd(AViewCompat targetView)
		{
			throw new NotImplementedException();
		}

		public int GetDecoratedStart(AViewCompat targetView)
		{
			throw new NotImplementedException();
		}

		public int TotalSpace { get; set; }

	}

	public void RemoveOnScrollListener(OnScrollListener initialScrollListener)
	{
		throw new NotImplementedException();
	}

}