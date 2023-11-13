#nullable disable
using static Microsoft.Maui.Controls.Handlers.Items.RecyclerView;
using ALayoutDirection = Microsoft.Maui.Controls.Handlers.Items.RecyclerView.LayoutDirection;
using AView = Gtk.Widget;

namespace Microsoft.Maui.Controls.Handlers.Items
{
	internal abstract class EdgeSnapHelper : NongreedySnapHelper
	{
		protected static OrientationHelper CreateOrientationHelper(RecyclerView.LayoutManager layoutManager)
		{
			return layoutManager.CanScrollHorizontally()
				? OrientationHelper.CreateHorizontalHelper(layoutManager)
				: OrientationHelper.CreateVerticalHelper(layoutManager);
		}

		protected static bool IsLayoutReversed(RecyclerView.LayoutManager layoutManager)
		{
			if (layoutManager.LayoutDirection == ALayoutDirection.Rtl)
				return true;

			if (layoutManager is LinearLayoutManager linearLayoutManager)
			{
				return linearLayoutManager.ReverseLayout;
			}

			return false;
		}

		protected int[] CalculateDistanceToFinalSnap(RecyclerView.LayoutManager layoutManager, AView targetView,
			int direction = 1)
		{
			var orientationHelper = CreateOrientationHelper(layoutManager);
			var isHorizontal = layoutManager.CanScrollHorizontally();
			var rtl = isHorizontal && IsLayoutReversed(layoutManager);

			var size = orientationHelper.GetDecoratedMeasurement(targetView);

			var hiddenPortion = size - VisiblePortion(targetView, orientationHelper, rtl);

			var distance = (rtl ? hiddenPortion : -hiddenPortion) * direction;

			return isHorizontal
				? new[] { distance, 1 }
				: new[] { 1, distance };
		}

		protected bool IsAtLeastHalfVisible(AView view, RecyclerView.LayoutManager layoutManager)
		{
			var orientationHelper = CreateOrientationHelper(layoutManager);
			var reversed = IsLayoutReversed(layoutManager);
			var isHorizontal = layoutManager.CanScrollHorizontally();

			// Find the size of the view (including margins, etc.)
			var size = orientationHelper.GetDecoratedMeasurement(view);

			var portionInViewPort = VisiblePortion(view, orientationHelper, reversed && isHorizontal);

			// Is the first visible view at least halfway on screen?
			return portionInViewPort >= size / 2;
		}

		protected abstract int VisiblePortion(AView view, OrientationHelper orientationHelper, bool rtl);

		public abstract int[] CalculateDistanceToFinalSnap(LayoutManager layoutManager, AView targetView);

		public abstract AView FindSnapView(LayoutManager layoutManager);

	}



}
