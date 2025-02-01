using System.Text.Json;
using static System.Environment;

namespace Micro.Utilities;

public static class Configuration
{
    public static string Version = "1.0.0";
    public static readonly string Root = Path.Combine(GetFolderPath(SpecialFolder.ApplicationData), "micro");
    public static readonly string MicroConfiguration = Path.Combine(Root, "configuration.json");
    
    public static async Task<Models.Configuration?> ReadConfiguration()
    {
        try
        {
            await using var stream = File.OpenRead(MicroConfiguration);
            return await JsonSerializer.DeserializeAsync<Models.Configuration>(stream);
        }
        catch (Exception ex) when (ex is FileNotFoundException or JsonException)
        {
            return null;
        }
    }
}