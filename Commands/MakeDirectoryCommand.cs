using Spectre.Console.Cli;

namespace Micro.Commands;

public class MakeDirectoryCommand : AsyncCommand<MakeDirectoryCommand.Settings>
{
    public class Settings : CommandSettings
    {
        [CommandArgument(0, "<name>")] public required string Name { get; set; }
    }

    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
    {
        var state = ApplicationState.Instance;
        var nodes = (await state.Client.GetNodesAsync()).ToList();
        var current = nodes.Single(n => n.Id == state.WorkingDirectoryNode);

        await state.Client.CreateFolderAsync(settings.Name, current);
        return 0;
    }
}