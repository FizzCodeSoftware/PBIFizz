namespace FizzCode.PBIFizz;

internal partial class AppCommands
{
    private readonly List<string> commands = [];

    private void SetCommands()
    {
        var commands = ConsoleCommandHelper.GetCommands(this.GetType());
        this.commands.AddRange(commands);
    }
}
