using System;
using System.IO;
using System.Linq;

public class LsCommand : ICommand
{
    public void Execute(string args)
    {
        try
        {
            // Parsing arguments
            var parsedArgs = args.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            string pathArg = parsedArgs.FirstOrDefault(arg => !arg.StartsWith("-")) ?? ".";
            bool showHidden = parsedArgs.Contains("-a");
            bool longFormat = parsedArgs.Contains("-l");

            // Directory definition
            string directory = Path.GetFullPath(pathArg);

            if (!Directory.Exists(directory))
            {
                Console.WriteLine($"Directory does not exist: {directory}");
                return;
            }

            // Retrieving contents with flags
            var dirs = Directory.GetDirectories(directory)
                          .Where(d => showHidden || !IsHidden(d))
                          .OrderBy(d => d);

            var files = Directory.GetFiles(directory)
                          .Where(f => showHidden || !IsHidden(f))
                          .OrderBy(f => f);

            // Info output
            Console.WriteLine($"Contents of {directory}:\n");

            //folder output
            Console.ForegroundColor = ConsoleColor.Blue;
            foreach (var dir in dirs)
            {
                Console.WriteLine($"[{Path.GetFileName(dir)}]");
            }

            //File output
            Console.ForegroundColor = ConsoleColor.Green;
            foreach (var file in files)
            {
                Console.WriteLine(Path.GetFileName(file));
            }

            Console.ResetColor();
            Console.WriteLine($"\nTotal: {files.Count()} files, {dirs.Count()} directories");
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Error: {ex.Message}");
            Console.ResetColor();
        }
    }

    private bool IsHidden(string path)
    {
        try
        {
            return (File.GetAttributes(path) & FileAttributes.Hidden) == FileAttributes.Hidden;
        }
        catch
        {
            return false;
        }
    }
}