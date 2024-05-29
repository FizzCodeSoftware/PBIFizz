namespace FizzCode.PBIFizz;
public class Config
{
    public bool AutostartWithRecursiveSearchPaths { get; set; }
    public bool AutostartAllWatchers { get; set; }
    public bool AutostartSearchWorkingDirectory { get; set; }
    public List<string> AutostartRecursiveSearchPaths { get; } = [];

    public Settings Settings { get; } = new Settings();
    public Dictionary<string, string> Watchers { get; } = [];
}
