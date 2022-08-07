using Spectre.Console.Cli;

namespace Micro.Commands;

public class PrintWorkingDirectoryCommand : Command<PrintWorkingDirectoryCommand.Settings>
{
    public class Settings : CommandSettings
    {
    }

    public override int Execute(CommandContext context, Settings settings)
    {
        AnsiConsole.MarkupLine(ApplicationState.Instance.WorkingDirectory);
        return 0;
    }
}