using Micro.Commands.Utilities;
using Spectre.Console.Cli;

namespace Micro.Commands;

public class TouchCommand : AsyncCommand<TouchCommand.Settings>
{
    public class Settings : CommandSettings
    {
        [CommandArgument(0, "<name>")] public required string Name { get; set; }
    }

    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
    {
        var state = ApplicationState.Instance;
        var nodes = (await state.Client.GetNodesAsync()).ToList();
        var current = nodes.Single(n => n.Id == state.WorkingDirectoryNode);

        var name = settings.Name;
        var node = name.Split('/').Skip(1).Any()
            ? Nodes.FindNodeByPath(name.Remove(name.LastIndexOf('/')), current, nodes)
            : current;
        
        await state.Client.UploadAsync(Stream.Null, name.Split('/')[^1], node);
        return 0;
    }
}