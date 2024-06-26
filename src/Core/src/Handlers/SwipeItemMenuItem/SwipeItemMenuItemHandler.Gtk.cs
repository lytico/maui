﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Maui.Handlers
{
	public partial class SwipeItemMenuItemHandler : ElementHandler<ISwipeItemMenuItem, NotImplementedView>
	{
		protected override NotImplementedView CreatePlatformElement() => new(nameof(ISwipeItemMenuItem));

		public static void MapTextColor(ISwipeItemMenuItemHandler handler, ITextStyle view) { }

		public static void MapCharacterSpacing(ISwipeItemMenuItemHandler handler, ITextStyle view) { }

		public static void MapFont(ISwipeItemMenuItemHandler handler, ITextStyle view) { }

		public static void MapText(ISwipeItemMenuItemHandler handler, ISwipeItemMenuItem view) { }

		public static void MapBackground(ISwipeItemMenuItemHandler handler, ISwipeItemMenuItem view) { }

		public static void MapVisibility(ISwipeItemMenuItemHandler handler, ISwipeItemMenuItem view) { }

		partial class SwipeItemMenuItemImageSourcePartSetter
		{
			public override void SetImageSource(Gdk.Pixbuf? platformImage) { }
		}
	}
}
