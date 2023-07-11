using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using CG.Web.MegaApiClient;
using Micro.Commands.Utilities;
using Spectre.Console.Cli;
using static CG.Web.MegaApiClient.NodeType;

namespace Micro.Commands;

public class ClearCommand : Command<ClearCommand.Settings>
{
    public class Settings : CommandSettings
    {
    }

    public override int Execute(CommandContext context, Settings settings)
    {
        AnsiConsole.Clear();
        return 0;
    }
}