using CG.Web.MegaApiClient;

namespace Micro.Models;

public record NodeStatus(string Name, string Id, NodeType Type, long Size, DateTime? CreatedAt, DateTime? ModifiedAt)
{
    public override string ToString() =>
        $"""
         Name: {Name}
         Node ID: {Id}
         Type: {Type}
         Size: {Size} bytes
         Created At: {CreatedAt}
         Last Modified At: {ModifiedAt ?? CreatedAt}
         """;
};