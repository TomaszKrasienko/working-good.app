using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using wg.modules.owner.application.CQRS.Groups.Queries;
using wg.modules.owner.application.CQRS.Owners.Queries;
using wg.modules.owner.application.CQRS.Users.Queries;
using wg.modules.owner.application.DTOs;
using wg.modules.owner.infrastructure.Configuration;
using wg.shared.abstractions.CQRS.Queries;
using wg.shared.abstractions.Modules;
using wg.shared.infrastructure.Modules.Configuration;

namespace wg.modules.owner.api;

internal sealed class OwnerModule : IModule
{
    internal const string RoutePath = "owner-module";
    public string Name { get; } = "Owner";
    
    public void Register(IServiceCollection services)
    {
        services.AddInfrastructure();
    }

    public void Use(WebApplication app)
    {
        app
            .UseModuleRequest()
            .Subscribe<GetUserByIdQuery, UserDto>("owner/users/get",
                (query, sp) => sp.GetRequiredService<IQueryDispatcher>().SendAsync(new GetUserByIdQuery(query.Id), default))
            .Subscribe<GetOwnerQuery, OwnerDto>("owner/get",
                (query, sp) => sp.GetRequiredService<IQueryDispatcher>().SendAsync(query, default))
            .Subscribe<GetActiveUserByIdQuery, UserDto>("owner/users/active/get",
            (query, sp) => sp.GetRequiredService<IQueryDispatcher>().SendAsync(query, default))
            .Subscribe<IsMembershipExistsQuery, IsExistsDto>("owner/group/is-membership-exists/get",
                (query, sp) => sp.GetRequiredService<IQueryDispatcher>().SendAsync(query, default))
            .Subscribe<IsActiveUserExistsQuery, IsExistsDto>("owner/users/is-active-exists/get",
                (query, sp) => sp.GetRequiredService<IQueryDispatcher>().SendAsync(query, default));
    }
}