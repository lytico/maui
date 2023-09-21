using System;
using System.IO;
using System.Runtime.InteropServices;
using GLib;
using WebKit;
using UserScript = WebKit.Upstream.UserScript;

namespace GtkSharpUpstream;

internal static class UpstreamExtensions
{

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void d_webkit_user_content_manager_add_script(IntPtr raw, IntPtr script);

	internal static d_webkit_user_content_manager_add_script webkit_user_content_manager_add_script = FuncLoader.LoadFunction<d_webkit_user_content_manager_add_script>(FuncLoader.GetProcAddress(GLibrary.Load(Library.Webkit), "webkit_user_content_manager_add_script"));

	internal static void AddScript(this UserContentManager it, UserScript? script)
	{

		webkit_user_content_manager_add_script(it.Handle, script == null ? IntPtr.Zero : script.Handle);

	}

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	delegate void d_webkit_user_content_manager_remove_script(IntPtr raw, IntPtr script);

	static d_webkit_user_content_manager_remove_script webkit_user_content_manager_remove_script = FuncLoader.LoadFunction<d_webkit_user_content_manager_remove_script>(FuncLoader.GetProcAddress(GLibrary.Load(Library.Webkit), "webkit_user_content_manager_remove_script"));

	public static void RemoveScript(this UserContentManager it, UserScript? script)
	{
		webkit_user_content_manager_remove_script(it.Handle, script == null ? IntPtr.Zero : script.Handle);
	}

	// use MemoryInputStream after https://github.com/GtkSharp/GtkSharp/pull/412 is merged & published
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	delegate IntPtr d_g_memory_input_stream_new_from_data(byte[] data, uint len, IntPtr destroy);

	static d_g_memory_input_stream_new_from_data g_memory_input_stream_new_from_data = FuncLoader.LoadFunction<d_g_memory_input_stream_new_from_data>(FuncLoader.GetProcAddress(GLibrary.Load(Library.Gio), "g_memory_input_stream_new_from_data"));

	// missing in GtkSharp: found in GioSharp-api.xml but not in Code
	// class MemoryInputStream
	// https://docs.gtk.org/gio/ctor.MemoryInputStream.new_from_data.html
	internal static InputStream AsInputStream(this Stream content)
	{
		using var ms = new MemoryStream();

		content.CopyTo(ms, (int)content.Length);

		// TODO: use MemoryInputStream after https://github.com/GtkSharp/GtkSharp/pull/412 is merged & published
		var streamPtr = g_memory_input_stream_new_from_data(ms.GetBuffer(), (uint)ms.Length, IntPtr.Zero);
		var inputStream = new global::GLib.InputStream(streamPtr);

		return inputStream;

	}

	// missing in GtkSharp; also not found in *-api.xml's
	// https://docs.gtk.org/gobject/func.signal_connect_data.html
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	delegate ulong d_g_signal_connect_data(IntPtr instance, string detailed_signal, IntPtr c_handler, IntPtr data, IntPtr destroy_data, global::GLib.ConnectFlags connect_flags);

	static d_g_signal_connect_data g_signal_connect_data = FuncLoader.LoadFunction<d_g_signal_connect_data>(FuncLoader.GetProcAddress(GLibrary.Load(Library.Gio), "g_signal_connect_data"));

	public static ulong SignalConnectData<TDelegate>(this GLib.Object instance, string detailed_signal, TDelegate callBack, IntPtr data = default, IntPtr destroy_data = default, global::GLib.ConnectFlags connect_flags = 0) where TDelegate : notnull
	{
		var c_handler = Marshal.GetFunctionPointerForDelegate(callBack);

		return g_signal_connect_data(instance.Handle, detailed_signal, c_handler, data, destroy_data, connect_flags);
	}

}