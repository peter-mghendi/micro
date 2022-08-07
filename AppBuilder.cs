using Micro.Commands;
using Spectre.Console.Cli;

namespace Micro;

public static class AppBuilder
{
    public static CommandApp Build()
    {
        var app = new CommandApp();

        app.Configure(config =>
        {
            config.AddCommand<PrintWorkingDirectoryCommand>("pwd");
        });

        return app;
    }
}