using AccessControl.Contracts;

namespace AccessControl.Model;

public class FeatureKey
{
    public required string Name { get; set; }

    public List<Role> Roles { get; set; } = [];

    public List<FeatureKeyRole> FeatureKeyRoles { get; set; } = [];

    public Permissions GetEffectivePermissions(string[] roleNames)
    {
        var dict = FeatureKeyRoles.ToDictionary(fkr => fkr.RoleName);
        var effective = Permissions.None;

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
