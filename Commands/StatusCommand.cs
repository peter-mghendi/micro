using CG.Web.MegaApiClient;
using Micro.Commands.Utilities;
using Spectre.Console.Cli;
using static CG.Web.MegaApiClient.NodeType;

namespace Micro.Commands;

public class StatusCommand : AsyncCommand<StatusCommand.Settings>
{
    public class Settings : CommandSettings
    {
        [CommandArgument(0, "<path>")] public required string Path { get; set; }
    }

    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
    {
        var state = ApplicationState.Instance;
        var nodes = (await state.Client.GetNodesAsync()).ToList();
        var node = Nodes.FindNodeByPath(settings.Path, state.WorkingDirectoryNode, nodes);
        WriteLine((await Nodes.GetNodeStatus(node, nodes)).ToString());

        return 0;
    }
}