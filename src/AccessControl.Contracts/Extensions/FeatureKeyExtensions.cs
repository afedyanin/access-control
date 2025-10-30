using AccessControl.Contracts.Entities;

namespace AccessControl.Contracts.Extensions;
public static class FeatureKeyExtensions
{
    public static Permissions GetEffectivePermissions(this FeatureKey fetaureKey, string[] roleNames)
    {
        var effective = Permissions.None;

        if (fetaureKey == null || roleNames == null || roleNames.Length == 0)
        {
            return effective;
        }

        var dict = fetaureKey.RolePermissions.ToDictionary(fkr => fkr.RoleName);

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
