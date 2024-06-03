using System.Text;
using System.Text.RegularExpressions;

namespace FizzCode.PBIFizz;

public class FileNameEscaper
{
    private const string invalids = "/\\<>\"|?";
    private readonly Dictionary<int, char> map = [];
    public readonly char escape = '_';

    private FileNameEscaper()
    {
        foreach(var c in invalids)
        {
            map.Add(
                Encoding.ASCII.GetBytes(new char[1] { c })[0]
                , c);
        }
    }

    private static readonly FileNameEscaper Instance = new FileNameEscaper();

    public static string Escape(string input)
    {
        StringBuilder sb = new StringBuilder();

        var escapeEscaped = "_" + Encoding.ASCII.GetBytes(new char[1] { Instance.escape })[0] + "_";
        var escaped = input.Replace(Instance.escape.ToString(), escapeEscaped);
        
        foreach (char c in escaped)
        {
            if (invalids.Contains(c))
            {
                var cc = new char[1] { c };
                sb.Append("_" + Encoding.ASCII.GetBytes(new char[1] { c })[0] + "_");
            }
            else
            {
                sb.Append(c);
            }
        }

        return sb.ToString();
    }

    private static readonly Regex escapeRegex = new("_(.*?)_", RegexOptions.Compiled);

    public static string Unescape(string input)
    {
        Regex escapeRegex2 = new Regex("_(.*?)_", RegexOptions.Compiled);
        string unescaped = escapeRegex.Replace(input,
            delegate (Match m) {
                var match = m.Value.Substring(1, m.Value.Length - 2);
                var code = int.Parse(match);
                return ((char)code).ToString();
            }
        );

        return unescaped;
    }
}