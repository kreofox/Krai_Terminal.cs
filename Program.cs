using System;
using System.Diagnostics;
using System.IO;
using Microsoft.Win32;
using System.Runtime.InteropServices;

class Program
{
    // 1. First we declare all auxiliary methods
    public static void RegisterFileAssociation()
    {
        try
        {
            string appPath = Process.GetCurrentProcess().MainModule.FileName;
            string menuText = "Open in Terminal";
            string iconPath = $"\"{appPath}\",0";
            string command = $"\"{appPath}\" \"%1\"";

            using (var key = Registry.ClassesRoot.CreateSubKey(@"*\shell\TerminalOpen"))
            {
                key.SetValue("", menuText);
                key.SetValue("Icon", iconPath);
                key.SetValue("Position", "Top");
            }

            using (var key = Registry.ClassesRoot.CreateSubKey(@"*\shell\TerminalOpen\command"))
            {
                key.SetValue("", command);
            }

            Console.WriteLine("File association registered successfully!");
        }
        catch (UnauthorizedAccessException)
        {
            Console.WriteLine("Error: Requires administrator privileges");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Registration failed: {ex.Message}");
        }
    }

    static void CreateShortcut()
    {
        try
        {
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            string shortcutPath = Path.Combine(desktopPath, "My Terminal.lnk");
            string exePath = Process.GetCurrentProcess().MainModule.FileName;

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                string psScript = $@"
                    $WshShell = New-Object -comObject WScript.Shell
                    $Shortcut = $WshShell.CreateShortcut('{shortcutPath}')
                    $Shortcut.TargetPath = '{exePath}'
                    $Shortcut.Arguments = '%1'
                    $Shortcut.WorkingDirectory = '{Path.GetDirectoryName(exePath)}'
                    $Shortcut.Save()
                ";
                Process.Start("powershell", $"-Command \"{psScript}\"")?.WaitForExit();
            }
            else
            {
                string desktopEntry = $"""
                    [Desktop Entry]
                    Name=My Terminal
                    Exec={exePath} %F
                    Type=Application
                    Terminal=true
                    """;
                File.WriteAllText(shortcutPath.Replace(".lnk", ".desktop"), desktopEntry);
            }

            Console.WriteLine($"Shortcut created: {shortcutPath}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error creating shortcut: {ex.Message}");
        }
    }

    // 2. Then the main method Main
    static void Main(string[] args)
    {
        if (args.Contains("--create-shortcut"))
        {
            CreateShortcut();
            return;
        }

        if (args.Contains("--register-file-assoc"))
        {
            RegisterFileAssociation();
            return;
        }

        using (var terminal = new Terminal())
        {
            if (args.Length > 0 && File.Exists(args[0]))
            {
                terminal.RunWithFile(args[0]);
            }
            else
            {
                terminal.Run();
            }
        }
    }

    // 3. Additional supporting methods
    private static string ResolveFilePath(string path)
    {
        try
        {
            return Path.GetFullPath(Environment.ExpandEnvironmentVariables(path));
        }
        catch
        {
            return path;
        }
    }

    private static void HandleCriticalError(Exception ex)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("FATAL ERROR:");
        Console.WriteLine(ex.Message);
        Console.ResetColor();
        Console.WriteLine("\nPress any key to exit...");
        Console.ReadKey();
    }
}