using CommandDotNet;

namespace FizzCode.PBIFizz;

internal partial class AppCommands
{
    [Command("startAllWatchers", Description = "Start all watchers defined in config.")]
    public void StartAllWatchers()
    {
        if (Program.Config.AutostartAllWatchers)
        {
            foreach (var watcher in Program.Config.Watchers)
            {
                if (!State.RunningWatchers.ContainsKey(watcher.Key))
                {
                    var pbiFizz = new PbiFizz(watcher.Value, Program.Config.Settings.ReportFileName, Program.Config.Settings);
                    State.RunningWatchers.Add(watcher.Key, pbiFizz);
                    pbiFizz.Start();
                    Console.WriteLine(watcher.Key + " started.");
                }
                else
                {
                    Console.WriteLine(watcher.Key + " is already runnning.");
                }
            }
            Console.WriteLine();
        }
    }

    [Command("listWatchers", Description = "List all running watchers.")]
    public void ListWatchers()
    {
        foreach (var watcher in State.RunningWatchers)
        {
            Console.WriteLine(watcher.Key + " " + "watching " + watcher.Value);
        }
    }


    [Command("activateWatcher", Description = "Add watcher as an argument. Sets the watcher as the active watcher. Further commands will work with the active watcher.")]
    public void ActivateWatcher(string watcher)
    {
        var wathcherKvp = State.RunningWatchers.Where((x) => x.Key == watcher).Select(x => x).FirstOrDefault();
        if (wathcherKvp.Key != null)
        {
            State.ActiveWatcher = wathcherKvp.Key;
            Console.WriteLine(wathcherKvp.Key + " is activated, " + "watching " + wathcherKvp.Value.GetOriginalPathAndFileName());
        }
        else
        {
            Console.WriteLine(watcher + " - unknown Watcher.");
        }
    }

    [Command("activateAllWatchers", Description = "All running watchers will be active. Further commands will work with all running watcher.")]
    public void ActivateAllWatchers()
    {
        State.ActiveWatcher = "";
        ListWatchers();
    }
}
