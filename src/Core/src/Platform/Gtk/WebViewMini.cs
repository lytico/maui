// adopted from: https://github.com/mono/xwt/blob/main/Xwt.Gtk/Xwt.GtkBackend/GtkWebKitMini.cs

using System;
using Gtk;
using static Microsoft.Maui.GtkInterop.DllImportWebkit;

namespace Microsoft.Maui
{

	public class WebView : Container, IScrollable
	{

		public WebView(IntPtr raw) : base(raw)
		{ }

		public WebView() : base(IntPtr.Zero)
		{
			Raw = webkit_web_view_new();
		}

		bool IScrollable.GetBorder(out Border border)
		{
			border = Border.Zero;

			return true;
		}

		public void LoadUri(string uri)
		{
			var nativeUri = GLib.Marshaller.StringToPtrGStrdup(uri);
			webkit_web_view_load_uri(Handle, nativeUri);
			GLib.Marshaller.Free(nativeUri);
		}

		public string Uri
		{
			get
			{
				var rawRet = webkit_web_view_get_uri(Handle);
				var ret = GLib.Marshaller.Utf8PtrToString(rawRet);

				return ret;
			}
		}

		public double LoadProgress
		{
			get
			{
				var ret = webkit_web_view_get_progress(Handle);

				return ret;
			}
		}

		public bool FullContentZoom
		{
			get
			{
				var rawRet = webkit_web_view_get_full_content_zoom(Handle);
				var ret = rawRet;

				return ret;
			}
			set
			{
				webkit_web_view_set_full_content_zoom(Handle, value);
			}
		}

		[GLib.Property("self-scrolling")]
		public bool SelfScrolling
		{
			get
			{
				using var property = GetProperty("self-scrolling");

				return (bool)property.Val;

			}
			set
			{
				using var val = new GLib.Value(value);

				SetProperty("self-scrolling", val);

			}
		}

		[GLib.Property("transparent")]
		public bool Transparent
		{
			get
			{
				using var property = GetProperty("transparent");

				return (bool)property.Val;

			}
			set
			{
				using var val = new GLib.Value(value);

				SetProperty("transparent", val);

			}
		}

		[GLib.Property("hadjustment")]
		public Adjustment Hadjustment
		{
			get
			{
				using var property = GetProperty("hadjustment");

				return (property.Val as Adjustment)!;

			}
			set
			{
				using var val = new GLib.Value(value);

				SetProperty("hadjustment", val);

			}
		}

		[GLib.Property("vadjustment")]
		public Adjustment Vadjustment
		{
			get
			{
				using var property = GetProperty("vadjustment");

				return (property.Val as Adjustment)!;

			}
			set
			{
				using var val = new GLib.Value(value);

				SetProperty("vadjustment", val);

			}
		}

		[GLib.Property("hscroll-policy")]
		public ScrollablePolicy HscrollPolicy
		{
			get
			{
				using var property = GetProperty("hscroll-policy");

				return (ScrollablePolicy)property.Val;

			}
			set
			{
				using var val = new GLib.Value(value);

				SetProperty("hscroll-policy", val);

			}
		}

		[GLib.Property("vscroll-policy")]
		public ScrollablePolicy VscrollPolicy
		{
			get
			{
				using var property = GetProperty("vscroll-policy");

				return (ScrollablePolicy)property.Val;

			}
			set
			{
				using var val = new GLib.Value(value);

				SetProperty("vscroll-policy", val);

			}
		}

		public void StopLoading()
		{
			webkit_web_view_stop_loading(Handle);
		}

		public void Reload()
		{
			webkit_web_view_reload(Handle);
		}

		public bool CanGoBack()
		{
			var rawRet = webkit_web_view_can_go_back(Handle);
			var ret = rawRet;

			return ret;
		}

		public void GoBack()
		{
			webkit_web_view_go_back(Handle);
		}

		public void GoForward()
		{
			webkit_web_view_go_forward(Handle);
		}

		public bool CanGoForward()
		{
			var rawRet = webkit_web_view_can_go_forward(Handle);
			var ret = rawRet;

			return ret;
		}

		public void LoadHtmlString(string content, string baseUri)
		{
			var nativeContent = GLib.Marshaller.StringToPtrGStrdup(content);
			var nativeBaseUri = GLib.Marshaller.StringToPtrGStrdup(baseUri);
			webkit_web_view_load_string(Handle, nativeContent, IntPtr.Zero, IntPtr.Zero, nativeBaseUri);
			GLib.Marshaller.Free(nativeContent);
			GLib.Marshaller.Free(nativeBaseUri);
		}

