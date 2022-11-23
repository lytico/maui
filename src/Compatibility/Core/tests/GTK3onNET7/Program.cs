using System.Diagnostics;
using Gtk;
using GTK3onNET7;

Gtk.Application.Init();

//static int ButtonCounter { get; set; } = 0;
//Gtk.Button myButton = null!;

//Create the Window
Window myWin = new Window("My first .Net6.0 GTK# Application! ");
myWin.Resize(500, 600);
myWin.Destroyed += MyWin_Destroyed;

// Create a VBox
Box myContainer = new Box(Gtk.Orientation.Vertical, 0);

//Create an icon
Gtk.Image myIcon = new Gtk.Image("dotnet_bot_medium2.gif");

//Add the icon to the VBox
myContainer.PackStart(myIcon, false, false, 0);

//Create a label and put some text in it.
Gtk.Label myLabel = new Gtk.Label();
myLabel.Text = "Hello, World!";

//Change the label font
//Pango.FontDescription fontdesc = new Pango.FontDescription();
//fontdesc.Family = "Arial";
//fontdesc.Size = (int)(28 * Pango.Scale.PangoScale);
//myLabel.ModifyFont(fontdesc);

//Add the label to the VBox
myContainer.PackStart(myLabel, false, false, 0);

//Create another label and put some text in it.
Gtk.Label myLabel2 = new Gtk.Label();
myLabel2.Text = "Welcome to .NET 6.0 GTK#";

////Change the label font
//Pango.FontDescription fontdesc2 = new Pango.FontDescription();
//fontdesc2.Family = "Arial";
//fontdesc2.Size = (int)(16 * Pango.Scale.PangoScale);
//myLabel2.ModifyFont(fontdesc2);

//Add the label to the VBox
myContainer.PackStart(myLabel2, false, false, 0);

//Create the button
Gtk.Image clickMe = new Gtk.Image("click_me.gif");
var myButton = new MyButton();
myButton.Image = clickMe;
myButton.Clicked += MyButton_Clicked;

//Create an HBox for the button
Box buttonBox = new Box(Gtk.Orientation.Horizontal, 0);
Box buttonSpacer = new Box(Gtk.Orientation.Vertical, 0);
buttonSpacer.SetSizeRequest(190, 25);
buttonBox.PackStart(buttonSpacer, false, false, 3);
buttonBox.PackStart(myButton, false, false, 3);

//Add the button to the VBox
myContainer.PackStart(buttonBox, false, false, 3);

Box windowSpacer = new Box(Gtk.Orientation.Vertical, 0);
windowSpacer.SetSizeRequest(25, 100);
myContainer.PackStart(windowSpacer, false, false, 3);

//Add the VBox to the form
myWin.Add(myContainer);

//Show Everything
myWin.ShowAll();

Gtk.Application.Run();

Debug.WriteLine("After Run");

static void MyWin_Destroyed(object? sender, EventArgs e)
{
	Debug.WriteLine("Destroyed");
	Gtk.Application.Quit();
}

static void MyButton_Clicked(object? sender, EventArgs e)
{
	if (sender is MyButton myButton)
	{
		myButton.ButtonCounter++;

		if (myButton.ButtonCounter == 1)
			myButton.Label = $"Clicked {myButton.ButtonCounter} time";
		else
			myButton.Label = $"Clicked {myButton.ButtonCounter} times";
	}
}
