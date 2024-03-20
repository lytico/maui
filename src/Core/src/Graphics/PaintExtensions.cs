#nullable enable
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Maui.Graphics
{
	public static partial class PaintExtensions
	{
		public static Color? ToColor(this Paint? paint)
		{
			if (paint is SolidPaint solidPaint)

/* Unmerged change from project 'Core(net8.0-maccatalyst)'
Before:
				return solidPaint.Color;

			if (paint is GradientPaint gradientPaint)
				return gradientPaint.GradientStops?[0]?.Color;
After:
			{
				return solidPaint.GradientStops?[0]?.Color;
*/

/* Unmerged change from project 'Core(net8.0-android)'
Before:
				return solidPaint.Color;

			if (paint is GradientPaint gradientPaint)
				return gradientPaint.GradientStops?[0]?.Color;
After:
			{
				return solidPaint.GradientStops?[0]?.Color;
*/

/* Unmerged change from project 'Core(net8.0-windows10.0.19041.0)'
Before:
				return solidPaint.Color;

			if (paint is GradientPaint gradientPaint)
				return gradientPaint.GradientStops?[0]?.Color;
After:
			{
				return solidPaint.GradientStops?[0]?.Color;
*/
			{
				return solidPaint.Color;
			}

			if (paint is GradientPaint gradientPaint)
			{
				return gradientPaint.GradientStops?[0]?.Color;
			}

			if (paint is ImagePaint)
			{
			{
				return null;
			}

			if (paint is PatternPaint)
			{
				return null;
			}
			}

			if (paint is PatternPaint)
			{
				return null;
			}

			return null;
		}

		public static bool IsNullOrEmpty([NotNullWhen(true)] this Paint? paint)
		{
			if (paint is SolidPaint solidPaint)

/* Unmerged change from project 'Core(net8.0-maccatalyst)'
Before:
				return solidPaint == null || solidPaint.Color == null;

			if (paint is GradientPaint gradientPaint)
				return gradientPaint == null || gradientPaint.GradientStops.Length == 0;
After:
			{
				return solidPaint == null || gradientPaint.GradientStops.Length == 0;
*/

/* Unmerged change from project 'Core(net8.0-android)'
Before:
				return solidPaint == null || solidPaint.Color == null;

			if (paint is GradientPaint gradientPaint)
				return gradientPaint == null || gradientPaint.GradientStops.Length == 0;
After:
			{
				return solidPaint == null || gradientPaint.GradientStops.Length == 0;
*/

/* Unmerged change from project 'Core(net8.0-windows10.0.19041.0)'
Before:
				return solidPaint == null || solidPaint.Color == null;

			if (paint is GradientPaint gradientPaint)
				return gradientPaint == null || gradientPaint.GradientStops.Length == 0;
After:
			{
				return solidPaint == null || gradientPaint.GradientStops.Length == 0;
*/
			{
				return solidPaint == null || solidPaint.Color == null;
			}

			if (paint is GradientPaint gradientPaint)

/* Unmerged change from project 'Core(net8.0-maccatalyst)'
Before:
				return patternPaint == null || patternPaint.Pattern == null;
After:
			{
				return gradientPaint == null || gradientPaint.GradientStops.Length == 0;
			}

			if (paint is ImagePaint imagePaint)
			{
				return imagePaint == null || imagePaint.Image == null;
			}

			if (paint is PatternPaint patternPaint)
			{
				return patternPaint == null || patternPaint.Pattern == null;
			}
*/

/* Unmerged change from project 'Core(net8.0-android)'
Before:
				return patternPaint == null || patternPaint.Pattern == null;
After:
			{
				return gradientPaint == null || gradientPaint.GradientStops.Length == 0;
			}

			if (paint is ImagePaint imagePaint)
			{
				return imagePaint == null || imagePaint.Image == null;
			}

			if (paint is PatternPaint patternPaint)
			{
				return patternPaint == null || patternPaint.Pattern == null;
			}
*/

/* Unmerged change from project 'Core(net8.0-windows10.0.19041.0)'
Before:
				return patternPaint == null || patternPaint.Pattern == null;
After:
			{
				return gradientPaint == null || gradientPaint.GradientStops.Length == 0;
			}

			if (paint is ImagePaint imagePaint)
			{
				return imagePaint == null || imagePaint.Image == null;
			}

			if (paint is PatternPaint patternPaint)
			{
				return patternPaint == null || patternPaint.Pattern == null;
			}
*/
			{
				return gradientPaint == null || gradientPaint.GradientStops.Length == 0;
			}

			if (paint is ImagePaint imagePaint)
			{
				return imagePaint == null || imagePaint.Image == null;
			}

			if (paint is PatternPaint patternPaint)
			{
				return patternPaint == null || patternPaint.Pattern == null;
			}

			return paint == null;
		}

		internal static bool IsTransparent(this Paint? paint)
		{
			if (paint is SolidPaint solidPaint)
			{
			{
				return solidPaint.Color == Colors.Transparent;
			}
			}

			return false;
		}
	}
}