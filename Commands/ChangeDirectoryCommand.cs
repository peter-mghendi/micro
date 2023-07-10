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

        if (settings.Path is null or "")
        {
            var root = nodes.Single(n => n is { Type: NodeType.Root }).Id;
            state.WorkingDirectoryNode = root;
            state.WorkingDirectoryPath = PathUtilities.UnravelPathToRoot(root, nodes);
            return 0;
        }

        var current = nodes.Single(n => n.Id == state.WorkingDirectoryNode);
        var node = PathUtilities.FindNodeByPath(settings.Path, current, nodes);

        state.WorkingDirectoryNode = node.Id;
        state.WorkingDirectoryPath = PathUtilities.UnravelPathToRoot(node.Id, nodes);
        return 0;
    }
}