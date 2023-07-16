using System.Collections.Specialized;
using CG.Web.MegaApiClient;

namespace Micro.Commands.Utilities;

public static class Nodes
{
    private const string empty = "";
    
    public static string UnravelPathToRoot(string node, List<INode> nodes)
    {
        var current = nodes.Single(n => n.Id == node);
        return UnravelPathToRoot(current, nodes);
    }

    public static string UnravelPathToRoot(INode node, List<INode> nodes, string path = empty)
    {
        var root = nodes.Single(n => n is { Type: NodeType.Root });
        return UnravelPathToNode(root, node, nodes, path);
    }

    public static string UnravelPathToNode(INode target, INode child, List<INode> nodes, string path = empty)
    {
        if (child.Id == target.Id)
        {
            return child is { Type: NodeType.Root } ? $"/{path}" : path;
        }

        if (child.ParentId is null or empty)
        {
            throw new InvalidOperationException($"Target node '{target.Name}' not found in hierarchy.");
        }

        var parent = nodes.Single(n => n.Id == child.ParentId);
        return UnravelPathToNode(target, parent, nodes, Path.Combine(child.Name, path.Trim('/')));
    }

    public static INode FindNodeByPath(string path, string start, List<INode> nodes)
    {
        var current = nodes.Single(n => n.Id == start);
        return FindNodeByPath(path, current, nodes);
    }
    
    // Figure out how to find nodes whose names contain slashes
    public static INode FindNodeByPath(string path, INode start, List<INode> nodes)
    {
        if (path.StartsWith("/"))
        {
            var root = nodes.Single(n => n is { Type: NodeType.Root });
            return FindNodeByPath(path.TrimStart('/'), root, nodes);
        }

        var current = start;
        foreach (var part in path.Split('/'))
        {
            if (part == ".") continue;

            INode? next;

            if (part == "..")
            {
                next = nodes.SingleOrDefault(n => n.Id == current.ParentId) ?? current;
            }
            else
            {
                next = DeduplicateNodes(GetChildNodes(current, nodes).Where(node => node.Name == part).ToList());
            }

            current = next ?? throw new DirectoryNotFoundException($"Directory {part} not found.");
        }

        return current;
    }

    public static async Task<NodeStatus> GetNodeStatus(INode node, List<INode> nodes)
    {
        if (node is not { Type: NodeType.File or NodeType.Directory })
        {
            throw new InvalidOperationException("Status operation is only valid for files and directories.");
        }

        var name = node is { Type: NodeType.Root } ? "/" : node.Name;
        var size = node is { Type: NodeType.File } ? node.Size : await node.GetFolderSizeAsync(nodes);
        return new NodeStatus(name, node.Id, node.Type, size, node.CreationDate, node.ModificationDate);
    }

    public static List<INode> GetChildNodes(INode node, List<INode> nodes)
    {
        return nodes.Where(n => n.ParentId == node.Id).ToList();
    }

    private static INode? DeduplicateNodes(IReadOnlyList<INode> nodes)
    {
        switch (nodes.Count)
        {
            case 0:
                return null;
            case 1:
                return nodes[0];
            default:
                var distinct = nodes.DistinctBy(node => node.Name).ToList();
                if (distinct.Skip(1).Any())
                    throw new InvalidOperationException("Cannot deduplicate multiple items at once");

                var prompt = new SelectionPrompt<INode>()
                    .Title($"Multiple items with name {distinct.Single().Name}. Select the correct item:")
                    .AddChoices(nodes)
                    .UseConverter(node => $"{node.Name} ({node.Type}) - Last Modified: {node.ModificationDate ?? node.CreationDate}");

                return AnsiConsole.Prompt(prompt);
        }
    }
}