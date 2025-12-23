using AccessControl.Contracts.Entities;

namespace AccessControl.Contracts.Extensions;

public static class ResourceExtensions
{
    public static Permissions GetEffectivePermissions(this Resource resource, string[] roleNames)
    {
        var effective = Permissions.None;

        if (resource == null || roleNames == null || roleNames.Length == 0)
        {
            return effective;
        }

        var dict = resource.RolePermissions.ToDictionary(fkr => fkr.RoleName);

        foreach (var roleName in roleNames)
        {
            if (dict.TryGetValue(roleName, out var fkRole))
            {
                effective |= fkRole.Permissions;
            }
        }

        return effective;
    }
}
