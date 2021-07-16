using System;
using System.Runtime.InteropServices;

// ReSharper disable CA2211

// ReSharper disable InconsistentNaming

namespace Microsoft.Maui.GtkInterop
{

	public static class DllImportWebkit
	{

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate IntPtr d_webkit_network_request_new(IntPtr uri);

		public static d_webkit_network_request_new webkit_network_request_new = FuncLoader.LoadFunction<d_webkit_network_request_new>(FuncLoader.GetProcAddress(GLibrary.Load(Library.Webkit), "webkit_network_request_new"));

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate IntPtr d_webkit_network_request_get_type();

		public static d_webkit_network_request_get_type webkit_network_request_get_type = FuncLoader.LoadFunction<d_webkit_network_request_get_type>(FuncLoader.GetProcAddress(GLibrary.Load(Library.Webkit), "webkit_network_request_get_type"));

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate IntPtr d_webkit_network_request_get_uri(IntPtr raw);

		public static d_webkit_network_request_get_uri webkit_network_request_get_uri = FuncLoader.LoadFunction<d_webkit_network_request_get_uri>(FuncLoader.GetProcAddress(GLibrary.Load(Library.Webkit), "webkit_network_request_get_uri"));

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate void d_webkit_network_request_set_uri(IntPtr raw, IntPtr uri);

		public static d_webkit_network_request_set_uri webkit_network_request_set_uri = FuncLoader.LoadFunction<d_webkit_network_request_set_uri>(FuncLoader.GetProcAddress(GLibrary.Load(Library.Webkit), "webkit_network_request_set_uri"));

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate IntPtr d_webkit_web_view_new();

		public static d_webkit_web_view_new webkit_web_view_new = FuncLoader.LoadFunction<d_webkit_web_view_new>(FuncLoader.GetProcAddress(GLibrary.Load(Library.Webkit), "webkit_web_view_new"));

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate IntPtr d_webkit_web_view_get_type();

		public static d_webkit_web_view_get_type webkit_web_view_get_type = FuncLoader.LoadFunction<d_webkit_web_view_get_type>(FuncLoader.GetProcAddress(GLibrary.Load(Library.Webkit), "webkit_web_view_get_type"));

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate void d_webkit_web_view_load_uri(IntPtr raw, IntPtr uri);

		public static d_webkit_web_view_load_uri webkit_web_view_load_uri = FuncLoader.LoadFunction<d_webkit_web_view_load_uri>(FuncLoader.GetProcAddress(GLibrary.Load(Library.Webkit), "webkit_web_view_load_uri"));

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate IntPtr d_webkit_web_view_get_uri(IntPtr raw);

		public static d_webkit_web_view_get_uri webkit_web_view_get_uri = FuncLoader.LoadFunction<d_webkit_web_view_get_uri>(FuncLoader.GetProcAddress(GLibrary.Load(Library.Webkit), "webkit_web_view_get_uri"));

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate bool d_webkit_web_view_get_full_content_zoom(IntPtr raw);

		public static d_webkit_web_view_get_full_content_zoom webkit_web_view_get_full_content_zoom = FuncLoader.LoadFunction<d_webkit_web_view_get_full_content_zoom>(FuncLoader.GetProcAddress(GLibrary.Load(Library.Webkit), "webkit_web_view_get_full_content_zoom"));

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate void d_webkit_web_view_set_full_content_zoom(IntPtr raw, bool full_content_zoom);

		public static d_webkit_web_view_set_full_content_zoom webkit_web_view_set_full_content_zoom = FuncLoader.LoadFunction<d_webkit_web_view_set_full_content_zoom>(FuncLoader.GetProcAddress(GLibrary.Load(Library.Webkit), "webkit_web_view_set_full_content_zoom"));

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate void d_webkit_web_view_stop_loading(IntPtr raw);

		public static d_webkit_web_view_stop_loading webkit_web_view_stop_loading = FuncLoader.LoadFunction<d_webkit_web_view_stop_loading>(FuncLoader.GetProcAddress(GLibrary.Load(Library.Webkit), "webkit_web_view_stop_loading"));

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate void d_webkit_web_view_reload(IntPtr raw);

		public static d_webkit_web_view_reload webkit_web_view_reload = FuncLoader.LoadFunction<d_webkit_web_view_reload>(FuncLoader.GetProcAddress(GLibrary.Load(Library.Webkit), "webkit_web_view_reload"));

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate bool d_webkit_web_view_can_go_back(IntPtr raw);

		public static d_webkit_web_view_can_go_back webkit_web_view_can_go_back = FuncLoader.LoadFunction<d_webkit_web_view_can_go_back>(FuncLoader.GetProcAddress(GLibrary.Load(Library.Webkit), "webkit_web_view_can_go_back"));

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate void d_webkit_web_view_go_back(IntPtr raw);

		public static d_webkit_web_view_go_back webkit_web_view_go_back = FuncLoader.LoadFunction<d_webkit_web_view_go_back>(FuncLoader.GetProcAddress(GLibrary.Load(Library.Webkit), "webkit_web_view_go_back"));

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate bool d_webkit_web_view_can_go_forward(IntPtr raw);

		public static d_webkit_web_view_can_go_forward webkit_web_view_can_go_forward = FuncLoader.LoadFunction<d_webkit_web_view_can_go_forward>(FuncLoader.GetProcAddress(GLibrary.Load(Library.Webkit), "webkit_web_view_can_go_forward"));

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate void d_webkit_web_view_go_forward(IntPtr raw);

		public static d_webkit_web_view_go_forward webkit_web_view_go_forward = FuncLoader.LoadFunction<d_webkit_web_view_go_forward>(FuncLoader.GetProcAddress(GLibrary.Load(Library.Webkit), "webkit_web_view_go_forward"));

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate void d_webkit_web_view_load_string(IntPtr raw, IntPtr content, IntPtr mime_type, IntPtr encoding, IntPtr base_uri);

		public static d_webkit_web_view_load_string webkit_web_view_load_string = FuncLoader.LoadFunction<d_webkit_web_view_load_string>(FuncLoader.GetProcAddress(GLibrary.Load(Library.Webkit), "webkit_web_view_load_string"));

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate IntPtr d_webkit_web_view_get_title(IntPtr raw);

		public static d_webkit_web_view_get_title webkit_web_view_get_title = FuncLoader.LoadFunction<d_webkit_web_view_get_title>(FuncLoader.GetProcAddress(GLibrary.Load(Library.Webkit), "webkit_web_view_get_title"));

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate double d_webkit_web_view_get_progress(IntPtr raw);

		public static d_webkit_web_view_get_progress webkit_web_view_get_progress = FuncLoader.LoadFunction<d_webkit_web_view_get_progress>(FuncLoader.GetProcAddress(GLibrary.Load(Library.Webkit), "webkit_web_view_get_progress"));

	}

}