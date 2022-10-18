using Microsoft.Maui.Controls.SourceGen;

if (args == null || args.Length < 2)
{
	if (args?.Length > 0)
		Console.WriteLine("GenerateXamlRunner Failure, you must provide the directories: " + args?[0]);
	else
		Console.WriteLine("GenerateXamlRunner Failure, you must provide the directories.");

	if (args?.Length > 1)
		Console.WriteLine("GenerateXamlRunner Failure, you must provide the directories: " + args?[1]);

	return -1;
}

// See https://aka.ms/new-console-template for more information
Console.WriteLine("GenerateXamlRunner Begin...");
if (args != null && args.Length > 1)
{ 
	GenerateGenericFiles(args[0], args[1]);
}

//var fileNameSourcePath = "D:\\Source\\maui\\src\\Controls\\samples-gtk\\Controls.Sample.OnePage.GTK";
//var projectOutputPath = "D:\\Source\\maui\\src\\Controls\\samples-gtk\\Controls.Sample.OnePage.GTK\\obj\\Debug\\net6.0-windows\\prebuilt";
//GenerateGenericFiles(fileNameSourcePath, projectOutputPath);

//Generate1File();
//GenerateAnotherFile();
Console.WriteLine("GenerateXamlRunner Complete");
return 0;


void GenerateGenericFiles(string fileNameSourcePath, string projectOutputPath)
{
	//var fileNameSourcePath = "D:\\Source\\maui\\src\\Controls\\samples-gtk\\Controls.Sample.OnePage.GTK";

	foreach (var file in Directory.EnumerateFiles(fileNameSourcePath, "*.xaml", SearchOption.AllDirectories))
	{
		//var fileNameRoot = "MainPage.xaml";
		var fileNameRoot = Path.GetFileName(file);
		//var fullFileNamePath = Path.Combine(fileNameSourcePath, file);
		var fileSource = File.ReadAllText(file);
		//var projectOutputPath = "D:\\Source\\maui\\src\\Controls\\samples-gtk\\Controls.Sample.OnePage.GTK\\obj\\Debug\\net6.0-windows\\prebuilt";
		Directory.CreateDirectory(projectOutputPath);
		GenerateXaml.GenerateXamlCodeBehind(fileSource, fileNameRoot, fileNameSourcePath, fileNameRoot, projectOutputPath);
	}
}

//void Generate1File()
//{
//	var fileNameRoot = "MainPage.xaml";
//	var fileNameSourcePath = "D:\\Source\\maui\\src\\Controls\\samples-gtk\\Controls.Sample.OnePage.GTK";
//	var fullFileNamePath = Path.Combine(fileNameSourcePath, fileNameRoot);
//	var fileSource = File.ReadAllText(fullFileNamePath);
//	var projectOutputPath = "D:\\Source\\maui\\src\\Controls\\samples-gtk\\Controls.Sample.OnePage.GTK\\obj\\Debug\\net6.0-windows\\prebuilt";
//	Directory.CreateDirectory(projectOutputPath);
//	GenerateXaml.GenerateXamlCodeBehind(fileSource, fileNameRoot, fileNameSourcePath, fileNameRoot, projectOutputPath);
//}

//void GenerateAnotherFile()
//{
//	var fileNameRoot = "SandboxShell.xaml";
//	var fileNameSourcePath = "D:\\Source\\maui\\src\\Controls\\samples-gtk\\Controls.Sample.OnePage.GTK";
//	var fullFileNamePath = Path.Combine(fileNameSourcePath, fileNameRoot);
//	var fileSource = File.ReadAllText(fullFileNamePath);
//	var projectOutputPath = "D:\\Source\\maui\\src\\Controls\\samples-gtk\\Controls.Sample.OnePage.GTK\\obj\\Debug\\net6.0-windows\\prebuilt";
//	Directory.CreateDirectory(projectOutputPath);
//	GenerateXaml.GenerateXamlCodeBehind(fileSource, fileNameRoot, fileNameSourcePath, fileNameRoot, projectOutputPath);
//}
