using System.Net.Http.Headers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using wg.shared.abstractions.Auth;
using wg.shared.infrastructure.Auth;
using wg.shared.infrastructure.Auth.Configuration.Models;
using wg.sharedForTests.Mocks;

namespace wg.sharedForTests.Integration;

public abstract class BaseTestsController : IDisposable
{
    protected HttpClient HttpClient { get; }
    private readonly IAuthenticator _authenticator;
    
    public BaseTestsController()
    {
        var app = new TestApp(ConfigureServices);
        HttpClient = app.HttpClient;
        var clock = TestsClock.Create();
        var optionsProvider = new OptionsProvider();
        var jwtOptions = optionsProvider.Get<JwtOptions>("Jwt");
        _authenticator = new JwtAuthenticator(clock, Options.Create(jwtOptions));
    }

    protected virtual void ConfigureServices(IServiceCollection services)
    {
        
    }

    protected virtual void Authorize(Guid userId, string role)
    {
        var token = _authenticator.CreateToken(userId.ToString(), role);
        HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer",token.Token);
    }
    
    public virtual void Dispose()
    {
    }
}