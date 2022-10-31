namespace Microsoft.Maui
{
	public partial interface IFontManager
	{
		// FontFamily DefaultFontFamily { get; }

		string GetFontFamily(Font font);

		double GetFontSize(Font font, double defaultFontSize = 0);
	}
}