using System.Reflection;
using Spectre.Console.Cli;
using Spectre.Console.Cli.Help;

namespace Micro.Utilities;

public static class Help
{
    public static int Delegate(ICommandApp app)
    {
        Render(app);
        return 0;
    }
    
    // TODO: https://github.com/spectreconsole/spectre.console/issues/1555
    public static void Render(ICommandApp app)
    {
        // var _app = app.GetType().InvokeMember("_app",
        //     BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetField, null, app, []);
        var configurator = app!.GetType().InvokeMember("_configurator",
            BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetField, null, app, []);
        var settings = configurator!.GetType().InvokeMember("Settings",
            BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty, null, configurator, []);

        var cmdModelBuilderType = Type.GetType("Spectre.Console.Cli.CommandModelBuilder,Spectre.Console.Cli");
        var cmdModel = cmdModelBuilderType!.InvokeMember("Build",
            BindingFlags.Public | BindingFlags.Static | BindingFlags.InvokeMethod, null, null,
            [configurator]);

        var defCommand = cmdModel!.GetType().InvokeMember("DefaultCommand",
            BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty, null, cmdModel, []);

        var helpProvider = new HelpProvider((settings as ICommandAppSettings)!);
        var helpLines = helpProvider.Write((cmdModel as ICommandModel)!, defCommand as ICommandInfo);

        foreach (var line in helpLines)
        {
            Write(line);
        }
    }
}