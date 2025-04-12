using System; 

public class HelpCommand : ICommand
{
    public void Execute(string args)
    {
        Console.WriteLine("Commands");
        Console.WriteLine("help - Show this help");
        Console.WriteLine("clear - Clear screen");
        Console.WriteLine("exit - Exit the terminal");
        Console.WriteLine("neofetch - Show system info");
        Console.WriteLine("ls - file list");
        Console.WriteLine("dir - file list");
        Console.WriteLine("cd - file transfer"); 
    }
}
