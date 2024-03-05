using Microsoft.Extensions.DependencyInjection;

namespace wg.shared.tests.shared.Integration;

public abstract class BaseTestsController : IDisposable
{
    protected HttpClient HttpClient { get; }
    
    public BaseTestsController()
    {
        var app = new TestApp(ConfigureServices);
        HttpClient = app.HttpClient;
    }

    protected virtual void ConfigureServices(IServiceCollection services)
    {
        
    }
    
    public void Dispose()
    {
    }
}