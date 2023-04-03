using System.Threading.Tasks;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Devices.Sensors;

namespace Microsoft.Maui.Devices.Sensors
{
	/// <summary>
	/// This class contains static extension methods for use with <see cref="Location"/> objects.
	/// </summary>
	public static partial class LocationExtensions
	{
		/// <inheritdoc cref="Location.CalculateDistance(double, double, double, double, DistanceUnits)"/>
		public static double CalculateDistance(this Location locationStart, double latitudeEnd, double longitudeEnd, DistanceUnits units) =>
			Location.CalculateDistance(locationStart, latitudeEnd, longitudeEnd, units);

		/// <inheritdoc cref="Location.CalculateDistance(Location, Location, DistanceUnits)"/>
		public static double CalculateDistance(this Location locationStart, Location locationEnd, DistanceUnits units) =>
			Location.CalculateDistance(locationStart, locationEnd, units);

#if !__GTK__
		/// <include file="../../docs/Microsoft.Maui.Essentials/LocationExtensions.xml" path="//Member[@MemberName='OpenMapsAsync'][2]/Docs/*" />
		public static Task OpenMapsAsync(this Location location, MapLaunchOptions options) =>
			Map.OpenAsync(location, options);

		/// <inheritdoc cref="Map.OpenAsync(Location)"/>
		public static Task OpenMapsAsync(this Location location) =>
			Map.OpenAsync(location);
#endif
	}
}
