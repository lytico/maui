using System;
using Microsoft.Maui.Graphics;
using GColor = Gdk.Color;
//using AShapeDrawable = Android.Graphics.Drawables.ShapeDrawable;
//using AShapes = Android.Graphics.Drawables.Shapes;
//using AView = Android.Views.View;
using Color = Microsoft.Maui.Graphics.Color;
using System.Collections.Generic;

namespace Microsoft.Maui.Platform
{
	public class MauiPageControl : MauiView
	{
		const int DefaultPadding = 4;

		public MauiPageControl()
		{
			FixedWidget = new Gtk.Fixed();
			Add(FixedWidget);
		}

		public Gtk.Fixed FixedWidget { get; set; } = null!;

		//Drawable? _currentPageShape;
		//Drawable? _pageShape;

		IIndicatorView? _indicatorView;

		public void SetIndicatorView(IIndicatorView? indicatorView)
		{
			_indicatorView = indicatorView;
			if (indicatorView == null)
			{
				RemoveViews(0);
			}
		}

		public void ResetIndicators()
		{
			//_pageShape = null;
			//_currentPageShape = null;

			if ((_indicatorView as ITemplatedIndicatorView)?.IndicatorsLayoutOverride == null)
				UpdateShapes();
			else
				UpdateIndicatorTemplate((_indicatorView as ITemplatedIndicatorView)?.IndicatorsLayoutOverride);

			UpdatePosition();
		}

		public void UpdatePosition()
		{
			//var index = GetIndexFromPosition();
			//var count = ChildCount;
			//for (int i = 0; i < count; i++)
			//{
			//	ImageView? view = GetChildAt(i) as ImageView;
			//	if (view == null)
			//		continue;
			//	var drawableToUse = index == i ? _currentPageShape : _pageShape;
			//	if (drawableToUse != view.Drawable)
			//		view.SetImageDrawable(drawableToUse);
			//}
		}

		public void UpdateIndicatorCount()
		{
			//if (_indicatorView == null)
			//	return;

			//var index = GetIndexFromPosition();

			//var count = _indicatorView.GetMaximumVisible();

			//var childCount = ChildCount;

			//for (int i = childCount; i < count; i++)
			//{
			//	var imageView = new ImageView(Context)
			//	{
			//		Tag = i
			//	};

			//	if (Orientation == Orientation.Horizontal)
			//		imageView.SetPadding((int)Context.ToPixels(DefaultPadding), 0, (int)Context.ToPixels(DefaultPadding), 0);
			//	else
			//		imageView.SetPadding(0, (int)Context.ToPixels(DefaultPadding), 0, (int)Context.ToPixels(DefaultPadding));

			//	imageView.SetImageDrawable(index == i ? _currentPageShape : _pageShape);

			//	imageView.SetOnClickListener(new TEditClickListener(view =>
			//	{
			//		if (view?.Tag != null)
			//		{
			//			var position = (int)view.Tag;
			//			_indicatorView.Position = position;
			//		}
			//	}));

			//	AddView(imageView);
			//}

			//RemoveViews(count);
		}

		void UpdateIndicatorTemplate(ILayout? layout)
		{
			//if (layout == null || _indicatorView?.Handler?.MauiContext == null)
			//	return;

			//AView? handler = layout.ToPlatform(_indicatorView.Handler.MauiContext);

			//RemoveAllViews();
			//AddView(handler);
		}

		void UpdateShapes()
		{
			//if (_currentPageShape != null || _indicatorView == null)
			//	return;

			//var indicatorColor = _indicatorView.IndicatorColor;

			//if (indicatorColor is SolidPaint indicatorPaint)
			//{
			//	if (indicatorPaint.Color is Color c)
			//		_pageShape = GetShape(c.ToPlatform());

			//}
			//var indicatorPositionColor = _indicatorView.SelectedIndicatorColor;
			//if (indicatorPositionColor is SolidPaint indicatorPositionPaint)
			//{
			//	if (indicatorPositionPaint.Color is Color c)
			//		_currentPageShape = GetShape(c.ToPlatform());
			//}
		}

//		Drawable? GetShape(AColor color)
//		{
//			if (_indicatorView == null || Context == null)
//				return null;

//			AShapeDrawable shape;

//			if (_indicatorView.IsCircleShape())
//				shape = new AShapeDrawable(new AShapes.OvalShape());
//			else
//				shape = new AShapeDrawable(new AShapes.RectShape());

//			var indicatorSize = _indicatorView.IndicatorSize;

//			shape.SetIntrinsicHeight((int)Context.ToPixels(indicatorSize));
//			shape.SetIntrinsicWidth((int)Context.ToPixels(indicatorSize));

//			if (shape.Paint != null)
//#pragma warning disable CA1416 // https://github.com/xamarin/xamarin-android/issues/6962
//				shape.Paint.Color = color;
//#pragma warning restore CA1416

//			return shape;
		//}

		int GetIndexFromPosition()
		{
			if (_indicatorView == null)
				return 0;

			var maxVisible = _indicatorView.GetMaximumVisible();
			var position = _indicatorView.Position;
			return Math.Max(0, position >= maxVisible ? maxVisible - 1 : position);
		}

		void RemoveViews(int startAt)
		{
			List<Gtk.Widget> childList = new List<Gtk.Widget>();
			foreach (var child in FixedWidget.Children)
			{
				childList.Add(child);
			}
			foreach (var child in childList)
			{
				FixedWidget.Remove(child);
			}
		}

		//class TEditClickListener : Java.Lang.Object, IOnClickListener
		//{
		//	Action<AView?>? _command;

		//	public TEditClickListener(Action<AView?> command)
		//	{
		//		_command = command;
		//	}

		//	public void OnClick(AView? v)
		//	{
		//		_command?.Invoke(v);
		//	}
		//	protected override void Dispose(bool disposing)
		//	{
		//		if (disposing)
		//		{
		//			_command = null;
		//		}
		//		base.Dispose(disposing);
		//	}
		//}
	}
}
