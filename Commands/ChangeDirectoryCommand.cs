using CG.Web.MegaApiClient;
using Micro.Commands.Utilities;
using Spectre.Console.Cli;

namespace Micro.Commands;

public class ChangeDirectoryCommand : AsyncCommand<ChangeDirectoryCommand.Settings>
{
    public class Settings : CommandSettings
    {
        [CommandArgument(0, "[path]")] public string? Path { get; set; }
    }

    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
    {
        var state = ApplicationState.Instance;
        var nodes = (await state.Client.GetNodesAsync()).ToList();
        var node = nodes.Single(n => n is { Type: NodeType.Root });

        if (settings.Path is not null or "")
        {
            node = PathUtilities.FindNodeByPath(settings.Path, state.WorkingDirectoryNode, nodes);   
        }

        state.WorkingDirectoryNode = node.Id;
        state.WorkingDirectoryPath = PathUtilities.UnravelPathToRoot(node, nodes);
        return 0;
    }
}