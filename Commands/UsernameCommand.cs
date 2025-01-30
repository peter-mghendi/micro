using System.Text.Json;
using Micro.Models;
using Spectre.Console.Cli;
using static System.IO.File;

namespace Micro.Commands;

public class UsernameCommand : AsyncCommand<UsernameCommand.Settings>
{
    public class Settings : CommandSettings;

    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
    {
        var text = await ReadAllTextAsync("./config.json");
        var configuration = JsonSerializer.Deserialize<Configuration>(text);
        WriteLine(configuration!.Username!);
        return 0;
    }
}