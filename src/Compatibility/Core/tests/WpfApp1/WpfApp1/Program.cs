using Gtk;
using Microsoft.Maui.Controls.Compatibility.Platform.GTK;

namespace WpfApp1
{
	static class Program
	{
		static int count = 0;
		static Gtk.Button myButton = null!;

		/// <summary>
		///  The main entry point for the application.
		/// </summary>
		static void Main()
		{
			MauiProgram.CreateMauiApp();
			//Gtk.Application.Init();


			var app = new WpfApp1.App();

			//var app = new WpfApp1.App();
			//var window = new FormsWindow();
			//window.LoadApplication(app);
			//window.SetSizeRequest(500, 600);
			//window.SetApplicationTitle("Welcome");


			////Create the Window
			//Window myWin = new Window("My first GTK# Application! ");
			//myWin.Resize(500, 600);

			//// Create a VBox
			//VBox myContainer = new VBox();

			////Create an icon
			//Gtk.Image myIcon = new Gtk.Image("dotnet_bot_medium2.gif");

			////Add the icon to the VBox
			//myContainer.PackStart(myIcon);

			////Create a label and put some text in it.
			//Gtk.Label myLabel = new Gtk.Label();
			//myLabel.Text = "Hello, World!";
			
			////Change the label font
			//Pango.FontDescription fontdesc = new Pango.FontDescription();
			//fontdesc.Family = "Arial";
			//fontdesc.Size = (int)(28 * Pango.Scale.PangoScale);
			//myLabel.ModifyFont(fontdesc);

			////Add the label to the VBox
			//myContainer.PackStart(myLabel);

			////Create another label and put some text in it.
			//Gtk.Label myLabel2 = new Gtk.Label();
			//myLabel2.Text = "Welcome to .NET GTK#";

			////Change the label font
			//Pango.FontDescription fontdesc2 = new Pango.FontDescription();
			//fontdesc2.Family = "Arial";
			//fontdesc2.Size = (int)(16 * Pango.Scale.PangoScale);
			//myLabel2.ModifyFont(fontdesc2);

			////Add the label to the VBox
			//myContainer.PackStart(myLabel2);

			////Create the button
			//Gtk.Image clickMe = new Gtk.Image("click_me.gif");
			//myButton = new Gtk.Button();
			//myButton.Image = clickMe;
			////myButton.Clicked += MyButton_Clicked;
			//myButton.Clicked += MyButton_Clicked;

			////Create an HBox for the button
			//HBox buttonBox = new HBox();
			//VBox buttonSpacer = new VBox();
			//buttonSpacer.SetSizeRequest(190, 25);
			//buttonBox.PackStart(buttonSpacer, false, false, 3);
			//buttonBox.PackStart(myButton, false, false, 3);

			////Add the button to the VBox
			//myContainer.PackStart(buttonBox, false, false, 3);

			//VBox windowSpacer = new VBox();
			//windowSpacer.SetSizeRequest(25, 100);
			//myContainer.PackStart(windowSpacer, false, false, 3);

			////Add the VBox to the form
			//myWin.Add(myContainer);

			//Show Everything
			//myWin.ShowAll();
			//window.Show();

			Gtk.Application.Run();
		}

		private static void MyButton_Clicked(object? sender, System.EventArgs e) {
			count++;

			if (count == 1)
				myButton.Label = $"Clicked {count} time";
			else
				myButton.Label = $"Clicked {count} times";
		}
	}
}