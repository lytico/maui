using Microsoft.Maui.Controls.SourceGen;

// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");
Generate1File();
GenerateAnotherFile();

void Generate1File()
{
	var fileNameRoot = "MainPage.xaml";
	var fileNameSourcePath = "D:\\Source\\maui\\src\\Controls\\samples-gtk\\Controls.Sample.OnePage.GTK";
	var fullFileNamePath = Path.Combine(fileNameSourcePath, fileNameRoot);
	var fileSource = File.ReadAllText(fullFileNamePath);
	var projectOutputPath = "D:\\Source\\maui\\src\\Controls\\samples-gtk\\Controls.Sample.OnePage.GTK\\obj\\Debug\\net6.0-windows\\prebuilt";
	Directory.CreateDirectory(projectOutputPath);
	GenerateXaml.GenerateXamlCodeBehind(fileSource, fileNameRoot, fileNameSourcePath, fileNameRoot, projectOutputPath);
}

void GenerateAnotherFile()
{
	var fileNameRoot = "SandboxShell.xaml";
	var fileNameSourcePath = "D:\\Source\\maui\\src\\Controls\\samples-gtk\\Controls.Sample.OnePage.GTK";
	var fullFileNamePath = Path.Combine(fileNameSourcePath, fileNameRoot);
	var fileSource = File.ReadAllText(fullFileNamePath);
	var projectOutputPath = "D:\\Source\\maui\\src\\Controls\\samples-gtk\\Controls.Sample.OnePage.GTK\\obj\\Debug\\net6.0-windows\\prebuilt";
	Directory.CreateDirectory(projectOutputPath);
	GenerateXaml.GenerateXamlCodeBehind(fileSource, fileNameRoot, fileNameSourcePath, fileNameRoot, projectOutputPath);
}
