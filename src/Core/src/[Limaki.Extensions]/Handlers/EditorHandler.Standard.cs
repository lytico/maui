using System;

namespace Microsoft.Maui.Handlers
{
	public partial class EditorHandler : AbstractViewHandler<IEditor, Limaki.Extensions.IEditorNativeView>
	{
		protected override Limaki.Extensions.IEditorNativeView CreateNativeView() => throw new NotImplementedException();

		public static void MapText(IViewHandler handler, IEditor editor) { }
	}
}