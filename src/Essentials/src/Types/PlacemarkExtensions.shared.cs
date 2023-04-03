using System;
using System.Threading.Tasks;
using Microsoft.Maui.ApplicationModel;

namespace Microsoft.Maui.Devices.Sensors
{
	/// <summary>
	/// This class contains static extension methods for use with <see cref="Placemark"/> objects.
	/// </summary>
	public static partial class PlacemarkExtensions
	{
#if !__GTK__
		/// <include file="../../docs/Microsoft.Maui.Essentials/PlacemarkExtensions.xml" path="//Member[@MemberName='OpenMapsAsync'][2]/Docs/*" />
		public static Task OpenMapsAsync(this Placemark placemark, MapLaunchOptions options) =>
			Map.OpenAsync(placemark, options);

		/// <inheritdoc cref="Map.OpenAsync(Placemark)"/>
		public static Task OpenMapsAsync(this Placemark placemark) =>
			Map.OpenAsync(placemark);
#endif

		internal static string GetEscapedAddress(this Placemark placemark)
		{
			if (placemark == null)
				throw new ArgumentNullException(nameof(placemark));

			var address = $"{placemark.Thoroughfare} {placemark.Locality} {placemark.AdminArea} {placemark.PostalCode} {placemark.CountryName}";

			return Uri.EscapeDataString(address);
		}
	}
}
