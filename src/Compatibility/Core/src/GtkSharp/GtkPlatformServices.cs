using System;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Maui.Controls.Internals;
using Microsoft.Maui.Graphics;

namespace Microsoft.Maui.Controls.Compatibility
{

	public class GtkPlatformServices : IPlatformServices
	{

		public bool IsInvokeRequired { get; set; }

		public void BeginInvokeOnMainThread(Action action)
		{
			throw new NotImplementedException();
		}

		public Ticker CreateTicker()
		{
			throw new NotImplementedException();
		}

		public Assembly[] GetAssemblies()
		{
			throw new NotImplementedException();
		}

		public string GetHash(string input)
		{
			throw new NotImplementedException();
		}

		public string GetMD5Hash(string input)
		{
			throw new NotImplementedException();
		}

		public double GetNamedSize(NamedSize size, Type targetElementType, bool useOldSizes)
		{
			throw new NotImplementedException();
		}

		public Color GetNamedColor(string name)
		{
			throw new NotImplementedException();
		}

		public OSAppTheme RequestedTheme { get; set; }

		public async Task<Stream> GetStreamAsync(Uri uri, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		public IIsolatedStorageFile GetUserStoreForApplication()
		{
			throw new NotImplementedException();
		}

		public void OpenUriAction(Uri uri)
		{
			throw new NotImplementedException();
		}

		public void StartTimer(TimeSpan interval, Func<bool> callback)
		{
			throw new NotImplementedException();
		}

		public string RuntimePlatform { get; set; }

		public void QuitApplication()
		{
			throw new NotImplementedException();
		}

		public SizeRequest GetNativeSize(VisualElement view, double widthConstraint, double heightConstraint)
		{
			throw new NotImplementedException();
		}

	}

}