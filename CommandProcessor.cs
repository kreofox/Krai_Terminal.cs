using System;
using System.Collections.Generic;
using System.Net.Http.Headers;

public class CommandProcessor
{
    private readonly Dictionary<string, ICommand> _commands;

    public CommandProcessor()
    {
        // Registration of all command
        _commands = new Dictionary<string, ICommand>(StringComparer.OrdinalIgnoreCase)
        {
            { "help", new HelpCommand() },
            { "clear", new ClearCommand() },
            { "exit", new ExitCommand() },
            { "neofetch", new NeofetchCommand() },
            { "ls", new LsCommand() },
            { "dir", new LsCommand() },
            { "cd", new CdCommand() } ,
            {"open", new EditCommand() },
            {"nano", new EditCommand() }
        };
    }

    public void Execute(string input)
    {
        var parts = input.Split(new[] { ' ' }, 2);
        var commandName = parts[0];
        var args = parts.Length > 1 ? parts[1] : string.Empty;

        if (_commands.TryGetValue(commandName, out var command))
        {
            command.Execute(args);
        }
        else
        {
            Console.WriteLine($"Command not found: {commandName}");
        }
    }
}