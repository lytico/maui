using System;
using Microsoft.AspNetCore.Components;

namespace Microsoft.Maui.Razor {

    public static class HandlerExtensions {

        public static IComponent ToNative(this IElement view, IMauiContext context) {
            _ = view ?? throw new ArgumentNullException(nameof(view));
            _ = context ?? throw new ArgumentNullException(nameof(context));

            //This is how MVU works. It collapses views down
            if (view is IReplaceableView ir)
                view = ir.ReplacedView;

            var handler = view.Handler;

            if (handler == null)
                handler = context.Handlers.GetHandler(view.GetType());

            if (handler == null)
                throw new Exception($"Handler not found for view {view}.");

            handler.SetMauiContext(context);

            view.Handler = handler;

            if (handler.VirtualView != view)
                handler.SetVirtualView(view);

            if (handler.NativeView is not IComponent result) {
                throw new InvalidOperationException($"Unable to convert {view} to {typeof(IComponent)}");
            }

            return result;
        }

    }

}