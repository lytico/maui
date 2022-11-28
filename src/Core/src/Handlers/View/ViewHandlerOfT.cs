using System;
#if IOS || MACCATALYST
using PlatformView = UIKit.UIView;
#elif MONOANDROID
using PlatformView = Android.Views.View;
#elif WINDOWS && __GTK__
using PlatformView = System.Object;
#elif WINDOWS && !__GTK__
using PlatformView = Microsoft.UI.Xaml.FrameworkElement;
#elif TIZEN
using PlatformView = Tizen.NUI.BaseComponents.View;
#elif (NETSTANDARD || !PLATFORM) || (NET6_0_OR_GREATER && !IOS && !ANDROID && !TIZEN)
using PlatformView = System.Object;
#endif

namespace Microsoft.Maui.Handlers
{
	public abstract partial class ViewHandler<TVirtualView, TPlatformView> : ViewHandler, IViewHandler
		where TVirtualView : class, IView
#if !(NETSTANDARD || !PLATFORM) || IOS || ANDROID || WINDOWS || TIZEN
#if __GTK__
		where TPlatformView : class
#else
		where TPlatformView : PlatformView
#endif
#else
		where TPlatformView : class
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

		public new TPlatformView PlatformView
		{
			get => (TPlatformView?)base.PlatformView ?? throw new InvalidOperationException($"PlatformView cannot be null here");
			private protected set => base.PlatformView = value;
		}

		public new TVirtualView VirtualView
		{
			get => (TVirtualView?)base.VirtualView ?? throw new InvalidOperationException($"VirtualView cannot be null here");
			private protected set => base.VirtualView = value;
		}

		IView? IViewHandler.VirtualView => base.VirtualView;

		IElement? IElementHandler.VirtualView => base.VirtualView;

		object? IElementHandler.PlatformView => base.PlatformView;

		public virtual void SetVirtualView(IView view) =>
			base.SetVirtualView(view);

		public sealed override void SetVirtualView(IElement view) =>
			SetVirtualView((IView)view);

		public static Func<ViewHandler<TVirtualView, TPlatformView>, TPlatformView>? PlatformViewFactory { get; set; }

#if __GTK__
		protected abstract TPlatformView CreatePlatformView(IView button);
#else
		protected abstract TPlatformView CreatePlatformView();
#endif

		protected virtual void ConnectHandler(TPlatformView platformView)
		{
		}

		protected virtual void DisconnectHandler(TPlatformView platformView)
		{
		}

		private protected override PlatformView OnCreatePlatformView()
		{
#if __GTK__
			if (PlatformViewFactory == null)
			{
				if (base.VirtualView == null)
				{
					return new MauiView();
				}
				else
				{
					 return CreatePlatformView(base.VirtualView);
				}
			}
			
			return PlatformViewFactory.Invoke(this);
#else
			return PlatformViewFactory?.Invoke(this) ?? CreatePlatformView();
#endif
		}

		private protected override void OnConnectHandler(PlatformView platformView) =>
			ConnectHandler((TPlatformView)platformView);

		private protected override void OnDisconnectHandler(PlatformView platformView) =>
			DisconnectHandler((TPlatformView)platformView);
	}
}