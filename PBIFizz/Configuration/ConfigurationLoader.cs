using Microsoft.Extensions.Configuration;

namespace FizzCode.PBIFizz;
public static class ConfigurationLoader
{
    public static Config Load()
    {
        var Configuration = LoadFromJsonFile("config");

        var config = new Config();

        var autostart = Configuration.GetSection("Autostart");
        config.AutostartWithRecursiveSearchPaths = autostart.GetValue<bool>("WithRecursiveSearchPaths");
        config.AutostartAllWatchers = autostart.GetValue<bool>("AllWatchers");
        config.AutostartSearchWorkingDirectory = autostart.GetValue<bool>("SearchWorkingDirectory");

        var general = Configuration.GetSection("General");
        config.Settings.ReportFileName = general.GetValue<string>("ReportFileName");
        config.Settings.ToolPath = general.GetValue<string>("ToolPath");
        config.Settings.PlaceholderStart = general.GetValue<string>("PlaceholderStart");
        config.Settings.PlaceholderEnd = general.GetValue<string>("PlaceholderEnd");

        var watchers = Configuration.GetSection("Watchers").GetChildren();

        foreach(var watcher in watchers)
        {
            config.Watchers.Add(watcher.GetValue<string>("Name"), watcher.GetValue<string>("Path"));

        }

        return config;
    }

    public static IConfigurationRoot LoadFromJsonFile(string fileName, bool optional = false)
    {
        return new ConfigurationBuilder().AddJsonFile(fileName + "-local.json")
            .Build();
        /*return new ConfigurationBuilder().AddJsonFile(fileName + ".json", optional).AddJsonFile(fileName + "-local.json", optional: true).AddJsonFile(fileName + "-" + Environment.MachineName + ".json", optional: true)
            .Build();*/
    }
}