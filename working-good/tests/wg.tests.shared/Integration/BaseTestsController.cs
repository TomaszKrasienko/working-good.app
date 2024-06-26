using System.Net.Http.Headers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using wg.modules.activities.infrastructure.DAL;
using wg.modules.companies.infrastructure.DAL;
using wg.modules.notifications.core.Services.Abstractions;
using wg.modules.owner.infrastructure.DAL;
using wg.modules.tickets.infrastructure.DAL;
using wg.modules.wiki.infrastructure.DAL;
using wg.shared.abstractions.Pagination;
using wg.shared.infrastructure.Auth;
using wg.shared.infrastructure.Auth.Configuration.Models;
using wg.shared.infrastructure.Serialization;
using wg.tests.shared.Db;
using wg.tests.shared.Mocks;

namespace wg.tests.shared.Integration;

public abstract class BaseTestsController : IDisposable
{
    protected readonly HttpClient HttpClient;
    internal readonly TestAppDb TestAppDb;
    internal readonly CompaniesDbContext CompaniesDbContext;
    internal readonly OwnerDbContext OwnerDbContext;
    internal readonly TicketsDbContext TicketsDbContext;
    internal readonly ActivitiesDbContext ActivitiesDbContext;
    internal readonly WikiDbContext WikiDbContext;

    protected BaseTestsController()
    {
        var app = new TestApp(ConfigureServices);
        HttpClient = app.HttpClient;
        TestAppDb = new TestAppDb();
        CompaniesDbContext = TestAppDb.CompaniesDbContext;
        OwnerDbContext = TestAppDb.OwnerDbContext;
        TicketsDbContext = TestAppDb.TicketsDbContext;
        ActivitiesDbContext = TestAppDb.ActivitiesDbContext;
        WikiDbContext = TestAppDb.WikiDbContext;
    }

    protected virtual void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<IEmailPublisher, FakeEmailPublisher>();
    }

    protected virtual void Authorize(Guid userId, string role)
    {
        var clock = TestsClock.Create();
        var optionsProvider = new OptionsProvider();
        var jwtOptions = optionsProvider.Get<JwtOptions>("Jwt");
        var authenticator = new JwtAuthenticator(clock, Options.Create(jwtOptions));
        var token = authenticator.CreateToken(userId.ToString(), role);
        HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer",token.Token);
    }

    protected virtual Guid? GetResourceIdFromHeader(HttpResponseMessage httpResponseMessage) 
    {
        if (httpResponseMessage is null)
        {
            throw new InvalidOperationException("Http response message is null");
        }

        if (!httpResponseMessage.Headers.TryGetValues("x-resource-id", out var value))
        {
            return null;
        }

        var stringId = value.Single();
        if (!Guid.TryParse(stringId, out var id))
        {
            throw new InvalidOperationException("Resource id is not GUID type");
        }

        return id;
    }

    protected virtual MetaDataDto GetPaginationMetaDataFromHeader(HttpResponseMessage httpResponseMessage)
    {
        if (httpResponseMessage is null)
        {
            throw new InvalidOperationException("Http response message is null");
        }

        if (!httpResponseMessage.Headers.TryGetValues("x-pagination", out var value))
        {
            return null;
        }
        
        var metaJson = value.Single();
        return metaJson.ToObject<MetaDataDto>();
    }
    
    public virtual void Dispose()
    {
        TestAppDb.Dispose();
    }
}