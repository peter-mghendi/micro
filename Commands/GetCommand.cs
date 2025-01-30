using System.IO.Compression;
using CG.Web.MegaApiClient;
using Micro.Commands.Utilities;
using Spectre.Console.Cli;

namespace Micro.Commands;

public class GetCommand : AsyncCommand<GetCommand.Settings>
{
    public class Settings : CommandSettings
    {
        [CommandArgument(0, "<path>")] public required string Path { get; set; }
        [CommandArgument(1, "[destination]")] public string? Destination { get; set; }
    }

    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
    {
        var state = ApplicationState.Instance;
        var nodes = (await state.Client.GetNodesAsync()).ToList();
        var node = Nodes.FindNodeByPath(settings.Path, state.WorkingDirectoryNode, nodes);

        var destination = settings.Destination is null ? node.Name : $"{settings.Destination}/{node.Name}";
        await (node.Type switch
        {
            NodeType.File => state.Client.DownloadFileAsync(node, destination),
            NodeType.Directory => DownloadFolder(state.Client, node, nodes, settings.Destination ?? "."),
            _ => throw new InvalidOperationException("Only files and directories can be downloaded.")
        });

        return 0;
    }

    private static async Task DownloadFolder(
        IMegaApiClient client, 
        INode target,
        List<INode> nodes,
        string destination,
        CancellationToken cancellationToken = default)
    {
        var name = target is { Type: NodeType.Root } ? "Root" : target.Name;
        var path = Path.Combine(destination, $"{name}.zip");
        
        using var archive = ZipFile.Open(path, ZipArchiveMode.Create);
        await DownloadFolderRecursive(target, target, client, nodes, archive, cancellationToken);

        MarkupLine($"Directory downloaded to [blue]{path}[/].");
    }
    
    private static async Task DownloadFolderRecursive(
        INode root,
        INode target,
        IMegaApiClient client,
        List<INode> nodes,
        ZipArchive archive,
        CancellationToken cancellationToken = default
    )
    {
        foreach (var node in Nodes.GetChildNodes(target, nodes))
        {
            switch (node.Type)
            {
                case NodeType.File:
                {
                    var name = Nodes.UnravelPathToNode(root, node, nodes);
                    var entry = archive.CreateEntry(name);
                    
                    MarkupLine($"Downloading [blue]{name}[/]...");
                    
                    await using var download = await client.DownloadAsync(node, cancellationToken: cancellationToken);
                    await using var stream = entry.Open();
                    await download.CopyToAsync(stream, cancellationToken);
                    break;
                }
                case NodeType.Root:
                case NodeType.Directory:
                    await DownloadFolderRecursive(root, node, client, nodes, archive, cancellationToken);
                    break;
                case NodeType.Inbox:
                case NodeType.Trash:
                default:
                    throw new InvalidOperationException("Only files and directories can be downloaded.");
            }
        }
    }
}