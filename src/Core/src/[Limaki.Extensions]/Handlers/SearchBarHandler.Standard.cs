using System;

namespace Microsoft.Maui.Handlers
{
	public partial class SearchBarHandler : AbstractViewHandler<ISearchBar, Limaki.Extensions.ISearchBarNativeView>
	{
		protected override Limaki.Extensions.ISearchBarNativeView CreateNativeView() => throw new NotImplementedException();

		public static void MapText(IViewHandler handler, ISearchBar searchBar) { }
		public static void MapPlaceholder(IViewHandler handler, ISearchBar searchBar) { }
		public static void MapHorizontalTextAlignment(IViewHandler handler, ISearchBar searchBar) { }
	}
}