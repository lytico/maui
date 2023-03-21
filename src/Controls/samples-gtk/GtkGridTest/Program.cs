using Gtk;

namespace GtkGridTest
{
	class Program
	{
		private static Gtk.Entry entry1 = null!;
		private static void Main(string[] args)
		{
			Console.WriteLine("Hello, World!");

			Gtk.Application.Init();

			var window = new Gtk.Window("Gtk Grid Tester");
			window.SetSizeRequest(500, 500);

			var grid = new Gtk.Grid();
			window.Add(grid);

			var label1 = new Gtk.Label("Hello, World!");
			label1.WidthRequest = 500;
			grid.Attach(label1, 0, 0, 3, 1);
			
			var label2 = new Gtk.Label("Welcome to .NET Multi-platform App UI for GTK");
			label2.WidthRequest = 500;
			grid.Attach(label2, 0, 1, 3, 1);

			var button1 = new Gtk.Button("Click Me");
			button1.WidthRequest = 50;
			grid.Attach(button1, 1, 2, 1, 1);

			var button2 = new Gtk.Button("Click Too");
			button2.WidthRequest = 50;
			grid.Attach(button2, 0, 3, 1, 1);

			var button3 = new Gtk.Button("Click Also");
			button3.WidthRequest = 50;
			button3.Clicked += Button3_Clicked;
			grid.Attach(button3, 2, 4, 1, 1);

			entry1 = new Gtk.Entry();
			entry1.WidthRequest = 500;
			grid.Attach(entry1, 0, 5, 3, 1);

			window.Destroyed += Window_Destroyed;
			window.ShowAll();

			Gtk.Application.Run();
		}

		private static void Button3_Clicked(object? sender, EventArgs e)
		{
			if (entry1 != null!) {
				Console.WriteLine("Here is the contents of the entry: " + entry1.Text);
			}
		}

		private static void Window_Destroyed(object? sender, EventArgs e) {
			Gtk.Main.Quit();
		}
	}
}