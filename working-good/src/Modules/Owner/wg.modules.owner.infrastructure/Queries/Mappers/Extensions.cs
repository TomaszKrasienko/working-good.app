using System.Collections.Immutable;
using wg.modules.owner.application.DTOs;
using wg.modules.owner.domain.Entities;

namespace wg.modules.owner.infrastructure.Queries.Mappers;

internal static class Extensions
{
    internal static OwnerDto AsDto(this Owner owner)
        => new OwnerDto
        {
            Id = owner.Id,
            Name = owner.Name,
            Users = owner.Users?.Select(x => x.AsDto()).ToImmutableList(),
            Groups = owner.Groups?.Select(g => g.AsDto()).ToImmutableList()
        };

    internal static UserDto AsDto(this User user)
        => new UserDto()
        {
            Id = user.Id,
            Email = user.Email,
            FirstName = user.FullName.FirstName,
            LastName = user.FullName.LastName,
            Role = user.Role,
            State = user.State
        };

    internal static GroupDto AsDto(this Group group)
        => new GroupDto()
        {
            Id = group.Id,
            Title = group.Title,
            Users = group.Users?.Select(x => x.Id.Value).ToImmutableList()
        };
}