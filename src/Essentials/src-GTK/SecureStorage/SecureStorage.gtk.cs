using System;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Microsoft.Maui.Storage
{
	partial class SecureStorageImplementation : ISecureStorage
	{
		readonly CspParameters cspParams = new CspParameters();
		RSACryptoServiceProvider rsaService;

		readonly object locker = new object();

		Task<string> PlatformGetAsync(string key)
		{
			return Task.Run(() =>
			{
				try
				{
					lock (locker)
					{
						cspParams.KeyContainerName = key;
						rsaService = new RSACryptoServiceProvider(cspParams)
						{
							PersistKeyInCsp = true
						};

						return $"Key: {cspParams.KeyContainerName} - Full Key Pair";
					}
				}
				catch (Exception)
				{
					System.Diagnostics.Debug.WriteLine($"Unable to decrypt key, {key}, which is likely due to an app uninstall. Removing old key and returning null.");
					Remove(key);

					return null;
				}
			});
		}

		Task PlatformSetAsync(string key, string data)
		{
			return Task.CompletedTask;
		}

		bool PlatformRemove(string key)
		{
			return true;
		}

		void PlatformRemoveAll()
		{
		}
	}
}
