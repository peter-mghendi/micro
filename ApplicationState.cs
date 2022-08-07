namespace Micro;

public class ApplicationState
{
    private ApplicationState()    
    {    
    }    
    private static readonly Lazy<ApplicationState> Lazy = new(() => new ApplicationState());    
    public static ApplicationState Instance => Lazy.Value;

    public string WorkingDirectory { get; set; } = "/";
}