		public string Title
		{
			get
			{
				var rawRet = webkit_web_view_get_title(Handle);
				var ret = GLib.Marshaller.Utf8PtrToString(rawRet);

				return ret;
			}
		}

		[GLib.Signal("load-finished")]
		public event EventHandler<GLib.SignalArgs> LoadFinished
		{
			add
			{
				AddSignalHandler("load-finished", value, typeof(GLib.SignalArgs));
			}
			remove
			{
				RemoveSignalHandler("load-finished", value);
			}
		}

		[GLib.Signal("load-started")]
		public event EventHandler<GLib.SignalArgs> LoadStarted
		{
			add
			{
				AddSignalHandler("load-started", value, typeof(GLib.SignalArgs));
			}
			remove
			{
				RemoveSignalHandler("load-started", value);
			}
		}

		[GLib.Signal("navigation-requested")]
		public event EventHandler<NavigationRequestedArgs> NavigationRequested
		{
			add
			{
				AddSignalHandler("navigation-requested", value, typeof(NavigationRequestedArgs));
			}
			remove
			{
				RemoveSignalHandler("navigation-requested", value);
			}
		}

		[GLib.Signal("title-changed")]
		public event EventHandler<TitleChangedArgs> TitleChanged
		{
			add
			{
				AddSignalHandler("title-changed", value, typeof(TitleChangedArgs));
			}
			remove
			{
				RemoveSignalHandler("title-changed", value);
			}
		}

		[GLib.Signal("context-menu")]
		public event EventHandler<ContextMenuArgs> ContextMenu
		{
			add
			{
				AddSignalHandler("context-menu", value, typeof(ContextMenuArgs));
			}
			remove
			{
				RemoveSignalHandler("context-menu", value);
			}
		}

		static WebView()
		{
			Initialize();
		}

		static bool Initialized = false;

		internal static void Initialize()
		{
			if (Initialized)
				return;

			Initialized = true;
			GLib.GType.Register(GType, typeof(WebView));
			GLib.GType.Register(NetworkRequest.GType, typeof(NetworkRequest));
		}

		public new static GLib.GType GType
		{
			get
			{
				var rawRet = webkit_web_view_get_type();
				var ret = new GLib.GType(rawRet);

				return ret;
			}
		}

	}

	public sealed class NetworkRequest : GLib.Object
	{

		public NetworkRequest(IntPtr raw) : base(raw) { }

		public NetworkRequest(string uri) : base(IntPtr.Zero)
		{
			var nativeUri = GLib.Marshaller.StringToPtrGStrdup(uri);
			Raw = webkit_network_request_new(nativeUri);
			GLib.Marshaller.Free(nativeUri);
		}

		public new static GLib.GType GType
		{
			get
			{
				var rawRet = webkit_network_request_get_type();
				var ret = new GLib.GType(rawRet);

				return ret;
			}
		}

		public string Uri
		{
			get
			{
				var rawRet = webkit_network_request_get_uri(Handle);
				var ret = GLib.Marshaller.Utf8PtrToString(rawRet);

				return ret;
			}
			set
			{
				var nativeValue = GLib.Marshaller.StringToPtrGStrdup(value);
				webkit_network_request_set_uri(Handle, nativeValue);
				GLib.Marshaller.Free(nativeValue);
			}
		}

		static NetworkRequest()
		{
			WebView.Initialize();
		}

	}

	public enum NavigationResponse
	{

		Accept,
		Ignore,
		Download,

	}

	public class LoadProgressChangedArgs : GLib.SignalArgs
	{

		public int Progress => (int)Args[0];

	}

	public class NavigationRequestedArgs : GLib.SignalArgs
	{

		public IntPtr Frame => (IntPtr)Args[0];

		public NetworkRequest Request => (NetworkRequest)Args[1];

	}

	public class TitleChangedArgs : GLib.SignalArgs
	{

		public string Title => (string)Args[1];

	}

	public class ContextMenuArgs : GLib.SignalArgs
	{

		public Widget DefaultMenu
		{
			get => (Args[0] as Widget)!;
			set { Args[0] = value; }
		}

		public GLib.Object HitTestResult => (Args[1] as GLib.Object)!;

		public bool TriggeredWithKeyboard => (bool)Args[2];

	}

}