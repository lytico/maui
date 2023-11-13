#nullable disable
using System;

namespace Microsoft.Maui.Controls.Handlers.Items
{

	internal class SimpleViewHolder : RecyclerView.ViewHolder
	{

		Gtk.Widget _itemView;

		public SimpleViewHolder(Gtk.Widget itemView, View rootElement) : base(itemView)
		{
			_itemView = itemView;
			View = rootElement;
		}

		public View View { get; }

		public void Recycle(ItemsView itemsView)
		{
			if (_itemView is SizedItemContentView _sizedItemContentView)
			{
				_sizedItemContentView.Recycle();
			}

			itemsView.RemoveLogicalChild(View);
		}

		public static SimpleViewHolder FromText(string text, bool fill = true)
		{
			var textView = new TextView(context) { Text = text };

			if (fill)
			{
				var layoutParams = new ViewGroup.LayoutParams(ViewGroup.LayoutParams.MatchParent,
					ViewGroup.LayoutParams.MatchParent);

				textView.LayoutParameters = layoutParams;
			}

			textView.Gravity = GravityFlags.Center;

			return new SimpleViewHolder(textView, null);
		}

		public static SimpleViewHolder FromFormsView(View formsView, Func<double> width, Func<double> height, ItemsView container)
		{
			var itemContentControl = new SizedItemContentView(width, height);

			// Make sure the Visual property is available during renderer creation
			Internals.PropertyPropagationExtensions.PropagatePropertyChanged(null, formsView, container);
			itemContentControl.RealizeContent(formsView, container);

			return new SimpleViewHolder(itemContentControl, formsView);
		}

		public static SimpleViewHolder FromFormsView(View formsView, ItemsView container)
		{
			var itemContentControl = new ItemContentView();
			itemContentControl.RealizeContent(formsView, container);

			return new SimpleViewHolder(itemContentControl, formsView);
		}

	}

}