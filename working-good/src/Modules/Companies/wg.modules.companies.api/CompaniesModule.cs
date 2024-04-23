using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using wg.modules.companies.application.CQRS.Companies.Queries;
using wg.modules.companies.application.CQRS.Employees.Queries;
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
            .Subscribe<GetCompanyByEmployeeIdQuery, CompanyDto>("companies/get/by-employee-id",
                (query, sp) => sp.GetRequiredService<IQueryDispatcher>().SendAsync(query, default))
            .Subscribe<GetActiveEmployeeByEmailQuery, EmployeeDto>("companies/employee/active/get/by-email",
                (query, sp) => sp.GetRequiredService<IQueryDispatcher>().SendAsync(query, default))
            .Subscribe<GetEmployeeByIdQuery, EmployeeDto>("companies/employee/get/by-id",
                (query, sp) => sp.GetRequiredService<IQueryDispatcher>().SendAsync(query, default));
    }
}