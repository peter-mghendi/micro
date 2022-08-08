using System.Text;
using CG.Web.MegaApiClient;
using Spectre.Console.Cli;

namespace Micro.Commands;

public class ListContentsCommand : AsyncCommand<ListContentsCommand.Settings>
{
    public class Settings : CommandSettings
    {
        [CommandArgument(0, "[path]")] public string? Path { get; set; }

        [CommandOption("-r|--recursive")] public bool? Recursive { get; set; }
    }

    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
    {
        var nodes = (await ApplicationState.Instance.Client.GetNodesAsync()).ToList();
        var parent = settings.Path is null 
            ? nodes.Single(n => n.Id == ApplicationState.Instance.WorkingDirectory)
            : GetNodeFromPath(settings.Path, nodes);
        var tree = BuildTreeRecursive(
            tree: new Tree($"[blue]{Emoji.Known.FileFolder} Root[/]"),
            recurse: settings.Recursive ?? false,
            nodes: nodes,
            parent: parent
        );
        AnsiConsole.Write(tree);
        return 0;
    }

    private INode GetNodeFromPath(string path, List<INode> nodes)
    {
        var root = nodes.Single(n => n.Type == NodeType.Root);
        var pathSegments =
            path.Split('/', '\\', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        if (pathSegments.Length <= 0) return root;

        foreach (var pathSegment in pathSegments)
        {
            root = nodes.Single(n => n.Name == pathSegment && n.Type == NodeType.Directory);
            nodes = nodes.FindAll(n => n.ParentId == root.Id);
        }

        return root;
    }

    private static Tree BuildTreeRecursive(Tree tree, bool recurse, List<INode> nodes, INode parent, int level = 0)
    {
        var children = nodes.Where(x => x.ParentId == parent.Id);
        foreach (var child in children)
        {
            var lineBuilder = new StringBuilder();
            var info = lineBuilder.Append(child.Type switch
                {
                    NodeType.Directory => Emoji.Known.FileFolder,
                    NodeType.File => Emoji.Known.PageFacingUp
                })
                .Append($" {child.Name}")
                .Append(child.Type switch
                {
                    NodeType.Directory => "",
                    NodeType.File => $" ({child.Size} bytes)"
                }).ToString();

            // Wrap the line in the appropriate color - blue for directories, white for files.
            info = child.Type switch
            {
                NodeType.Directory => $"[blue]{info}[/]",
                NodeType.File => info
            };


            if (recurse && child is {Type: NodeType.Directory})
            {
                var nestedTree = BuildTreeRecursive(
                    tree: new Tree(info),
                    recurse: recurse,
                    nodes: nodes,
                    parent: child,
                    level: level + 2
                );
                tree.AddNode(nestedTree);
            }
            else
            {
                tree.AddNode(info);
            }
        }

        return tree;
    }
}