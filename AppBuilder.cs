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
            config.SetApplicationVersion("1.0.0");
            // config.PropagateExceptions();
            
            // TODO: cp, find, rename, chat, editor
            config.AddCommand<ChangeDirectoryCommand>("cd");
            config.AddCommand<ClearCommand>("clear");
            config.AddCommand<GetCommand>("get");
            config.AddCommand<ListContentsCommand>("ls");
            config.AddCommand<MakeDirectoryCommand>("mkdir");
            config.AddCommand<MoveCommand>("mv");
            config.AddCommand<PutCommand>("put");
            config.AddCommand<PrintWorkingDirectoryCommand>("pwd");
            config.AddCommand<RemoveCommand>("rm");
            config.AddCommand<StatusCommand>("stat");
            config.AddCommand<TouchCommand>("touch");
            config.AddCommand<UsernameCommand>("whoami");
        });

        return app;
    }
}