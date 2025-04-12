using System;
using System.IO;

public class CdCommand : ICommand
{
    public void Execute(string args)
    {
        try
        {
            string targetDir = args.Trim();

            if (string.IsNullOrWhiteSpace(targetDir))
            {
                targetDir = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            }
            else if (targetDir == "..")
            {
                var parent = Directory.GetParent(Directory.GetCurrentDirectory());
                if (parent == null)
                {
                    Console.WriteLine("Already at root directory");
                    return;
                }
                targetDir = parent.FullName;
            }
            else if (targetDir == "/" || targetDir == "\\")
            {
                targetDir = Path.GetPathRoot(Directory.GetCurrentDirectory());
            }

            string fullPath = Path.IsPathRooted(targetDir)
                ? targetDir
                : Path.Combine(Directory.GetCurrentDirectory(), targetDir);

            if (!Directory.Exists(fullPath))
            {
                Console.WriteLine($"Directory not found: {fullPath}");
                return;
            }

            Directory.SetCurrentDirectory(fullPath);
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Error: {ex.Message}");
            Console.ResetColor();
        }
    }
}