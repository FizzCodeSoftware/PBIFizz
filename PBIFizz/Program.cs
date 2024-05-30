using CommandDotNet;
using CommandDotNet.Help;
using System.Text.RegularExpressions;

namespace FizzCode.PBIFizz;

public static class Program
{
    public static Config Config { get; private set; } = new Config();
    public static AppCommandState State { get;} = new AppCommandState();
    public static bool Terminated { get; set; }
    public static ConsoleCommand ConsoleCommand { get; } = new ConsoleCommand(ConsoleCommandHelper.GetCommands(typeof(AppCommands)));

    public static void Main(string[] args)
    {
        Config = ConfigurationLoader.Load();

        DsiaplayInfo();

        if (args.Length > 0)
        {
            var runner = new AppRunner<AppCommands>(GetAppSettings(""));
            runner.Run(args);
        }
        else
        {
            var runner = new AppRunner<AppCommands>(GetAppSettings(""));
            runner.Run("startAllWatchers");
        }

        DisplayHelp();

        var regEx = new Regex("(?<=\")[^\"]*(?=\")|[^\" ]+");

        ConsoleCommand.SetPrompt(GetPrompt());

        while (!Terminated)
        {
            Console.Write(GetPrompt());
            var commandLine = ConsoleCommand.ReadConsoleCommand();

            if (string.IsNullOrEmpty(commandLine))
                continue;

            var lineArguments = regEx
                .Matches(commandLine.Trim())
                .Select(x => x.Value.Trim())
                .ToArray();

            var runner = new AppRunner<AppCommands>(GetAppSettings(State.ActiveWatcher));
            runner.Run(lineArguments);

            Console.WriteLine();
        }
    }

    private static string GetPrompt()
    {
        return (State.ActiveWatcher == "" ? "(all)" : State.ActiveWatcher)
                        + "> ";
    }

    internal static void DisplayHelp(string? command = null)
    {
        var runner = new AppRunner<AppCommands>(GetAppSettings(""));

        if (string.IsNullOrEmpty(command))
        {
            runner.Run("--help");
        }
        else
        {
            var args = command.Split(' ').ToList();
            args.Add("--help");
            runner.Run(args.ToArray());
        }
    }


    private static AppSettings GetAppSettings(string prompt)
    {
        return new AppSettings()
        {
            Help = new AppHelpSettings()
            {
                TextStyle = HelpTextStyle.Basic,
                UsageAppName = prompt + ">",
                PrintHelpOption = false,
            }
        };
    }

    public static void DsiaplayInfo()
    {
        Console.WriteLine("FizzCode PBI Fizz tool. To break up reports.json by pages, for better source control and editing manually or by tools. (C) FizzCode 2024. Developed by gerleim.");
        Console.WriteLine("Beta software. No warranty.");
        Console.WriteLine("Requires Power BI Desktop setting to save the report a pbip file.");
        Console.WriteLine("");
        Console.WriteLine("Autodetects the changes of the report.json file(s) set in the configuration.");
        Console.WriteLine("");
    }
}