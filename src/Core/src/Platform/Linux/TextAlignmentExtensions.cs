using Gtk;

namespace Microsoft.Maui
{
	public static class TextAlignmentExtensions
	{
		public static Align ToNative(this TextAlignment alignment)
		{
			switch (alignment)
			{
				case TextAlignment.Start:
					return Align.Start;
				case TextAlignment.End:
					return Align.End;
				default:
					return Align.Center;
			}
		}
		
		public static Justification ToJustification(this TextAlignment alignment)
		{
			switch (alignment)
			{
				case TextAlignment.Start:
					return Justification.Left;
				case TextAlignment.End:
					return Justification.Right;
				default:
					return Justification.Center;
			}
		}
	}
}
