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
        Console.WriteLine("FizzCode Pbi Fizz tool. To break up reports.json by pages, for better source control and editing it manually or by tools. (C) FizzCode 2024. Developed by gerleim.");
        Console.WriteLine("Beta software. No warranty.");
        Console.WriteLine("Requires Power BI Desktop setting to save the report a pbip file.");
        Console.WriteLine("");
        Console.WriteLine("Autodetect the change of the report.json file set in code PbiFizz.path.");
        Console.WriteLine("");
        Console.WriteLine("Commands:");
        Console.WriteLine("q/quit - exit the program");
        Console.WriteLine("j/join - join the partial json files to 'report.original.json' file");
        Console.WriteLine("b/build - join the partial json files and override the original 'report.json' file");
        Console.WriteLine("d/disjoin - force the breakup of the report.json file. Usable if PbiFizz was not running in autodetect mode.");
        Console.WriteLine("");
        Console.WriteLine("Enter command:");
    }

    public static void PH()
    {
        /*var commandLine = Console.ReadLine();
        while (commandLine != "q" && commandLine != "quit")
        {
            if (commandLine == "j" || commandLine == "join")
            {
                var fj = new FileJoiner(UserSettings.path, Settings.file);
                fj.Process();
            }
            if (commandLine == "b" || commandLine == "build")
            {
                var fj = new FileJoiner(UserSettings.path, Settings.file, true);
                fj.Process();
                Console.WriteLine("You need to close and reopen the PBI report for the changes to take effect.");
                Console.WriteLine("Don't save changes from Power BI, because then it overwrites the report.json file.");
                Console.WriteLine("");
            }
            if (commandLine == "d" || commandLine == "disjoin")
            {
                var fd = new FileDisjoiner(UserSettings.path, Settings.file);
                Console.WriteLine("Processing: " + fd.GetOriginalPathAndFileName());
                fd.Process();
            }
            commandLine = Console.ReadLine();
        }*/
    }
}