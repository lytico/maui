using System.Diagnostics;
using Gtk;

namespace GTKnonMAUI
{
	static class Program
	{
		static int count = 0;
		static Gtk.Button myButton = null!;

		/// <summary>
		///  The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			Gtk.Application.Init();

			//Create the Window
			Window myWin = new Window("My first GTK# Application! ");
			myWin.Resize(500, 600);
			myWin.Destroyed += MyWin_Destroyed;

			// Create a VBox
			VBox myContainer = new VBox();

			//Create an icon
			Gtk.Image myIcon = new Gtk.Image("dotnet_bot_medium2.gif");

			//Add the icon to the VBox
			myContainer.PackStart(myIcon);

			//Create a label and put some text in it.
			Gtk.Label myLabel = new Gtk.Label();
			myLabel.Text = "Hello, World!";
			
			//Change the label font
			Pango.FontDescription fontdesc = new Pango.FontDescription();
			fontdesc.Family = "Arial";
			fontdesc.Size = (int)(28 * Pango.Scale.PangoScale);
			myLabel.ModifyFont(fontdesc);

			//Add the label to the VBox
			myContainer.PackStart(myLabel);

			//Create another label and put some text in it.
			Gtk.Label myLabel2 = new Gtk.Label();
			myLabel2.Text = "Welcome to .NET GTK#";

			//Change the label font
			Pango.FontDescription fontdesc2 = new Pango.FontDescription();
			fontdesc2.Family = "Arial";
			fontdesc2.Size = (int)(16 * Pango.Scale.PangoScale);
			myLabel2.ModifyFont(fontdesc2);

			//Add the label to the VBox
			myContainer.PackStart(myLabel2);

			//Create the button
			Gtk.Image clickMe = new Gtk.Image("click_me.gif");
			myButton = new Gtk.Button();
			myButton.Image = clickMe;
			myButton.Clicked += MyButton_Clicked;

			//Create an HBox for the button
			HBox buttonBox = new HBox();
			VBox buttonSpacer = new VBox();
			buttonSpacer.SetSizeRequest(190, 25);
			buttonBox.PackStart(buttonSpacer, false, false, 3);
			buttonBox.PackStart(myButton, false, false, 3);

			//Add the button to the VBox
			myContainer.PackStart(buttonBox, false, false, 3);

			VBox windowSpacer = new VBox();
			windowSpacer.SetSizeRequest(25, 100);
			myContainer.PackStart(windowSpacer, false, false, 3);

			//Add the VBox to the form
			myWin.Add(myContainer);

			//Show Everything
			myWin.ShowAll();

			Gtk.Application.Run();

			Debug.WriteLine("After Run");
		}

		private static void MyWin_Destroyed(object? sender, EventArgs e)
		{
			Debug.WriteLine("Destroyed");
			Gtk.Application.Quit();
		}

		private static void MyButton_Clicked(object? sender, EventArgs e)
		{
			count++;

			if (count == 1)
				myButton.Label = $"Clicked {count} time";
			else
				myButton.Label = $"Clicked {count} times";
		}
	}
}