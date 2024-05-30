using CommandDotNet;

namespace FizzCode.PBIFizz;

internal partial class AppCommands
{
    public AppCommandState State { get { return Program.State; } }

    [Command("exit", Description = "Exit from the command-line utility.")]
    public void Exit()
    {
        Program.Terminated = true;
    }

    

    [Command("join", Description = "Joins the partial json files to 'report.original.json' file.")]
    public void Join()
    {
        foreach (var watcher in State.RunningWatchers)
        {
            var path = Program.Config.Watchers[watcher.Key];
            var fj = new FileJoiner(path, Program.Config.Settings.ReportFileName, Program.Config.Settings);
            fj.Process();
        }
    }

    [Command("disjoin", Description = "Force the breakup of the report.json file. Usable if PbiFizz was not running in autodetect mode.")]
    public void Disjoin()
    {
        foreach (var watcher in State.RunningWatchers)
        {
            var path = Program.Config.Watchers[watcher.Key];
            var fd = new FileDisjoiner(path, Program.Config.Settings.ReportFileName, Program.Config.Settings);
            Console.WriteLine("Processing: " + fd.GetOriginalPathAndFileName());
            fd.Process();
        }
    }

    [Command("build", Description = "Join the partial json files and override the original 'report.json' file.")]
    public void Build()
    {
        foreach (var watcher in State.RunningWatchers)
        {
            var path = Program.Config.Watchers[watcher.Key];
            var fj = new FileJoiner(path, Program.Config.Settings.ReportFileName, Program.Config.Settings, true);
            fj.Process();
        }

        Console.WriteLine("You need to close and reopen the PBI reports for the changes to take effect.");
        Console.WriteLine("Don't save changes from Power BI, because then it overwrites the report.json file.");
        Console.WriteLine("");
    }
}
