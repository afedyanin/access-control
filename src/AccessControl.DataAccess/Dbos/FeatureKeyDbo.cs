using AccessControl.Contracts.Entities;

namespace AccessControl.DataAccess.Dbos;

internal class FeatureKeyDbo
{
    public required string Name { get; set; }

    public List<RoleDbo> Roles { get; set; } = [];

    public List<FeatureKeyRoleDbo> FeatureKeyRoles { get; set; } = [];

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
