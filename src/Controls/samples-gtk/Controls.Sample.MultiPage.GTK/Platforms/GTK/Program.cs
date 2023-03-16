using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui;
using Microsoft.Maui.Hosting;

namespace Maui.Controls.Sample.MultiPage.GTK.Platforms.GTK
{
	public class Program
	{
		// This is the main entry point of the application.
		static void Main(string[] args)
		{
			//// if you want to use a different Application Delegate class from "AppDelegate"
			//// you can specify it here.
			//UIApplication.Main(args, null, typeof(AppDelegate));

			var mauiGTKApp = new GTKApp();

			System.Diagnostics.Debug.WriteLine("Inside Platforms.GTK.Program.Main.");

			mauiGTKApp.Main(args);
			//Gtk.Application.Init();
			//MauiProgram.CreateMauiApp();

			//_ =  new App();


			// Gtk.Application.Run();
		}
	}

	class GTKApp : MauiGTKApplication
	{
		protected override MauiApp CreateMauiApp()
		{
			WindowCssFileName = "Resources/Styles/GeorgeGtk.css";
			return MauiProgram.CreateMauiApp();
		}
	}
}
