using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Maui.Handlers
{
	public partial class LayoutHandler : AbstractViewHandler<ILayout, Limaki.Extensions.LayoutNativeView>
	{
		public void Add(IView view) => throw new NotImplementedException();
		public void Remove(IView view) => throw new NotImplementedException();

		protected override Limaki.Extensions.LayoutNativeView CreateNativeView() => throw new NotImplementedException();
	}
}
