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
            config.SetApplicationVersion("1.0.0 \"Arc\"");
            // config.PropagateExceptions();
            
            // TODO: upload, cp, mv, rename, chat, whoami, editor
            config.AddCommand<PrintWorkingDirectoryCommand>("pwd");
            config.AddCommand<ListContentsCommand>("ls");
            config.AddCommand<ChangeDirectoryCommand>("cd");
            config.AddCommand<GetCommand>("get");
            config.AddCommand<MakeDirectoryCommand>("mkdir");
            config.AddCommand<RemoveCommand>("rm");
            config.AddCommand<StatusCommand>("stat");
            config.AddCommand<MoveCommand>("mv");
        });

        return app;
    }
}