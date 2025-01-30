using CG.Web.MegaApiClient;
using Micro.Commands.Utilities;
using Spectre.Console.Cli;

namespace Micro.Commands;

public class RemoveCommand : AsyncCommand<RemoveCommand.Settings>
{
    public class Settings : CommandSettings
    {
        [CommandArgument(0, "<path>")] public required string Path { get; set; }
        
        [CommandOption("-f|--force")] public bool? Force { get; set; }
    }

    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
    {
        var state = ApplicationState.Instance;
        var nodes = (await state.Client.GetNodesAsync()).ToList();
        var node = Nodes.FindNodeByPath(settings.Path, state.WorkingDirectoryNode, nodes);

        if (node is not { Type: NodeType.File or NodeType.Directory })
        {
            WriteException(new InvalidOperationException("Only files and directories can be downloaded."));
            return 1;
        }
        
        await state.Client.DeleteAsync(node, !(settings.Force ??= false));
        return 0;
    }
}