// This file was generated by the Gtk# code generator.
// Any changes made will be lost if regenerated.

namespace GLib.Upstream {

	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Runtime.InteropServices;
	using static GLib.AbiStructExtension;

#region Autogenerated code
	public partial class MemoryInputStream : GLib.InputStream, GLib.ISeekable {

		public MemoryInputStream (IntPtr raw) : base(raw) {}

		[UnmanagedFunctionPointer (CallingConvention.Cdecl)]
		delegate IntPtr d_g_memory_input_stream_new();
		static d_g_memory_input_stream_new g_memory_input_stream_new = FuncLoader.LoadFunction<d_g_memory_input_stream_new>(FuncLoader.GetProcAddress(GLibrary.Load(Library.Gio), "g_memory_input_stream_new"));

		public MemoryInputStream () : base (IntPtr.Zero)
		{
			if (GetType () != typeof (MemoryInputStream)) {
				CreateNativeObject (Array.Empty<string> (), Array.Empty<GLib.Value> ());
				return;
			}
			Raw = g_memory_input_stream_new();
		}

		[UnmanagedFunctionPointer (CallingConvention.Cdecl)]
		delegate IntPtr d_g_memory_input_stream_new_from_bytes(IntPtr bytes);
		static d_g_memory_input_stream_new_from_bytes g_memory_input_stream_new_from_bytes = FuncLoader.LoadFunction<d_g_memory_input_stream_new_from_bytes>(FuncLoader.GetProcAddress(GLibrary.Load(Library.Gio), "g_memory_input_stream_new_from_bytes"));

		public MemoryInputStream (GLib.Bytes bytes) : base (IntPtr.Zero)
		{
			if (GetType () != typeof (MemoryInputStream)) {
				var vals = new List<GLib.Value> ();
				var names = new List<string> ();
				CreateNativeObject (names.ToArray (), vals.ToArray ());
				return;
			}
			Raw = g_memory_input_stream_new_from_bytes(bytes == null ? IntPtr.Zero : bytes.Handle);
		}

		[UnmanagedFunctionPointer (CallingConvention.Cdecl)]
		delegate IntPtr d_g_memory_input_stream_new_from_data(IntPtr data, IntPtr len, GLib.DestroyNotify destroy);
		static d_g_memory_input_stream_new_from_data g_memory_input_stream_new_from_data = FuncLoader.LoadFunction<d_g_memory_input_stream_new_from_data>(FuncLoader.GetProcAddress(GLibrary.Load(Library.Gio), "g_memory_input_stream_new_from_data"));

		public MemoryInputStream (IntPtr data, long len, GLib.DestroyNotify destroy) : base (IntPtr.Zero)
		{
			if (GetType () != typeof (MemoryInputStream)) {
				var vals = new List<GLib.Value> ();
				var names = new List<string> ();
				CreateNativeObject (names.ToArray (), vals.ToArray ());
				return;
			}
			Raw = g_memory_input_stream_new_from_data(data, new IntPtr (len), destroy);
		}


		// Internal representation of the wrapped structure ABI.
		static GLib.AbiStruct _class_abi = null;
		static public unsafe new GLib.AbiStruct class_abi {
			get {
				if (_class_abi == null)
					_class_abi = new GLib.AbiStruct (new List<GLib.AbiField>{ 
						new GLib.AbiField("_g_reserved1"
							, GLib.InputStream.class_abi.Fields
							, (uint) sizeof( IntPtr ) // _g_reserved1
							, null
							, "_g_reserved2"
							, (uint) sizeof(IntPtr)
							, 0
							),
						new GLib.AbiField("_g_reserved2"
							, -1
							, (uint) sizeof( IntPtr ) // _g_reserved2
							, "_g_reserved1"
							, "_g_reserved3"
							, (uint) sizeof(IntPtr)
							, 0
							),
						new GLib.AbiField("_g_reserved3"
							, -1
							, (uint) sizeof( IntPtr ) // _g_reserved3
							, "_g_reserved2"
							, "_g_reserved4"
							, (uint) sizeof(IntPtr)
							, 0
							),
						new GLib.AbiField("_g_reserved4"
							, -1
							, (uint) sizeof( IntPtr ) // _g_reserved4
							, "_g_reserved3"
							, "_g_reserved5"
							, (uint) sizeof(IntPtr)
							, 0
							),
						new GLib.AbiField("_g_reserved5"
							, -1
							, (uint) sizeof( IntPtr ) // _g_reserved5
							, "_g_reserved4"
							, null
							, (uint) sizeof(IntPtr)
							, 0
							),
					});

				return _class_abi;
			}
		}


		// End of the ABI representation.

		[UnmanagedFunctionPointer (CallingConvention.Cdecl)]
		delegate void d_g_memory_input_stream_add_bytes(IntPtr raw, IntPtr bytes);
		static d_g_memory_input_stream_add_bytes g_memory_input_stream_add_bytes = FuncLoader.LoadFunction<d_g_memory_input_stream_add_bytes>(FuncLoader.GetProcAddress(GLibrary.Load(Library.Gio), "g_memory_input_stream_add_bytes"));

		public void AddBytes(GLib.Bytes bytes) {
			g_memory_input_stream_add_bytes(Handle, bytes == null ? IntPtr.Zero : bytes.Handle);
		}

