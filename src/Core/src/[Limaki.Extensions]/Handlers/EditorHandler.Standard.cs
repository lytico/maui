using System;

namespace Microsoft.Maui.Handlers
{
	public partial class EditorHandler : AbstractViewHandler<IEditor, Limaki.Extensions.EditorNativeView>
	{
		protected override Limaki.Extensions.EditorNativeView CreateNativeView() => throw new NotImplementedException();

		public static void MapText(IViewHandler handler, IEditor editor) { }
	}
}