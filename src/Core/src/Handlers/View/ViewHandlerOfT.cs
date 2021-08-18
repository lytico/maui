using System;
#if __IOS__ || MACCATALYST
using NativeView = UIKit.UIView;
#elif MONOANDROID
using NativeView = Android.Views.View;
#elif GTK
using NativeView = Gtk.Widget;
#elif WINDOWS
using NativeView = Microsoft.UI.Xaml.FrameworkElement;
#elif NETSTANDARD || (NET6_0 && !IOS && !ANDROID)
using NativeView = System.Object;
#endif

namespace Microsoft.Maui.Handlers
{
	public abstract partial class ViewHandler<TVirtualView, TNativeView> : ViewHandler, IViewHandler
		where TVirtualView : class, IView
#if !NETSTANDARD || IOS || ANDROID || GTK || WINDOWS
		where TNativeView : NativeView
#else
		where TNativeView : class
#endif
	{
		[HotReload.OnHotReload]
		internal static void OnHotReload()
		{
		}

		protected ViewHandler(IPropertyMapper mapper, CommandMapper? commandMapper = null)
			: base(mapper, commandMapper)
		{
		}

		public new TNativeView NativeView
		{
			get => (TNativeView?)base.NativeView ?? throw new InvalidOperationException($"NativeView cannot be null here");
			private set => base.NativeView = value;
		}

		public new TVirtualView VirtualView
		{
			get => (TVirtualView?)base.VirtualView ?? throw new InvalidOperationException($"VirtualView cannot be null here");
			private protected set => base.VirtualView = value;
		}

		IView? IViewHandler.VirtualView => base.VirtualView;

		IElement? IElementHandler.VirtualView => base.VirtualView;

		object? IElementHandler.NativeView => base.NativeView;

		public virtual void SetVirtualView(IView view) =>
			base.SetVirtualView(view);

		public sealed override void SetVirtualView(IElement view) =>
			SetVirtualView((IView)view);

		protected abstract TNativeView CreateNativeView();

		protected virtual void ConnectHandler(TNativeView nativeView)
		{
		}

		protected virtual void DisconnectHandler(TNativeView nativeView)
		{
		}

		private protected override NativeView OnCreateNativeView() =>
			CreateNativeView();

		private protected override void OnConnectHandler(NativeView nativeView) =>
			ConnectHandler((TNativeView)nativeView);

		private protected override void OnDisconnectHandler(NativeView nativeView) =>
			DisconnectHandler((TNativeView)nativeView);
	}
}