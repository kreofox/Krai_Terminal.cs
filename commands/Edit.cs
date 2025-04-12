using System;
using System.IO;

public class EditCommand : ICommand
{
    public void Execute(string args)
    {
        if (string.IsNullOrWhiteSpace(args))
        {
            Console.WriteLine("Usage: edit <filename>");
            return;
        }

        string filePath = Path.Combine(Directory.GetCurrentDirectory(), args);

        if (!File.Exists(filePath))
        {
            Console.WriteLine($"Create new file: {filePath}");
            File.WriteAllText(filePath, "");
        }

        Console.WriteLine($"Editing: {filePath}");
        Console.WriteLine("Press Ctrl+S to save, Esc to exit");

        EditFile(filePath);
    }

    private void EditFile(string filePath)
    {
        string[] lines = File.ReadAllLines(filePath);
        int currentLine = 0;
        ConsoleKeyInfo key;

        do
        {
            Console.Clear();
            DisplayContent(lines, currentLine);

            key = Console.ReadKey(true);

            switch (key.Key)
            {
                case ConsoleKey.UpArrow:
                    if (currentLine > 0) currentLine--;
                    break;

                case ConsoleKey.DownArrow:
                    if (currentLine < lines.Length - 1) currentLine++;
                    break;

                case ConsoleKey.Enter:
                    InsertLine(ref lines, ref currentLine);
                    break;

                case ConsoleKey.Backspace:
                    DeleteChar(ref lines, currentLine);
                    break;

                default:
                    if (!char.IsControl(key.KeyChar))
                        InsertChar(ref lines, currentLine, key.KeyChar);
                    break;
            }

        } while (key.Key != ConsoleKey.Escape && !(key.Key == ConsoleKey.S && key.Modifiers == ConsoleModifiers.Control));

        if (key.Key == ConsoleKey.S && key.Modifiers == ConsoleModifiers.Control)
        {
            File.WriteAllLines(filePath, lines);
            Console.WriteLine($"\nFile saved: {filePath}");
        }
    }

    private void DisplayContent(string[] lines, int currentLine)
    {
        for (int i = 0; i < lines.Length; i++)
        {
            if (i == currentLine)
            {
                Console.BackgroundColor = ConsoleColor.Gray;
                Console.ForegroundColor = ConsoleColor.Black;
            }

            Console.WriteLine(lines[i].PadRight(Console.WindowWidth - 1));
            Console.ResetColor();
        }

        Console.SetCursorPosition(lines[currentLine].Length, currentLine);
    }

    private void InsertLine(ref string[] lines, ref int currentLine)
    {
        Array.Resize(ref lines, lines.Length + 1);
        Array.Copy(lines, currentLine, lines, currentLine + 1, lines.Length - currentLine - 1);
        lines[currentLine + 1] = lines[currentLine].Substring(Console.CursorLeft);
        lines[currentLine] = lines[currentLine].Substring(0, Console.CursorLeft);
        currentLine++;
    }

    private void InsertChar(ref string[] lines, int line, char ch)
    {
        int pos = Console.CursorLeft;
        lines[line] = lines[line].Insert(pos, ch.ToString());
        Console.Write(ch);
        Console.SetCursorPosition(pos + 1, line);
    }

    private void DeleteChar(ref string[] lines, int line)
    {
        int pos = Console.CursorLeft;
        if (pos > 0)
        {
            lines[line] = lines[line].Remove(pos - 1, 1);
            Console.SetCursorPosition(pos - 1, line);
            Console.Write(" ");
            Console.SetCursorPosition(pos - 1, line);
        }
    }
}