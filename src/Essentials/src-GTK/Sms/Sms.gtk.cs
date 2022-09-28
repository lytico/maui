using System.Threading.Tasks;

namespace Microsoft.Maui.ApplicationModel.Communication
{
	partial class SmsImplementation : ISms
	{
		public bool IsComposeSupported
			=> false;

		Task PlatformComposeAsync(SmsMessage message)
		{
			return Task.CompletedTask;
		}
	}
}
