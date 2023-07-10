using CG.Web.MegaApiClient;
using Micro.Commands.Utilities;
using Spectre.Console.Cli;

namespace Micro.Commands;

public class ChangeDirectoryCommand : AsyncCommand<ChangeDirectoryCommand.Settings>
{
    public class Settings : CommandSettings
    {
        [CommandArgument(0, "[path]")] public string? Path { get; set; }
    }

    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
    {
        var state = ApplicationState.Instance;
        var nodes = (await state.Client.GetNodesAsync()).ToList();

        if (settings.Path is null)
        {
            var root = nodes.Single(n => n is { Type: NodeType.Root }).Id;
            state.WorkingDirectoryNode = root;
            state.WorkingDirectoryPath = PathUtilities.UnravelPathToRoot(root, nodes);
            return 0;
        }
        
        var path = settings.Path.Split('/');
        var current = nodes.Single(n => n.Id == state.WorkingDirectoryNode);
        foreach (var part in path)
        {
            if (part == ".") continue;

            INode? next;
            var back = part == "..";
            
            if (back)
            {
                next = nodes.SingleOrDefault(n => n.Id == current.ParentId) ?? current;
            }
            else
            {
                next = nodes.Where(n => n.ParentId == current.Id)
                    .ToList()
                    .SingleOrDefault(n => n.Name == part);   
            }
            
            if (next is null)
            {
                AnsiConsole.WriteException(new DirectoryNotFoundException($"Directory {part} not found."));
                return 1;
            }
            current = next;
        }
        
        state.WorkingDirectoryNode = current.Id;
        state.WorkingDirectoryPath = PathUtilities.UnravelPathToRoot(current.Id, nodes);
        return 0;
    }
}