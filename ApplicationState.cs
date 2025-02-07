using CG.Web.MegaApiClient;

namespace Micro;

public class ApplicationState
{
    private ApplicationState()
    {
    }

    private static readonly Lazy<ApplicationState> Lazy = new(() => new ApplicationState());
    
    public static ApplicationState Instance => Lazy.Value;

    public MegaApiClient Client { get; set; } = new();

    public string WorkingDirectoryNode { get; set; } = string.Empty;

    public string WorkingDirectoryPath { get; set; } = "/";
}