using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.Hosting;

namespace GtkTestApp
{
	public static class MauiProgram
	{
		public static void Main()
		{
			Console.WriteLine("Hello, World");

			/** var mauiApp = **/ CreateMauiApp();

			//if (mauiApp != null)
			//{
			//	// IPlatformApplication.Current = mauiApp;

			//	var rootContext = new MauiContext(mauiApp.Services);

			//	//var applicationContext = rootContext.MakeApplicationScope(this);

			//	//Services = applicationContext.Services;

			//	//Services.InvokeLifecycleEvents<WindowsLifecycle.OnLaunching>(del => del(this, args));

			//	// Application = Services.GetRequiredService<IApplication>();

			//	//this.SetApplicationHandler(Application, applicationContext);

			//	//this.CreatePlatformWindow(Application, args);

			//	//Services.InvokeLifecycleEvents<WindowsLifecycle.OnLaunched>(del => del(this, args));
			//}
		}

		public static MauiApp CreateMauiApp()
		{
			var builder = MauiApp.CreateBuilder();
			builder.UseMauiApp<GtkTestApp.App>();

			return builder.Build();
		}
	}
}
