namespace AccessControl.Application.Model;

internal class PermissionMatrix
{
    // First key  - feature key
    // Second key - role name
    private readonly Dictionary<string, Dictionary<string, Permissions>> _permissions = [];

    public PermissionMatrix(PermissionsEntry[] permissionEntries)
    {
        _permissions = LoadPermissions(permissionEntries);
    }

    public Permissions GetActualPermissions(string featureKey, string[] userRoles)
    {
        var actualPermissions = Permissions.None;

        if (!_permissions.TryGetValue(featureKey, out var fkPermissions))
        {
            return actualPermissions;
        }

        foreach (var role in userRoles)
        {
            if (!fkPermissions.TryGetValue(role, out var rolePermissions))
            {
                continue;
            }

            actualPermissions |= rolePermissions;
        }

        return actualPermissions;
    }

    private static Dictionary<string, Dictionary<string, Permissions>> LoadPermissions(PermissionsEntry[] permissionEntries)
    {
        var res = new Dictionary<string, Dictionary<string, Permissions>>();

        foreach (var entry in permissionEntries)
        {
            if (!res.TryGetValue(entry.FeatureKey.Name, out var fkPermissionsDict))
            {
                fkPermissionsDict = [];
                res.Add(entry.FeatureKey.Name, fkPermissionsDict);
            }

            fkPermissionsDict[entry.Role.Name] = entry.Permissions;
        }

        return res;
    }
}
