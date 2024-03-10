using Bogus;
using wg.modules.companies.domain.Entities;

namespace wg.sharedForTests.Factories.Companies;

public static class ProjectFactory
{
    public static Project Get(bool withPlannedStart, bool withPlannedFinish)
    {
        var projectFaker = new Faker<Project>()
            .CustomInstantiator(f => Project.Create(
                Guid.NewGuid(),
                f.Lorem.Word(),
                f.Lorem.Sentence(),
                withPlannedStart ? f.Date.Future(0) : null,
                withPlannedFinish ? f.Date.Future(1) : null));
        return projectFaker.Generate(1).Single();
    }
}