using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using wg.modules.companies.application.CQRS.Companies.Queries;
using wg.modules.companies.application.CQRS.Employees.Queries;
using wg.modules.companies.application.CQRS.Projects.Queries;
using wg.modules.companies.application.DTOs;
using wg.modules.companies.domain.ValueObjects.Company;
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
            .Subscribe<IsActiveCompanyExistsQuery, IsExistsDto>("companies/is-exists/get",
                (query, sp) => sp.GetRequiredService<IQueryDispatcher>().SendAsync(query, default))
            .Subscribe<GetCompanyByEmployeeIdQuery, CompanyDto>("companies/get/by-employee-id",
                (query, sp) => sp.GetRequiredService<IQueryDispatcher>().SendAsync(query, default))
            .Subscribe<GetActiveEmployeeByEmailQuery, EmployeeDto>("companies/employee/active/get/by-email",
                (query, sp) => sp.GetRequiredService<IQueryDispatcher>().SendAsync(query, default))
            .Subscribe<GetActiveEmployeeByIdQuery, EmployeeDto>("companies/employee/active/get",
                (query, sp) => sp.GetRequiredService<IQueryDispatcher>().SendAsync(query, default))
            .Subscribe<IsActiveEmployeeExistsQuery, IsExistsDto>("companies/employees/is-active-exists/get",
                (query, sp) => sp.GetRequiredService<IQueryDispatcher>().SendAsync(query, default))
            .Subscribe<IsProjectInCompanyQuery, IsExistsDto>("companies/projects/is-project-for-company/get",
                (query, sp) => sp.GetRequiredService<IQueryDispatcher>().SendAsync(query, default))
            .Subscribe<GetSlaTimeByEmployeeIdQuery, SlaTimeDto>("companies/sla-time/by-employee/get",
                (query, sp) => sp.GetRequiredService<IQueryDispatcher>().SendAsync(query, default))
            .Subscribe<IsProjectActiveQuery, IsExistsDto>("companies/projects/is-active-exists/get",
                (query, sp) => sp.GetRequiredService<IQueryDispatcher>().SendAsync(query, default));
    }
}