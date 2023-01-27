namespace Prism;

internal class PrismInitializationService : Microsoft.Maui.Hosting.IMauiInitializeService
{
    /// <summary>
    /// Initializes the modules.
    /// </summary>
    public void Initialize(IServiceProvider services)
    {
        var builder = services.GetRequiredService<PrismAppBuilder>();
        builder.OnInitialized();
    }
}
