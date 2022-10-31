using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Storage;

namespace Microsoft.Maui.Authentication
{
	partial class WebAuthenticatorImplementation : IWebAuthenticator
	{
		public Task<WebAuthenticatorResult> AuthenticateAsync(WebAuthenticatorOptions webAuthenticatorOptions)
		{
			throw new TaskCanceledException();
		}
	}
}
