using CG.Web.MegaApiClient;
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
        var nodes = (await ApplicationState.Instance.Client.GetNodesAsync()).ToList();
        if (settings.Path is null)
        {
            ApplicationState.Instance.WorkingDirectory = nodes.Single(n => n is { Type: NodeType.Root }).Id;
            return 0;
        }
        
        var path = settings.Path.Split('/');
        var current = nodes.Single(n => n.Id == ApplicationState.Instance.WorkingDirectory);
        foreach (var part in path)
        {
            var children = nodes.Where(n => n.ParentId == current.Id).ToList();
            var next = children.SingleOrDefault(n => n.Name == part);
            if (next is null)
            {
                AnsiConsole.WriteException(new DirectoryNotFoundException($"Directory {part} not found."));
                return 1;
            }
            current = next;
        }
        ApplicationState.Instance.WorkingDirectory = current.Id;
        return 0;
    }
}