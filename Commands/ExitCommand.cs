using System.ComponentModel;
using Spectre.Console.Cli;

namespace Micro.Commands;

public class ExitCommand : Command<ExitCommand.Settings>
{
    public class Settings : CommandSettings
    {
        [CommandOption("--logout")]
        [Description("Also clear locally stored credentials. You will be required to log in again next time.")]
        public bool? Logout { get; set; }
    }

    public override int Execute(CommandContext context, Settings settings)
    {
        if (settings.Logout is true) File.Delete(Micro.Utilities.Configuration.MicroConfiguration);
        return 130;
    }
}