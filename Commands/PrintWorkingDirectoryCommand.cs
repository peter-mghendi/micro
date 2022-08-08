using CG.Web.MegaApiClient;
using Spectre.Console.Cli;

namespace Micro.Commands;

public class PrintWorkingDirectoryCommand : AsyncCommand<PrintWorkingDirectoryCommand.Settings>
{
    public class Settings : CommandSettings
    {
    }

    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
    {
        var nodes = (await ApplicationState.Instance.Client.GetNodesAsync()).ToList();
        var currentNode = nodes.Single(n=> n.Id == ApplicationState.Instance.WorkingDirectory);
        AnsiConsole.MarkupLine(UnravelPathToRoot(currentNode, nodes));
        return 0;
    }

    private static string UnravelPathToRoot(INode node, List<INode> nodes, string path = "/")
    {
        if (node is { Type: NodeType.Root }) return $"/{path}";

        var parent = nodes.Single(n => n.Id == node.ParentId);
        return UnravelPathToRoot(parent, nodes, Path.Combine(node.Name, path.Trim('/')));
    }
}