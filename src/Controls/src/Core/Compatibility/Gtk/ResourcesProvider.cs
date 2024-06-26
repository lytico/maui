using Microsoft.Maui.Controls.Internals;
using Microsoft.Maui.Graphics;

namespace Microsoft.Maui.Controls.Compatibility.Platform.Gtk
{

#pragma warning disable CS0612
	internal class ResourcesProvider : ISystemResourcesProvider
#pragma warning restore CS0612
	{

		private const string TitleStyleKey = "HeaderLabelStyle";
		private const string SubtitleStyleKey = "SubheaderLabelStyle";
		private const string BodyStyleKey = "BodyLabelStyle";
		private const string CaptionStyleKey = "CaptionLabelStyle";
		private const string ListItemDetailTextStyleKey = "BodyLabelStyle";
		private const string ListItemTextStyleKey = "BaseLabelStyle";

		public IResourceDictionary GetSystemResources()
		{
			return new ResourceDictionary
			{
#pragma warning disable CS0612
				[Device.Styles.TitleStyleKey] = GetStyle(TitleStyleKey),
				[Device.Styles.SubtitleStyleKey] = GetStyle(SubtitleStyleKey),
				[Device.Styles.BodyStyleKey] = GetStyle(BodyStyleKey),
				[Device.Styles.CaptionStyleKey] = GetStyle(CaptionStyleKey),
				[Device.Styles.ListItemDetailTextStyleKey] = GetStyle(ListItemDetailTextStyleKey),
				[Device.Styles.ListItemTextStyleKey] = GetStyle(ListItemTextStyleKey)
#pragma warning restore CS0612

			};
		}

		private Style GetStyle(string nativeKey)
		{
			var result = new Style(typeof(Label));

			switch (nativeKey)
			{
				case TitleStyleKey:
					result.Setters.Add(new Setter
					{
						Property = Label.FontSizeProperty,
						Value = 24
					});

					break;
				case SubtitleStyleKey:
					result.Setters.Add(new Setter
					{
						Property = Label.FontSizeProperty,
						Value = 20
					});

					break;
				case BodyStyleKey:
					result.Setters.Add(new Setter
					{
						Property = Label.TextColorProperty,
						Value = Colors.Blue
					});

					break;
				case CaptionStyleKey:
					break;
				case ListItemTextStyleKey:
					break;
			}

			return result;
		}

	}

}