using System;
using Gtk;

namespace Microsoft.Maui
{

	public static class HandlerExtensions
	{
		public static Widget ToNative(this IView view, IMauiContext context)
		{
			_ = view ?? throw new ArgumentNullException(nameof(view));
			_ = context ?? throw new ArgumentNullException(nameof(context));

			var handler = view.Handler;

			if (handler == null)
			{
				handler = context.Handlers.GetHandler(view.GetType());

				if (handler == null)
					throw new Exception($"Handler not found for view {view}");

				handler.SetMauiContext(context);

				view.Handler = handler;
			}

			handler.SetVirtualView(view);

			if (!(handler.NativeView is Widget result))
			{
				throw new InvalidOperationException($"Unable to convert {view} to {typeof(Widget)}");
			}

			return result;
		}

		public static Gtk.Requisition ToGtkRequisition(this Graphics.Size size) =>
			new()
			{
				Height = (int)size.Height,
				Width = (int)size.Width
			};

	}

}