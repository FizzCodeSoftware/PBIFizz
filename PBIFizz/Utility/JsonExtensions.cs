﻿using Newtonsoft.Json.Linq;

namespace FizzCode.PBIFizz;

public static class JsonExtensions
{
    public static void SetByPath(this JToken obj, string path, JToken value)
    {
        JToken token = obj.SelectToken(path);
        token.Replace(value);
    }

    public static List<JToken> FindTokens(this JToken containerToken, string name)
    {
        List<JToken> matches = new List<JToken>();
        FindTokens(containerToken, name, matches);
        return matches;
    }

    private static void FindTokens(JToken containerToken, string name, List<JToken> matches)
    {
        if (containerToken.Type == JTokenType.Object)
        {
            foreach (JProperty child in containerToken.Children<JProperty>())
            {
                if (child.Name == name)
                {
                    matches.Add(child.Value);
                }
                FindTokens(child.Value, name, matches);
            }
        }
        else if (containerToken.Type == JTokenType.Array)
        {
            foreach (JToken child in containerToken.Children())
            {
                FindTokens(child, name, matches);
            }
        }
    }
}