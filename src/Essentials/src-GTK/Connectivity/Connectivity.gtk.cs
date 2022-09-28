using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Microsoft.Maui.Networking
{
	partial class ConnectivityImplementation : IConnectivity
	{
		void StartListeners()
		{

		}

		void StopListeners()
		{

		}

		public NetworkAccess NetworkAccess
		{
			get
			{
				return NetworkAccess.Unknown;
			}
		}

		public IEnumerable<ConnectionProfile> ConnectionProfiles
		{
			get
			{
				yield return ConnectionProfile.Ethernet;
			}
		}
	}
}
