using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using wg.modules.companies.application.CQRS.Companies.Queries;
using wg.modules.companies.application.CQRS.Employees.Queries;
using wg.modules.companies.application.CQRS.Projects.Queries;
using wg.modules.companies.application.DTOs;
using wg.modules.companies.infrastructure.Configuration;
using wg.shared.abstractions.CQRS.Queries;
using wg.shared.abstractions.Modules;
using wg.shared.infrastructure.Modules.Configuration;

namespace wg.modules.companies.api;

internal sealed class CompaniesModule : IModule
{
    internal const string RoutePath = "companies-module";
    public string Name { get; } = "Companies";
    public void Register(IServiceCollection services)
    {
        services.AddInfrastructure();
    }

    public void Use(WebApplication app)
    {
        app
            .UseModuleRequest()
            .Subscribe<GetSlaTimeByEmployeeIdQuery, CompanySlaTimeDto>("companies/employee/sla-time/get",
                (query, sp) => sp.GetRequiredService<IQueryDispatcher>().SendAsync(query, default))
            .Subscribe<IsEmployeeExistsQuery, IsEmployeeExistsDto>("companies/employee/is-exists/get",
                (query, sp) => sp.GetRequiredService<IQueryDispatcher>().SendAsync(query, default))
            .Subscribe<IsProjectForEmployeeExistsQuery, IsProjectExistsDto>("companies/project/is-exists/get",
                (query, sp) => sp.GetRequiredService<IQueryDispatcher>().SendAsync(query, default))
            .Subscribe<GetEmployeeIdQuery, EmployeeIdDto>("companies/employee/id/get",
                (query, sp) =>
                {
                    var scope = sp.CreateScope();
                    return scope.ServiceProvider.GetRequiredService<IQueryDispatcher>().SendAsync(query, default);
                });
    }
}