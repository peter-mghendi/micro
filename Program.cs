using System.Text.Json;
using CG.Web.MegaApiClient;
using Micro;
using Micro.Services;
using Micro.Utilities;
using static System.IO.Directory;
using static System.Net.HttpStatusCode;
using static CG.Web.MegaApiClient.ApiResultCode;
using static Microsoft.CodeAnalysis.CommandLineParser;


CreateDirectory(Configuration.Root);
var configuration = await Configuration.ReadConfiguration() ?? new(
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

    await using var stream = File.Create(Configuration.MicroConfiguration);
    await JsonSerializer.SerializeAsync(stream, configuration);
    var nodes = await state.Client.GetNodesAsync();
    state.WorkingDirectoryNode = nodes.Single(x => x.Type == NodeType.Root).Id;
    Clear();
}
catch (Exception ex) when (ex is ApiException { ApiResultCode: BadArguments }
                               or HttpRequestException { StatusCode: PaymentRequired })
{
    MarkupLine($"[red][[FAIL]]: Invalid credentials.[/]");
    return;
}

MarkupLine($"[yellow]Welcome, [blue]{configuration.Name}[/].\nLogged in as [blue]{configuration.Username}[/].[/]\n");
MarkupLine("[yellow]Type \"help\" for a list of commands.\nType \"exit\" or \"exit --logout\" to exit.[/]\n");

while (true)
{
    // TODO: History: https://github.com/spectreconsole/spectre.console/issues/158
    var command = Ask<string>($"[yellow]μ:{state.WorkingDirectoryPath}>[/]");
    var arguments = SplitCommandLineIntoArguments(command, false).ToArray();

    if (AppBuilder.Build().Run(arguments) is 130) break;
    if (arguments is not ["history", ..] or ["version"]) await History.WriteAsync(command);
}