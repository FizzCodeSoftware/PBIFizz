using Newtonsoft.Json.Linq;

namespace FizzCode.PBIFizz;

public class FileJoiner : FileBase
{
    protected bool ShouldOverwriteOriginal { get; init; }
    public FileJoiner(string path, string file, Settings settings, bool shouldOverwriteOriginal = false)
        : base(path, file, settings)
    {
        ShouldOverwriteOriginal = shouldOverwriteOriginal;
    }

    public void Process()
    {
        var f = File.ReadAllText(GetToolReportJsonFileName());
        var json = JToken.Parse(f);

        List<string> jsonSections = [];

        foreach (var section in json["sections"])
        {
            var placeHolderName = section.Value<JObject>().Properties().First().Name;
            var sectionName = placeHolderName.TrimStart(Settings.PlaceholderStart.ToCharArray())
                .TrimEnd(Settings.PlaceholderEnd.ToCharArray());

            var sectionContent = File.ReadAllText(GetSectionFileName(sectionName));
            jsonSections.Add(sectionContent);
        }

        JArray jsonArray = new JArray();
        foreach (var jsonSection in jsonSections)
        {
            jsonArray.Add(JToken.Parse(jsonSection));
        }
        json["sections"] = jsonArray;

        json = ConvertToPbiJson(json);

        var fileNameAndPath = ShouldOverwriteOriginal
               ? this.GetOriginalPathAndFileName()
               : GetSectionFileName("original");
        Console.WriteLine("Saving file: " + fileNameAndPath);
        File.WriteAllText(fileNameAndPath, json.ToString(Newtonsoft.Json.Formatting.Indented, new DecimalJsonConverter()));
        
    }

    public JToken ConvertToPbiJson(JToken json)
    {
        json = ConvertToPbiJson(json, "config");
        json = ConvertToPbiJson(json, "parameters");
        json = ConvertToPbiJson(json, "filters");

        return json;
    }

    public JToken ConvertToPbiJson(JToken json, string key)
    {
        foreach (JToken token in json.FindTokens(key))
        {
            var s = JToken.Parse(token.ToString(Newtonsoft.Json.Formatting.None)).ToString(Newtonsoft.Json.Formatting.None);
            var s2 = s;
            if (s.StartsWith('{') || s.StartsWith('['))
            {
                s2 = "\"" + s.Replace("\"", "\\\"")
                    + "\"";

            }

            json.SetByPath(token.Path, JToken.Parse(s2));
        }

        return json;
    }
}
