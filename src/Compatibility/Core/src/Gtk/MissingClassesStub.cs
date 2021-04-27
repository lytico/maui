using System;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Gtk;
using Microsoft.Maui.Controls.Internals;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Hosting;
using Action = System.Action;

namespace Microsoft.Maui.Controls.Compatibility
{

	public static class AppHostBuilderExtensions
	{

		public static IAppHostBuilder UseCompatibilityRenderers(this IAppHostBuilder builder)
		{

			return builder;
		}

	}

}

namespace Microsoft.Maui.Controls.Compatibility
{

	public static class Forms
	{

		public static void Init(IActivationState state)
		{
			var gtkServices = new GtkPlatformServices();
			Device.PlatformServices = gtkServices;

		}

	}

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

#pragma warning disable 1998
		public async Task<Stream> GetStreamAsync(Uri uri, CancellationToken cancellationToken)
#pragma warning restore 1998
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

		public string RuntimePlatform => Device.GTK;

		public void QuitApplication()
		{ }

		public SizeRequest GetNativeSize(VisualElement view, double widthConstraint, double heightConstraint)
		{
			if (view.Handler.NativeView is Widget w)
			{
				return new SizeRequest(new Size(w.Allocation.Width, w.Allocation.Height));
			}

			return new SizeRequest(new Size(widthConstraint, heightConstraint));
		}

	}

}