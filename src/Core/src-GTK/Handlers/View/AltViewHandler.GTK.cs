#nullable enable
using System;
using PlatformView = Microsoft.Maui.Platform.CustomAltView;

namespace Microsoft.Maui.Handlers
{
	public partial class AltViewHandler
	{
		partial void ConnectingHandler(PlatformView? platformView)
		{
			//if (platformView != null)
			//{
			//	platformView.GotFocus += OnPlatformViewGotFocus;
			//	platformView.LostFocus += OnPlatformViewLostFocus;
			//}
		}

		partial void DisconnectingHandler(PlatformView platformView)
		{
			UpdateIsFocused(false);

			//platformView.GotFocus -= OnPlatformViewGotFocus;
			//platformView.LostFocus -= OnPlatformViewLostFocus;
		}

		static partial void MappingFrame(IAltViewHandler handler, IView view)
		{
			// Both Clip and Shadow depend on the Control size.
			//handler.ToPlatform().UpdateClip(view);
			//handler.ToPlatform().UpdateShadow(view);
		}

		public static void MapTranslationX(IAltViewHandler handler, IView view)
		{
			//handler.ToPlatform().UpdateTransformation(view);
		}

		public static void MapTranslationY(IAltViewHandler handler, IView view)
		{
			//handler.ToPlatform().UpdateTransformation(view);
		}

		public static void MapScale(IAltViewHandler handler, IView view)
		{
			//handler.ToPlatform().UpdateTransformation(view);
		}

		public static void MapScaleX(IAltViewHandler handler, IView view)
		{
			//handler.ToPlatform().UpdateTransformation(view);
		}

		public static void MapScaleY(IAltViewHandler handler, IView view)
		{
			//handler.ToPlatform().UpdateTransformation(view);
		}

		public static void MapRotation(IAltViewHandler handler, IView view)
		{
			//handler.ToPlatform().UpdateTransformation(view);
		}

		public static void MapRotationX(IAltViewHandler handler, IView view)
		{
			//handler.ToPlatform().UpdateTransformation(view);
		}

		public static void MapRotationY(IAltViewHandler handler, IView view)
		{
			//handler.ToPlatform().UpdateTransformation(view);
		}

		public static void MapAnchorX(IAltViewHandler handler, IView view)
		{
			//handler.ToPlatform().UpdateTransformation(view);
		}

		public static void MapAnchorY(IAltViewHandler handler, IView view)
		{
			//handler.ToPlatform().UpdateTransformation(view);
		}

		public static void MapToolbar(IAltViewHandler handler, IView view)
		{
			if (view is IToolbarElement tb)
				MapToolbar(handler, tb);
		}

		internal static void MapToolbar(IElementHandler handler, IToolbarElement toolbarElement)
		{
			//_ = handler.MauiContext ?? throw new InvalidOperationException($"{nameof(handler.MauiContext)} null");

			//if (toolbarElement.Toolbar != null)
			//{
			//	var toolBar = toolbarElement.Toolbar.ToPlatform(handler.MauiContext);
			//	handler.MauiContext.GetNavigationRootManager().SetToolbar(toolBar);
			//}
		}

		public virtual bool NeedsContainer
		{
			get
			{
				if (VirtualView is IBorderView border)
					return border?.Shape != null || border?.Stroke != null;

				return false;
			}
		}

		//void OnPlatformViewGotFocus(object sender, RoutedEventArgs args)
		//{
		//	UpdateIsFocused(true);
		//}

		//void OnPlatformViewLostFocus(object sender, RoutedEventArgs args)
		//{
		//	UpdateIsFocused(false);
		//}

		void UpdateIsFocused(bool isFocused)
		{
			if (VirtualView == null)
				return;

			bool updateIsFocused = (isFocused && !VirtualView.IsFocused) || (!isFocused && VirtualView.IsFocused);

			if (updateIsFocused)
				VirtualView.IsFocused = isFocused;
		}
	}
}