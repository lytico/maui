using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Versioning;
using System.Threading.Tasks;

namespace Microsoft.Maui.ApplicationModel
{
	class AppActionsImplementation : IAppActions, IPlatformAppActions
	{
		public bool IsSupported => false;

		public Task<IEnumerable<AppAction>> GetAsync()
		{
			if (!IsSupported)
				throw new FeatureNotSupportedException();

			return Task.FromResult<IEnumerable<AppAction>>(null);
		}

		public Task SetAsync(IEnumerable<AppAction> actions)
		{
			if (!IsSupported)
				throw new FeatureNotSupportedException();

			return Task.CompletedTask;
		}
	}
}
