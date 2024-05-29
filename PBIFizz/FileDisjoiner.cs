using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FizzCode.PBIFizz;

public class FileDisjoiner : FileBase
{
    public FileDisjoiner(string path, string file, Settings settings)
        : base(path, file, settings)
    {
    }

    public void Process()
    {
        var f = File.ReadAllText(Path + FileName);
        f = f.Replace("\"{", "{")
            .Replace("}\"", "}")
            .Replace("\"[{", "[{")
            .Replace("}]\"", "}]")
            .Replace("\\\"", "\"");

        var js = JToken.Parse(f);
        f = js.ToString(Formatting.Indented);

        //File.WriteAllText(GetToolReportJsonFileName(), f);

        var sections = js["sections"];

        Dictionary<string, int> sectionNameCounts = [];
        var sectionPlaceholders = new JArray();

        foreach (var section in sections)
        {
            var sectionName = (section["displayName"] ?? section["name"]).ToString();
            if (sectionNameCounts.ContainsKey(sectionName))
            {
                sectionNameCounts[sectionName] = sectionNameCounts[sectionName] + 1;
            }
            else
            {
                sectionNameCounts.Add(sectionName, 1);
            }
            f = section.ToString(Formatting.Indented);

            var sectionNameUnique = sectionName
                + (sectionNameCounts[sectionName] > 1
                ? "(" + sectionNameCounts[sectionName] + ")"
                : "");

            File.WriteAllText(GetSectionFileName(sectionNameUnique), f);

            sectionPlaceholders.Add(
                JObject.Parse("{\""
                + Settings.PlaceholderStart
                + sectionNameUnique
                + Settings.PlaceholderEnd
                + "\": \"\"}")
            );
        }

        js["sections"] = sectionPlaceholders;

        File.WriteAllText(GetToolReportJsonFileName(), js.ToString());

    }
}
