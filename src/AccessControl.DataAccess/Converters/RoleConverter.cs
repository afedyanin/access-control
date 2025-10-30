using AccessControl.Contracts.Entities;
using AccessControl.DataAccess.Dbos;

namespace AccessControl.DataAccess.Converters;

internal static class RoleConverter
{
    public static Role[] ToEntity(this IEnumerable<RoleDbo> roles)
        => [.. roles.Select(u => u.ToEntity())];

    public static Role ToEntity(this RoleDbo role)
        => new()
        {
            Name = role.Name,
            Description = role.Description,
        };
}
