using Spectre.Console.Cli;

namespace Micro.Commands;

public class PrintWorkingDirectoryCommand : AsyncCommand<PrintWorkingDirectoryCommand.Settings>
{
    public class Settings : CommandSettings
    {
    }

    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
    {
        AnsiConsole.MarkupLine("/");
        await Task.Delay(TimeSpan.Zero);
        return 0;
    }
}