using Spectre.Console.Cli;
using Configuration = Micro.Utilities.Configuration;

namespace Micro.Commands;

public class VersionCommand : Command<VersionCommand.Settings>
{
    public class Settings : CommandSettings;

    public override int Execute(CommandContext context, Settings settings)
    {
        WriteLine(Configuration.Version);
        return 0;
    }
}