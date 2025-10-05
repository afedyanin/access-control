using AccessControl.Contracts;
using AccessControl.Model;

namespace AccessControl.WebApi.Converters;
internal static class ResourceConverter
{
    public static ResourceDto[] ToDto(this IEnumerable<Resource> resources)
        => resources.Select(fk => fk.ToDto()).ToArray();

    public static ResourceDto ToDto(this Resource resource)
        => new ResourceDto
        {
            Id = resource.Id,
            Name = resource.Name,
            RolePermissions = resource.ResourceRoles.ToDto()
        };

    private static RolePermissions[] ToDto(this IEnumerable<ResourceRole> rRoles)
        => rRoles.Select(fkr => fkr.ToDto()).ToArray();

    private static RolePermissions ToDto(this ResourceRole rRole)
        => new RolePermissions
        {
            RoleName = rRole.RoleName,
            Permissions = rRole.Permissions,
        };

}
