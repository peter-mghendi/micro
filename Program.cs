using System.Text.Json;
using CG.Web.MegaApiClient;
using Micro;
using Micro.Models;
using static System.Environment;
using static System.IO.Directory;
using static System.IO.Path;
using static System.Net.HttpStatusCode;
using static CG.Web.MegaApiClient.ApiResultCode;
using static Microsoft.CodeAnalysis.CommandLineParser;

var root = Combine(GetFolderPath(SpecialFolder.ApplicationData), "micro");
CreateDirectory(root);
var configuration = await ReadConfiguration() ?? new(
    Username: Ask<string>("[yellow]Enter your mega.co.nz username:[/]"),
    Password: Prompt(new TextPrompt<string>("[yellow]Enter your mega.co.nz password:[/]").Secret())
);

var state = ApplicationState.Instance;
try
{
    await state.Client.LoginAsync(configuration.Username, configuration.Password);
    if (configuration.Name is null)
    {
        configuration = configuration with { Name = Ask<string>("[yellow]What should we call you?[/]") };
    }

    await using var stream = File.Create(Combine(root, "configuration.json"));
    await JsonSerializer.SerializeAsync(stream, configuration);
    var nodes = await state.Client.GetNodesAsync();
    state.WorkingDirectoryNode = nodes.Single(x => x.Type == NodeType.Root).Id;
    Clear();
}
catch (ApiException exception) when (exception.ApiResultCode is BadArguments)
{
    MarkupLine($"[red][[FAIL]]: Invalid credentials.[/]");
    return;
}
catch (HttpRequestException exception) when (exception.StatusCode is PaymentRequired)
{
    MarkupLine($"[red][[FAIL]]: Invalid credentials.[/]");
    return;
}

MarkupLine($"[yellow]Welcome, [blue]{configuration.Name}[/].\nLogged in as [blue]{configuration.Username}[/].[/]\n");
MarkupLine("[yellow]Type \"help\" for a list of commands.\nType \"exit\" or \"exit --logout\" to exit.[/]\n");

while (true)
{
    var command = Ask<string>($"[yellow]μ:{state.WorkingDirectoryPath}>[/]");
    var arguments = SplitCommandLineIntoArguments(command, false).ToArray();
    if (arguments is ["exit"] or ["exit", "--logout"])
    {
        if (arguments is [_, "--logout"]) File.Delete(Combine(root, "configuration.json"));
        break;
    }

    if (arguments is ["help" or "version"]) arguments = [$"--{arguments[0]}"];
    AppBuilder.Build().Run(arguments);
}

async Task<Configuration?> ReadConfiguration()
{
    try
    {
        await using var stream = File.OpenRead(Combine(root, "configuration.json"));
        return await JsonSerializer.DeserializeAsync<Configuration>(stream);
    }
    catch (Exception ex) when (ex is FileNotFoundException or JsonException)
    {
        return null;
    }
}
