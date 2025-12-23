using AccessControl.Contracts.Entities;
using AccessControl.DataAccess.Dbos;

namespace AccessControl.DataAccess.Converters;

internal static class ResourceConverter
{
    public static Resource[] ToEntity(this IEnumerable<ResourceDbo> resources)
        => [.. resources.Select(fk => fk.ToEntity())];

    public static Resource ToEntity(this ResourceDbo resource)
        => new()
        {
            Id = resource.Id,
            Name = resource.Name,
            RolePermissions = resource.ResourceRoles.ToEntity()
        };

    private static RolePermissions[] ToEntity(this IEnumerable<ResourceRoleDbo> rRoles)
        => [.. rRoles.Select(fkr => fkr.ToEntity())];

    private static RolePermissions ToEntity(this ResourceRoleDbo rRole)
        => new()
        {
            RoleName = rRole.RoleName,
            Permissions = rRole.Permissions,
        };
}
