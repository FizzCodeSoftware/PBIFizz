using CommandDotNet;
using System.Reflection;

namespace FizzCode.PBIFizz;

public static class ConsoleCommandHelper
{
    public static  List<string> GetCommands(Type type)
    {
        var members = type.GetMembers();

        var commands = members
            .Where(m => m.GetCustomAttributes<CommandAttribute>().Count() > 0)
            .Select(x => x.GetCustomAttributes<CommandAttribute>().First().Name)
            .ToList();

        return commands;
    }
}
