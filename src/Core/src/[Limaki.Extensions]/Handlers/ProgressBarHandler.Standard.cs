using System;

namespace Microsoft.Maui.Handlers
{
	public partial class ProgressBarHandler : AbstractViewHandler<IProgress, Microsoft.Maui.Limaki.Extensions.IProgressBarNativeView>
	{
		protected override Limaki.Extensions.IProgressBarNativeView CreateNativeView() => throw new NotImplementedException();
	}
}
