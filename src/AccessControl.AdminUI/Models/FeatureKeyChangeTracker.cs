using AccessControl.Contracts.Entities;

namespace AccessControl.AdminUI.Models;

public class FeatureKeyChangeTracker
{
    private readonly FeatureKey[] _allKeys = [];

    public FeatureKeyChangeTracker(FeatureKey[] allKeys)
    {
        _allKeys = allKeys;
    }

    private static string GetFkRoleKey(string fkName, string roleName)
        => $"{fkName}$${roleName}";


    private static IEnumerable<PermissionItem> Flattern(FeatureKey[] allKeys)
        => allKeys
        .SelectMany(k => k.RolePermissions,
        (key, permissions) =>
            new PermissionItem
            {
                KeyName = key.Name,
                RoleName = permissions.RoleName,
                Permissions = permissions.Permissions
            });

}

public record class PermissionItem
{
    public required string KeyName { get; set; }
    public required string RoleName { get; set; }
    public Permissions Permissions { get; set; }
}
