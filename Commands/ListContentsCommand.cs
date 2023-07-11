using System.Text;
using CG.Web.MegaApiClient;
using Micro.Commands.Utilities;
using Spectre.Console.Cli;
using static CG.Web.MegaApiClient.NodeType;

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
        var state = ApplicationState.Instance;
        var nodes = (await state.Client.GetNodesAsync()).ToList();
        var parent = Nodes.FindNodeByPath(settings.Path ?? ".", state.WorkingDirectoryNode, nodes);
        
        var name = parent is { Type: Root } ? "/" : parent.Name;
        var tree = BuildTreeRecursive(
            tree: new Tree($"[blue]{Emoji.Known.FileFolder} {name}[/]"),
            recurse: settings.Recursive ?? false,
            nodes: nodes,
            parent: parent
        );
        AnsiConsole.Write(tree);
        return 0;
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