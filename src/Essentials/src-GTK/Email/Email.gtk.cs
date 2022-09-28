using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Microsoft.Maui.ApplicationModel.Communication
{
	partial class EmailImplementation : IEmail
	{
		public bool IsComposeSupported => false;

		Task PlatformComposeAsync(EmailMessage message)
		{
			return Task.CompletedTask;
		}
	}
}
