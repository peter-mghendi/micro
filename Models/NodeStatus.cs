using CG.Web.MegaApiClient;

namespace Micro;

public record NodeStatus(string Name, string Id, NodeType Type, long Size, DateTime? CreatedAt,
    DateTime? LastModifiedAt)
{
    public override string ToString()
    {
        return $"""
        Name: {Name}
        Node ID: {Id}
        Type: {Type}
        Size: {Size} bytes
        Created At: {CreatedAt?.ToString()}
        Last Modified At: {LastModifiedAt?.ToString()}
        """;
    }
};