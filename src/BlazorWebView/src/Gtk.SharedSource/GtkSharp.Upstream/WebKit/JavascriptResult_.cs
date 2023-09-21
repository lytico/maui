using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
#pragma warning disable CS1591 

namespace WebKit.Upstream
{

	[SuppressMessage("ApiDesign", "RS0016:Öffentliche Typen und Member der deklarierten API hinzufügen")]
	public partial class JavascriptResult
	{

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		delegate IntPtr d_webkit_javascript_result_get_js_value(IntPtr raw);

		static d_webkit_javascript_result_get_js_value webkit_javascript_result_get_js_value = FuncLoader.LoadFunction<d_webkit_javascript_result_get_js_value>(FuncLoader.GetProcAddress(GLibrary.Load(Library.Webkit), "webkit_javascript_result_get_js_value"));

		public JavaScriptValue JsValue
		{
			get
			{
				IntPtr raw_ret = webkit_javascript_result_get_js_value(Handle);
				JavaScriptValue ret = new JavaScriptValue(raw_ret);

				return ret;
			}
		}

		static JavascriptResult()
		{
			GtkSharp.WebkitGtkSharp.ObjectManager.Initialize();
			ObjectManager.Initialize();

		}

	}

}