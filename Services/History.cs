using static System.Environment;
using static System.Environment.SpecialFolder;
using static System.IO.File;
using static System.IO.Path;

namespace Micro.Services;

public static class History
{
    private static readonly string HistoryPath = Combine(GetFolderPath(ApplicationData), "micro", ".micro_history");

    public static async Task<Dictionary<int, string>> ReadAsync(int? n = null, CancellationToken cancellation = default)
    {
        await AppendAllTextAsync(HistoryPath, string.Empty, cancellation);
        var history = (await ReadAllLinesAsync(HistoryPath, cancellation))
            .Select((entry, index) => (Index: index, Entry: entry))
            .ToDictionary(pair => pair.Index, x => x.Entry);

        return n.HasValue ? history.TakeLast(n.Value).ToDictionary(pair => pair.Key, pair => pair.Value) : history;
    }

    public static async Task WriteAsync(string command, CancellationToken cancellation = default) =>
        await AppendAllTextAsync(HistoryPath, $"{command}{NewLine}", cancellation);
}