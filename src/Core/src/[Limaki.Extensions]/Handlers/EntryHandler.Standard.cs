﻿using System;

namespace Microsoft.Maui.Handlers
{
	public partial class EntryHandler : AbstractViewHandler<IEntry, Limaki.Extensions.EntryNativeView>
	{
		protected override Limaki.Extensions.EntryNativeView CreateNativeView() => throw new NotImplementedException();

		public static void MapText(IViewHandler handler, IEntry entry) { }
		public static void MapTextColor(IViewHandler handler, IEntry entry) { }
		public static void MapIsPassword(IViewHandler handler, IEntry entry) { }
		public static void MapIsTextPredictionEnabled(IViewHandler handler, IEntry entry) { }
		public static void MapPlaceholder(IViewHandler handler, IEntry entry) { }
		public static void MapIsReadOnly(IViewHandler handler, IEntry entry) { }
		public static void MapFont(IViewHandler handler, IEntry entry) { }
	}
}