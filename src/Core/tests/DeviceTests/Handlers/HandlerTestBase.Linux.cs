using System;
using Gtk;

namespace Microsoft.Maui.DeviceTests
{

	public static class HandlerExtensions
	{

		public static TWidget WidgetOf<TWidget>(this IViewHandler it) where TWidget : Widget
			=> ((it as INativeViewHandler)?.NativeView is TWidget r) ? r : default;

	}

	public partial class HandlerTestBase<THandler, TStub>
	{

		protected THandler CreateHandler(IView view)
		{
			var handler = Activator.CreateInstance<THandler>();
			handler.SetMauiContext(MauiContext);

			handler.SetVirtualView(view);
			view.Handler = handler;

			return handler;
		}

		protected string GetAutomationId(IViewHandler viewHandler) =>
			throw new NotImplementedException();

		protected string GetSemanticDescription(IViewHandler viewHandler) =>
			throw new NotImplementedException();

		protected SemanticHeadingLevel GetSemanticHeading(IViewHandler viewHandler)
		{

			return viewHandler.VirtualView.Semantics.HeadingLevel;
		}

	}

}