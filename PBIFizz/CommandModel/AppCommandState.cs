namespace FizzCode.PBIFizz;

public class AppCommandState
{
    public string ActiveWatcher { get; set; } = "";
    public Dictionary<string, PbiFizz> RunningWatchers { get; set; } = [];
}
