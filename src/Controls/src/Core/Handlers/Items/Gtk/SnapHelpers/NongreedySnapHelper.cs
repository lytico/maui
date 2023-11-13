﻿#nullable disable
using System;
using Gtk;
using static Microsoft.Maui.Controls.Handlers.Items.RecyclerView;

namespace Microsoft.Maui.Controls.Handlers.Items
{

	public abstract class SnapHelper:IDisposable
	{

		public virtual void AttachToRecyclerView(RecyclerView recyclerView)
		{
			throw new System.NotImplementedException();
		}

		protected virtual void Dispose(bool disposing)
		{
			throw new System.NotImplementedException();
		}

		public virtual int FindTargetSnapPosition(LayoutManager layoutManager, int velocityX, int velocityY)
		{
			throw new System.NotImplementedException();
		}

		public void Dispose()
		{
			Dispose(true);
		}

	}

	public abstract class LinearSnapHelper : SnapHelper
	{ }

	internal abstract class NongreedySnapHelper : LinearSnapHelper
	{

		// Flag to indicate that the user has scrolled the view, so we can start snapping
		// (otherwise, this would start trying to snap the view as soon as we attached it to the RecyclerView)
		protected bool CanSnap { get; set; }

		bool _disposed;
		RecyclerView _recyclerView;
		InitialScrollListener _initialScrollListener;

		public override void AttachToRecyclerView(RecyclerView recyclerView)
		{
			base.AttachToRecyclerView(recyclerView);

			_recyclerView = recyclerView;

			if (_recyclerView != null)
			{
				StartListeningForScroll();
			}
		}

		protected override void Dispose(bool disposing)
		{
			if (_disposed)
			{
				return;
			}

			_disposed = true;

			if (disposing)
			{
				if (_recyclerView != null)
				{
					StopListeningForScroll();
					_initialScrollListener?.Dispose();
				}
			}

			base.Dispose(disposing);
		}

		void StartListeningForScroll()
		{
			_initialScrollListener = new InitialScrollListener(this);
			_recyclerView.AddOnScrollListener(_initialScrollListener);
		}

		void StopListeningForScroll()
		{
			if (_recyclerView != null && _initialScrollListener != null)
			{
				_recyclerView.RemoveOnScrollListener(_initialScrollListener);
			}
		}

		class InitialScrollListener : RecyclerView.OnScrollListener
		{

			readonly NongreedySnapHelper _helper;

			public InitialScrollListener(NongreedySnapHelper helper) => _helper = helper;

			public override void OnScrolled(RecyclerView recyclerView, int dx, int dy)
			{
				base.OnScrolled(recyclerView, dx, dy);
				_helper.CanSnap = true;
				_helper.StopListeningForScroll();
			}

		}

		public virtual Widget FindSnapView(LayoutManager layoutManager)
		{
			throw new System.NotImplementedException();
		}

	}

}