using Micro.Commands;
using Micro.Commands.Utilities;
using Micro.Utilities;
using Spectre.Console.Cli;

namespace Micro;

public static class AppBuilder
{
    public static CommandApp Build()
    {
        var app = new CommandApp();

        app.Configure(config =>
        {
            config.SetApplicationVersion(Configuration.Version);
            // config.PropagateExceptions();
            
            config.AddDelegate("help", _ => Help.Delegate(app));
            
            // TODO: cp, find, rename, chat, editor
            config.AddCommand<ChangeDirectoryCommand>("cd");
            config.AddCommand<ClearCommand>("clear");
            config.AddCommand<ExitCommand>("exit");
            config.AddCommand<GetCommand>("get");
            config.AddCommand<HistoryCommand>("history");
            config.AddCommand<ListContentsCommand>("ls");
            config.AddCommand<MakeDirectoryCommand>("mkdir");
            config.AddCommand<MoveCommand>("mv");
            config.AddCommand<PutCommand>("put");
            config.AddCommand<PrintWorkingDirectoryCommand>("pwd");
            config.AddCommand<RemoveCommand>("rm");
            config.AddCommand<StatusCommand>("stat");
            config.AddCommand<TouchCommand>("touch");
            config.AddCommand<VersionCommand>("version");
            config.AddCommand<UsernameCommand>("whoami");
        });

        return app;
    }
}