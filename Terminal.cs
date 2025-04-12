using System;
using System.IO;
using System.Linq;
using System.Diagnostics;

public class Terminal : IDisposable
{
    private readonly CommandProcessor _commandProcessor;
    private string _currentDirectory;
    private bool _disposed = false;

    public Terminal()
    {
        _currentDirectory = Directory.GetCurrentDirectory();
        _commandProcessor = new CommandProcessor();
    }

    public void Run()
    {
        if (_disposed)
            throw new ObjectDisposedException(nameof(Terminal));

        ShowWelcomeMessage();

        while (true)
        {
            try
            {
                UpdatePrompt();
                string input = Console.ReadLine()?.Trim();

                if (string.IsNullOrEmpty(input))
                    continue;

                ProcessCommand(input);
            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
            }
        }
    }

    public void RunWithFile(string filePath)
    {
        if (_disposed)
            throw new ObjectDisposedException(nameof(Terminal));

        if (!File.Exists(filePath))
        {
            ShowError($"File not found: {filePath}");
            Run();
            return;
        }

        Console.Title = $"Terminal - {Path.GetFileName(filePath)}";
        new EditCommand().Execute(filePath);
        Run();
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                // Освобождаем управляемые ресурсы
                if (_commandProcessor is IDisposable disposableProcessor)
                {
                    disposableProcessor.Dispose();
                }
            }
            // Здесь можно освободить неуправляемые ресурсы, если они есть
            _disposed = true;
        }
    }

    ~Terminal()
    {
        Dispose(false);
    }

    private void ShowWelcomeMessage()
    {
        Console.WriteLine("Hello, command-help");
        Console.WriteLine(@"
--   ##  ##   #####      ##      ####
--   ## ##    ##  ##    ####      ##
--   ####     ##  ##   ##  ##     ##
--   ###      #####    ######     ##
--   ####     ####     ##  ##     ##
--   ## ##    ## ##    ##  ##     ##
--   ##  ##   ##  ##   ##  ##    ####");
    }

    private void ProcessCommand(string input)
    {
        if (input.StartsWith("cd ", StringComparison.OrdinalIgnoreCase) ||
            input.Equals("cd", StringComparison.OrdinalIgnoreCase))
        {
            new CdCommand().Execute(input.Length > 2 ? input[3..] : "");
            _currentDirectory = Directory.GetCurrentDirectory();
        }
        else
        {
            _commandProcessor.Execute(input);
        }
    }

    private void UpdatePrompt()
    {
        string displayPath = GetDisplayPath();
        string branch = GetGitBranch();

        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.Write(displayPath);

        if (!string.IsNullOrEmpty(branch))
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write($" [{branch}]");
        }

        Console.ForegroundColor = ConsoleColor.White;
        Console.Write("> ");
        Console.ResetColor();
    }

    private string GetDisplayPath()
    {
        string path = Directory.GetCurrentDirectory();
        string homePath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        return path.Replace(homePath, "~");
    }

    private string GetGitBranch()
    {
        try
        {
            var dir = new DirectoryInfo(_currentDirectory);
            while (dir != null && !Directory.Exists(Path.Combine(dir.FullName, ".git")))
            {
                dir = dir.Parent;
            }

            if (dir != null)
            {
                string headFile = Path.Combine(dir.FullName, ".git", "HEAD");
                if (File.Exists(headFile))
                {
                    string head = File.ReadAllText(headFile);
                    if (head.Contains("ref: refs/heads/"))
                    {
                        return head.Replace("ref: refs/heads/", "").Trim();
                    }
                    return head.Trim();
                }
            }
        }
        catch { }
        return "";
    }

    private void ShowError(string message)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"Error: {message}");
        Console.ResetColor();
    }
}