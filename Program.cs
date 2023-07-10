using System.Text;
using System.Text.Json;
using CG.Web.MegaApiClient;
using Micro;

const string path = "./config.json";
var delay = TimeSpan.FromMilliseconds(1500);

Configuration configuration = new();

AnsiConsole.MarkupLine("[bold yellow]Hello there![/]");
await Task.Delay(delay: delay);

AnsiConsole.MarkupLine("[yellow]Let me just set up a few things![/]");
AnsiConsole.WriteLine();
await Task.Delay(delay: delay);

// Synchronous
await AnsiConsole.Status()
    .StartAsync("Initializing...", async ctx =>
    {
        Thread.Sleep(timeout: delay);
        AnsiConsole.MarkupLine("[blue][[INFO]]: Initialized.[/]");
        ctx.Status("Checking for config file...");


        Thread.Sleep(timeout: delay);
        var exists = File.Exists("./config.json");
        if (!exists)
        {
            AnsiConsole.MarkupLine("[yellow][[WARN]]: No config file found.[/]");
            ctx.Status("Cresting config...");

            Thread.Sleep(timeout: delay);
            File.Create("./config.json").Close();
            await File.WriteAllTextAsync(path, JsonSerializer.Serialize(configuration));

            AnsiConsole.MarkupLine("[blue][[INFO]]: Config file created.[/]");
            ctx.Status("Loading default config...");
        }
        else
        {
            AnsiConsole.MarkupLine("[blue][[INFO]]: Config file found.[/]");
            ctx.Status("Reading config...");

            Thread.Sleep(timeout: delay);
            var text = await File.ReadAllTextAsync("./config.json");
            configuration = JsonSerializer.Deserialize<Configuration>(text) ?? configuration;
        }

        Thread.Sleep(timeout: delay);
        AnsiConsole.MarkupLine("[blue][[INFO]]: Configuration loaded successfully.[/]");
    });

AnsiConsole.WriteLine();
if (!configuration.Configured)
{
    AnsiConsole.MarkupLine("[yellow]This is your first time running this program![/]");
    AnsiConsole.MarkupLine("[yellow]Let's get you set up![/]");
    AnsiConsole.WriteLine();

    var name = AnsiConsole.Ask<string>("What should I call you?");
    var username =
        AnsiConsole.Ask<string>("[yellow]Now, enter the email address you use to sign in on mega.co.nz:[/]");
    var password = AnsiConsole.Prompt<string>(
        new TextPrompt<string>("[yellow]Now, enter the password you use to sign in on mega.co.nz:[/]").Secret()
    );
    
    AnsiConsole.WriteLine();

    configuration = configuration with {Configured = true, Name = name, Username = username, Password = password};
    await File.WriteAllTextAsync(path, JsonSerializer.Serialize(configuration));
    AnsiConsole.MarkupLine("[blue][[INFO]]: Configuration saved successfully.[/]");
}
else
{
    AnsiConsole.MarkupLine($"[yellow]Welcome back, [blue]{configuration.Name!}[/].[/]");
    AnsiConsole.WriteLine();
}

AnsiConsole.MarkupLine("[blue][[INFO]]: Starting up...[/]");
AnsiConsole.MarkupLine("[blue][[INFO]]: Logging you in...[/]");

await ApplicationState.Instance.Client.LoginAsync(configuration.Username!, configuration.Password!);

// Set working directory to the the root of the cloud storage
var nodes = await ApplicationState.Instance.Client.GetNodesAsync();
ApplicationState.Instance.WorkingDirectoryNode = nodes.Single(x => x.Type == NodeType.Root).Id;

AnsiConsole.MarkupLine($"[blue][[INFO]]: Login successful.[/]");

AnsiConsole.Clear();
AnsiConsole.MarkupLine("[yellow]Welcome to Micro![/]");
AnsiConsole.MarkupLine($"[yellow]Logged in as [blue]{configuration.Username!}[/].[/]");
AnsiConsole.WriteLine();
AnsiConsole.MarkupLine("[yellow]Type \"help\" for a list of commands.[/]");
AnsiConsole.MarkupLine("[yellow]Type \"exit\" to exit.[/]");
AnsiConsole.WriteLine();

// Prompt
while (true)
{
    var command = AnsiConsole.Ask<string>($"[yellow]micro[[{ApplicationState.Instance.WorkingDirectoryPath}]]>[/]");
    if (command is "exit") break;
    
    // Hook into Spectre help command
    if (command is "help" or "version") command = $"--{command}"; 

    // try
    // {
         AppBuilder.Build().Run(ParseArgsFromString(command));
    // }
    // catch (Exception ex)
    // {
    //     AnsiConsole.WriteException(ex, ExceptionFormats.ShortenEverything);
    // }
    //
}

AnsiConsole.MarkupLine("[yellow]Goodbye![/]");

List<string> ParseArgsFromString(string input)
{
    var args = new List<string>();
    var current = new StringBuilder();
    var inQuote = false;
    foreach (var c in input)
    {
        switch (c)
        {
            case '"':
                inQuote = !inQuote;
                break;
            case ' ' when !inQuote:
                args.Add(current.ToString());
                current.Clear();
                break;
            default:
                current.Append(c);
                break;
        }
    }
    args.Add(current.ToString());
    return args;
}