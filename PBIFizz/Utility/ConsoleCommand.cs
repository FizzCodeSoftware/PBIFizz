namespace FizzCode.PBIFizz;

public class ConsoleCommand
{
    private readonly List<string> previousCommands = [];
    private int previousCommandIndex = 0;
    private string prompt = "";

    public string ReadConsoleCommand()
    {
        var keyPresses = "";
        var breakFlag = false;
        while (!breakFlag)
        {
            var pressedKey = Console.ReadKey(true);
            switch (pressedKey.Key)
            {
                case ConsoleKey.UpArrow:
                    if (previousCommandIndex <= previousCommands.Count - 1)
                    {
                        ClearLine();
                        previousCommandIndex += 1;
                        Console.Write(prompt);
                        keyPresses = previousCommands[previousCommands.Count - previousCommandIndex];
                        Console.Write(keyPresses);
                    }
                    break;
                case ConsoleKey.DownArrow:
                    if (previousCommandIndex > 1)
                    {
                        ClearLine();
                        previousCommandIndex -= 1;
                        Console.Write(prompt);
                        keyPresses = previousCommands[previousCommands.Count - previousCommandIndex];
                        Console.Write(keyPresses);
                    }
                    break;
                case ConsoleKey.Enter:
                    Console.Write(pressedKey.KeyChar);
                    breakFlag = true;
                    break;
                case ConsoleKey.Tab:
                    break;
                case ConsoleKey.Backspace:
                    keyPresses = keyPresses[..^1];
                    Console.Write("\b \b");
                    break;
                default:
                    {
                        keyPresses += pressedKey.KeyChar;
                        Console.Write(pressedKey.KeyChar);
                        break;
                    }
            }
        }

        previousCommands.Add(keyPresses);
        return keyPresses;
    }

    public static void ClearLine()
    {
        Console.Write("\r" + new string(' ', Console.WindowWidth) + "\r");
    }

    public void SetPrompt(string prompt)
    {
        this.prompt = prompt;
    }
}