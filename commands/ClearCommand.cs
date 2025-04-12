public class ClearCommand : ICommand
{
    public void Execute(string args)
    {
        Console.Clear();
    }
}