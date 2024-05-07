using Bogus;
using Microsoft.AspNetCore.Builder;
using wg.modules.companies.domain.Entities;

namespace wg.tests.shared.Factories.Companies;

internal static class ProjectFactory
{
    internal static Project GetInCompany(Company company, bool withPlannedStart, bool withPlannedFinish)
        => GetInCompany(company, withPlannedStart, withPlannedFinish, 1).Single();
    
    private static IEnumerable<Project> GetInCompany(Company company, bool withPlannedStart, bool withPlannedFinish, int count)
    {
        var projects = Get(count, withPlannedStart, withPlannedFinish);
        foreach (var project in projects)
        {
            company.AddProject(project.Id, project.Title, project.Description,
                project.PlannedStart, project.PlannedFinish);
        }

        return company.Projects;
    }

    private static List<Project> Get(int count, bool withPlannedStart, bool withPlannedFinish)
        => GetFaker(withPlannedStart, withPlannedFinish).Generate(count);
    
    private static Faker<Project> GetFaker(bool withPlannedStart, bool withPlannedFinish)
        => new Faker<Project>()
            .CustomInstantiator(f => Project.Create(
                Guid.NewGuid(),
                f.Lorem.Word(),
                f.Lorem.Sentence(),
                withPlannedStart ? f.Date.Future(0) : null,
                withPlannedFinish ? f.Date.Future(1) : null));
}