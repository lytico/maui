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

Console.WriteLine("GenerateXamlRunner Begin...");

if (args != null && args.Length > 1)
{
	GenerateXamlFiles(args[0], args[1]);
}

Console.WriteLine("GenerateXamlRunner Complete");
return 0;


void GenerateXamlFiles(string fileNameSourcePath, string projectOutputPath)
{
	foreach (var file in Directory.EnumerateFiles(fileNameSourcePath, "*.xaml", SearchOption.AllDirectories))
	{
		var fileNameRoot = Path.GetFileName(file);
		var fileSource = File.ReadAllText(file);
		Directory.CreateDirectory(projectOutputPath);
		GenerateXaml.GenerateXamlCodeBehind(fileSource, fileNameRoot, fileNameSourcePath, fileNameRoot, projectOutputPath);
	}
}
