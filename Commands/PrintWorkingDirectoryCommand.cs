using System.ComponentModel;
using CG.Web.MegaApiClient;
using Micro.Commands.Utilities;
using Spectre.Console.Cli;

namespace Micro.Commands;

public class PrintWorkingDirectoryCommand : AsyncCommand<PrintWorkingDirectoryCommand.Settings>
{
    public class Settings : CommandSettings
    {
        [CommandOption("-f|--force")]
        [Description("When set, micro will bypass the cache and re-compute the current path")]
        public bool? Force { get; set; }
    }

    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
    {
        var state = ApplicationState.Instance;
        var path = state.WorkingDirectoryPath;
        
        if (settings.Force ?? false)
        {
            var nodes = (await state.Client.GetNodesAsync()).ToList();
            var currentNode = nodes.Single(n=> n.Id == state.WorkingDirectoryNode);
            path = Nodes.UnravelPathToRoot(currentNode, nodes);
            state.WorkingDirectoryPath = path;
        }

        MarkupLine(path);
        return 0;
    }
}