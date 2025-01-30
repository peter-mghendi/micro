using Spectre.Console.Cli;

namespace Micro.Commands;

public class ClearCommand : Command<ClearCommand.Settings>
{
    public class Settings : CommandSettings;

    public override int Execute(CommandContext context, Settings settings)
    {
        Clear();
        return 0;
    }
}