using Bogus;
using Microsoft.AspNetCore.Builder;
using wg.modules.companies.domain.Entities;

namespace wg.sharedForTests.Factories.Companies;

public static class ProjectFactory
{
    public static Project GetInCompany(Company company, bool withPlannedStart, bool withPlannedFinish)
    {
        var projectFaker = new Faker<Project>()
            .CustomInstantiator(f => Project.Create(
                Guid.NewGuid(),
                f.Lorem.Word(),
                f.Lorem.Sentence(),
                withPlannedStart ? f.Date.Future(0) : null,
                withPlannedFinish ? f.Date.Future(1) : null));
        var project = projectFaker.Generate(1).Single();
        company.AddProject(project.Id, project.Title, project.Description,
            project.PlannedStart, project.PlannedFinish);
        return company.Projects.Single(x => x.Id == project.Id);
    }
}