using Micro.Commands.Utilities;
using Spectre.Console.Cli;

namespace Micro.Commands;

public class PutCommand : AsyncCommand<PutCommand.Settings>
{
    public class Settings : CommandSettings
    {
        [CommandArgument(0, "<path>")] public required string Path { get; set; }
        [CommandArgument(1, "[destination]")] public string? Destination { get; set; }
    }

    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
    {
        var state = ApplicationState.Instance;
        var nodes = (await state.Client.GetNodesAsync()).ToList();

        await state.Client.UploadFileAsync(settings.Path, Nodes.FindNodeByPath(settings.Destination ?? ".", state.WorkingDirectoryNode, nodes));
        return 0;
    }
}