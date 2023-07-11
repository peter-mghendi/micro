using System.Text.Json;
using CG.Web.MegaApiClient;
using Micro.Commands.Utilities;
using Spectre.Console.Cli;
using static CG.Web.MegaApiClient.NodeType;

namespace Micro.Commands;

public class UsernameCommand : AsyncCommand<UsernameCommand.Settings>
{
    public class Settings : CommandSettings
    {
    }

    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
    {
        var text = await System.IO.File.ReadAllTextAsync("./config.json");
        var configuration = JsonSerializer.Deserialize<Configuration>(text) ?? new Configuration();
        AnsiConsole.WriteLine(configuration.Username!);
        return 0;
    }
}