using Bogus;
using wg.modules.companies.domain.Entities;
using wg.modules.tickets.application.Clients.Companies.DTO;

namespace wg.tests.shared.Factories.DTOs.Tickets.Company;

internal static class ProjectDtoFactory
{
    internal static List<ProjectDto> Get(bool plannedStart = true, bool plannedFinish = true, int count = 1)
        => GetFaker(plannedStart, plannedFinish).Generate(count);
    
    internal static Faker<ProjectDto> GetFaker(bool plannedStart = true, bool plannedFinish = true)
        => new Faker<ProjectDto>()
            .RuleFor(f => f.Id, Guid.NewGuid())
            .RuleFor(f => f.Description, v => v.Lorem.Sentence())
            .RuleFor(f => f.Title, v => v.Lorem.Sentence(null, 5))
            .RuleFor(f => f.PlannedStart, v => plannedStart ? v.Date.Past() : null)
            .RuleFor(f => f.PlannedFinish, v => plannedFinish ? v.Date.Future() : null);
}