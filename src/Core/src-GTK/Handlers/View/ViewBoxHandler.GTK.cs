#nullable enable
using System;
using Microsoft.Maui.Graphics;
using PlatformView = Gtk.VBox;

namespace Microsoft.Maui.Handlers
{
	public partial class ViewBoxHandler<TVirtualView, TPlatformView> : IPlatformViewBoxHandler
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

		static partial void MappingFrame(IViewHandler handler, IView view)
		{
			// Both Clip and Shadow depend on the Control size.
			//handler.ToPlatform().UpdateClip(view);
			//handler.ToPlatform().UpdateShadow(view);
		}

		public static void MapTranslationX(IViewHandler handler, IView view)
		{
			//handler.ToPlatform().UpdateTransformation(view);
		}

		public static void MapTranslationY(IViewHandler handler, IView view)
		{
			//handler.ToPlatform().UpdateTransformation(view);
		}

		public static void MapScale(IViewHandler handler, IView view)
		{
			//handler.ToPlatform().UpdateTransformation(view);
		}

		public static void MapScaleX(IViewHandler handler, IView view)
		{
			//handler.ToPlatform().UpdateTransformation(view);
		}

		public static void MapScaleY(IViewHandler handler, IView view)
		{
			//handler.ToPlatform().UpdateTransformation(view);
		}

		public static void MapRotation(IViewHandler handler, IView view)
		{
			//handler.ToPlatform().UpdateTransformation(view);
		}

		public static void MapRotationX(IViewHandler handler, IView view)
		{
			//handler.ToPlatform().UpdateTransformation(view);
		}

		public static void MapRotationY(IViewHandler handler, IView view)
		{
			//handler.ToPlatform().UpdateTransformation(view);
		}

		public static void MapAnchorX(IViewHandler handler, IView view)
		{
			//handler.ToPlatform().UpdateTransformation(view);
		}

		public static void MapAnchorY(IViewHandler handler, IView view)
		{
			//handler.ToPlatform().UpdateTransformation(view);
		}

		public static void MapToolbar(IViewHandler handler, IView view)
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

		//public PlatformView? PlatformView => throw new NotImplementedException();

		//public PlatformView? ContainerView => throw new NotImplementedException();

		//public bool HasContainer { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

		//object? IViewHandler.ContainerView => throw new NotImplementedException();

		//public IView? VirtualView => throw new NotImplementedException();

		//object? IElementHandler.PlatformView => throw new NotImplementedException();

		//IElement? IElementHandler.VirtualView => throw new NotImplementedException();

		//public IMauiContext? MauiContext => throw new NotImplementedException();

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

		public Size GetDesiredSize(double widthConstraint, double heightConstraint)
		{
			throw new NotImplementedException();
		}

		public void PlatformArrange(Rect frame)
		{
			throw new NotImplementedException();
		}

		public void SetMauiContext(IMauiContext mauiContext)
		{
			throw new NotImplementedException();
		}

		public void ClearVirtualView()
		{
			throw new NotImplementedException();
		}

		public void SetVirtualView(IElement view)
		{
			throw new NotImplementedException();
		}

		public void UpdateValue(string property)
		{
			throw new NotImplementedException();
		}

		public void Invoke(string command, object? args = null)
		{
			throw new NotImplementedException();
		}

		public void DisconnectHandler()
		{
			throw new NotImplementedException();
		}
	}
}