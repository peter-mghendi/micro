using CG.Web.MegaApiClient;
using Micro.Commands.Utilities;
using Spectre.Console.Cli;
using static CG.Web.MegaApiClient.NodeType;

namespace Micro.Commands;

public class MoveCommand : AsyncCommand<MoveCommand.Settings>
{
    public class Settings : CommandSettings
    {
        [CommandArgument(0, "<path>")] public required string Path { get; set; }
        [CommandArgument(1, "<destination>")] public required string Destination { get; set; }
    }

    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
    {
        var state = ApplicationState.Instance;
        var nodes = (await state.Client.GetNodesAsync()).ToList();
        var node = Nodes.FindNodeByPath(settings.Path, state.WorkingDirectoryNode, nodes);

        if (node is not { Type: NodeType.File or NodeType.Directory })
        {
            AnsiConsole.WriteException(new InvalidOperationException("Only files can be downloaded."));
            return 1;
        }

        var destination = Nodes.FindNodeByPath(settings.Destination, state.WorkingDirectoryNode, nodes);
        await state.Client.MoveAsync(node, destination);
        return 0;
    }
}