		[UnmanagedFunctionPointer (CallingConvention.Cdecl)]
		delegate void d_g_memory_input_stream_add_data(IntPtr raw, IntPtr data, IntPtr len, GLib.DestroyNotify destroy);
		static d_g_memory_input_stream_add_data g_memory_input_stream_add_data = FuncLoader.LoadFunction<d_g_memory_input_stream_add_data>(FuncLoader.GetProcAddress(GLibrary.Load(Library.Gio), "g_memory_input_stream_add_data"));

		public void AddData(IntPtr data, long len, GLib.DestroyNotify destroy) {
			g_memory_input_stream_add_data(Handle, data, new IntPtr (len), destroy);
		}

		[UnmanagedFunctionPointer (CallingConvention.Cdecl)]
		delegate IntPtr d_g_memory_input_stream_get_type();
		static d_g_memory_input_stream_get_type g_memory_input_stream_get_type = FuncLoader.LoadFunction<d_g_memory_input_stream_get_type>(FuncLoader.GetProcAddress(GLibrary.Load(Library.Gio), "g_memory_input_stream_get_type"));

		public static new GLib.GType GType { 
			get {
				IntPtr raw_ret = g_memory_input_stream_get_type();
				GLib.GType ret = new GLib.GType(raw_ret);
				return ret;
			}
		}

		[UnmanagedFunctionPointer (CallingConvention.Cdecl)]
		delegate bool d_g_seekable_can_seek(IntPtr raw);
		static d_g_seekable_can_seek g_seekable_can_seek = FuncLoader.LoadFunction<d_g_seekable_can_seek>(FuncLoader.GetProcAddress(GLibrary.Load(Library.Gio), "g_seekable_can_seek"));

		public bool CanSeek { 
			get {
				bool raw_ret = g_seekable_can_seek(Handle);
				bool ret = raw_ret;
				return ret;
			}
		}

		[UnmanagedFunctionPointer (CallingConvention.Cdecl)]
		delegate bool d_g_seekable_can_truncate(IntPtr raw);
		static d_g_seekable_can_truncate g_seekable_can_truncate = FuncLoader.LoadFunction<d_g_seekable_can_truncate>(FuncLoader.GetProcAddress(GLibrary.Load(Library.Gio), "g_seekable_can_truncate"));

		public bool CanTruncate() {
			bool raw_ret = g_seekable_can_truncate(Handle);
			bool ret = raw_ret;
			return ret;
		}

		[UnmanagedFunctionPointer (CallingConvention.Cdecl)]
		delegate bool d_g_seekable_seek(IntPtr raw, long offset, int type, IntPtr cancellable, out IntPtr error);
		static d_g_seekable_seek g_seekable_seek = FuncLoader.LoadFunction<d_g_seekable_seek>(FuncLoader.GetProcAddress(GLibrary.Load(Library.Gio), "g_seekable_seek"));

		public bool Seek(long offset, GLib.SeekType type, GLib.Cancellable cancellable) {
			IntPtr error = IntPtr.Zero;
			bool raw_ret = g_seekable_seek(Handle, offset, (int) type, cancellable == null ? IntPtr.Zero : cancellable.Handle, out error);
			bool ret = raw_ret;
			if (error != IntPtr.Zero) throw new GLib.GException (error);
			return ret;
		}

		[UnmanagedFunctionPointer (CallingConvention.Cdecl)]
		delegate long d_g_seekable_tell(IntPtr raw);
		static d_g_seekable_tell g_seekable_tell = FuncLoader.LoadFunction<d_g_seekable_tell>(FuncLoader.GetProcAddress(GLibrary.Load(Library.Gio), "g_seekable_tell"));

		public long Position { 
			get {
				long raw_ret = g_seekable_tell(Handle);
				long ret = raw_ret;
				return ret;
			}
		}

		[UnmanagedFunctionPointer (CallingConvention.Cdecl)]
		delegate bool d_g_seekable_truncate(IntPtr raw, long offset, IntPtr cancellable, out IntPtr error);
		static d_g_seekable_truncate g_seekable_truncate = FuncLoader.LoadFunction<d_g_seekable_truncate>(FuncLoader.GetProcAddress(GLibrary.Load(Library.Gio), "g_seekable_truncate"));

		public bool Truncate(long offset, GLib.Cancellable cancellable) {
			IntPtr error = IntPtr.Zero;
			bool raw_ret = g_seekable_truncate(Handle, offset, cancellable == null ? IntPtr.Zero : cancellable.Handle, out error);
			bool ret = raw_ret;
			if (error != IntPtr.Zero) throw new GLib.GException (error);
			return ret;
		}


		// Internal representation of the wrapped structure ABI.
		static GLib.AbiStruct _abi_info = null;
		static public unsafe new GLib.AbiStruct abi_info {
			get {
				if (_abi_info == null)
					_abi_info = new GLib.AbiStruct (new List<GLib.AbiField>{ 
						new GLib.AbiField("priv"
							, GLib.InputStream.abi_info.Fields
							, (uint) sizeof( IntPtr ) // priv
							, null
							, null
							, (uint) sizeof(IntPtr)
							, 0
							),
					});

				return _abi_info;
			}
		}


		// End of the ABI representation.

#endregion
	}
}
