using CG.Web.MegaApiClient;

namespace Micro.Commands.Utilities;

public static class PathUtilities
{
    public static string UnravelPathToRoot(string node, List<INode> nodes)
    {
        var current = nodes.Single(n=> n.Id == node);
        return UnravelPathToRoot(current, nodes);
    }
    
    public static string UnravelPathToRoot(INode node, List<INode> nodes, string path = "")
    {
        if (node is { Type: NodeType.Root }) return $"/{path}";

        var parent = nodes.Single(n => n.Id == node.ParentId);
        return UnravelPathToRoot(parent, nodes, Path.Combine(node.Name, path.Trim('/')));
    }
}