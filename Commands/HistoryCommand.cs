using Micro.Services;
using Spectre.Console.Cli;

namespace Micro.Commands;

public class HistoryCommand : AsyncCommand<HistoryCommand.Settings>
{
    public class Settings : CommandSettings
    {
        [CommandOption("-n|--number <number>")]
        public int? Number { get; set; }
    }

    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
    {
        var history = await History.ReadAsync(settings.Number);
        var width = history.Any() ? history.Max(pair => pair.Key).ToString().Length : 0;
        history.Select(pair => $"{pair.Key.ToString().PadLeft(width, '0')} {pair.Value}").ToList().ForEach(MarkupLine);
        return 0;
    }
}