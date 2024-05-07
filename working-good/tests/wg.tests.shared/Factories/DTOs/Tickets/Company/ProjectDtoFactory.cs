using Bogus;
using wg.modules.companies.domain.Entities;
using wg.modules.tickets.application.Clients.Companies.DTO;

namespace wg.tests.shared.Factories.DTOs.Tickets.Company;

internal static class ProjectDtoFactory
{
    internal static ProjectDto Get(bool plannedStart = true, bool plannedFinish = true)
        => Get(1, plannedStart, plannedFinish).Single();
    
    internal static List<ProjectDto> Get(int count, bool plannedStart = true, bool plannedFinish = true)
        => GetFaker(plannedStart, plannedFinish).Generate(count);
    
    internal static Faker<ProjectDto> GetFaker(bool plannedStart = true, bool plannedFinish = true)
        => new Faker<ProjectDto>()
            .RuleFor(f => f.Id, Guid.NewGuid())
            .RuleFor(f => f.Description, v => v.Lorem.Sentence())
            .RuleFor(f => f.Title, v => v.Lorem.Sentence(null, 5))
            .RuleFor(f => f.PlannedStart, v => plannedStart ? v.Date.Past() : null)
            .RuleFor(f => f.PlannedFinish, v => plannedFinish ? v.Date.Future() : null);
}