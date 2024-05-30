namespace FizzCode.PBIFizz;

public class ConsoleCommandTabState
{
    public int MatchedCommandIndex { get; set; } = 0;
    public string? LastEnteredPart { get; set; }
}

public class ConsoleCommand
{
    private readonly List<string> commands = [];
    private string prompt = "";
    private readonly List<string> previousEnteredCommands = [];
    private int previousCommandIndex = 0;
    private ConsoleCommandTabState TabState = new();
    private int cursorPosition = 0;

    public ConsoleCommand(List<string> commands)
    {
        this.commands = commands.OrderBy(c => c).ToList();
    }

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
                    if (previousCommandIndex <= previousEnteredCommands.Count - 1)
                    {
                        previousCommandIndex += 1;
                        keyPresses = HandleNextOrPreviousCommand();
                        cursorPosition = keyPresses.Length;
                    }
                    break;
                case ConsoleKey.DownArrow:
                    if (previousCommandIndex > 1)
                    {
                        previousCommandIndex -= 1;
                        keyPresses = HandleNextOrPreviousCommand(); ;
                        cursorPosition = keyPresses.Length;
                    }
                    break;
                case ConsoleKey.Enter:
                    Console.Write(pressedKey.KeyChar);
                    cursorPosition = 0;
                    breakFlag = true;
                    break;
                case ConsoleKey.Tab:
                    keyPresses = HandleTab(keyPresses);
                    ReplaceLine(keyPresses);
                    cursorPosition = keyPresses.Length;
                    break;
                case ConsoleKey.Backspace:
                    if (keyPresses.Length > 0)
                    {
                        cursorPosition -= 1;
                        keyPresses = keyPresses[..^1];
                        Console.Write("\b \b");
                    }
                    break;
                case ConsoleKey.LeftArrow:
                    if (cursorPosition > 0)
                    {
                        cursorPosition -= 1;
                        Console.CursorLeft -= 1;
                    }
                    break;
                case ConsoleKey.RightArrow:
                    if (cursorPosition < keyPresses.Length)
                    {
                        cursorPosition += 1;
                        Console.CursorLeft += 1;
                    }
                    break;
                case ConsoleKey.Insert:
                    // TODO toggle Insert mode
                    // Console.CursorSize += 1;
                    break;
                default:
                    {
                        var currentCusrsorPosition = Console.CursorLeft;
                        keyPresses = keyPresses.Insert(cursorPosition, pressedKey.KeyChar.ToString());
                        //Console.Write(pressedKey.KeyChar.ToString());
                        ReplaceLine(keyPresses);
                        cursorPosition += 1;
                        Console.CursorLeft = currentCusrsorPosition + 1;
                        //Console.CursorLeft = currentCusrsorPosition + cursorPosition;
                        break;
                    }
            }

            if (pressedKey.Key != ConsoleKey.Tab
                && pressedKey.Key != ConsoleKey.RightArrow
                && pressedKey.Key != ConsoleKey.LeftArrow)
                TabState.LastEnteredPart = null;
        }

        previousEnteredCommands.Add(keyPresses);
        return keyPresses;
    }

    private void ReplaceLine(string keyPresses)
    {
        ClearLine();
        Console.Write(prompt);
        Console.Write(keyPresses);
    }

    private string HandleTab(string keyPresses)
    {
        if (TabState.LastEnteredPart == null)
            TabState.LastEnteredPart = keyPresses;
        
        var matches = commands.Where(c => c.StartsWith(TabState.LastEnteredPart)).ToList();
        if (matches.Any())
        {
            if (TabState.MatchedCommandIndex < matches.Count - 1)
                TabState.MatchedCommandIndex += 1;
            else
                TabState.MatchedCommandIndex = 0;

            return matches[TabState.MatchedCommandIndex];
        }
        else
        {
            return keyPresses;
        }
    }

    private string HandleNextOrPreviousCommand()
    {
        string keyPresses;
        keyPresses = previousEnteredCommands[^previousCommandIndex];
        ReplaceLine(keyPresses);
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