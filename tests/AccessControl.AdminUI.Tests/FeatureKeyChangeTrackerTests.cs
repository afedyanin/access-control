using AccessControl.AdminUI.Models;
using AccessControl.Contracts.Entities;

namespace AccessControl.AdminUI.Tests;

public class FeatureKeyChangeTrackerTests
{
    private static readonly FeatureKey[] _allKeys =
    [
        new FeatureKey
        {
            Name = "fk01",
            RolePermissions =
            [
                new RolePermissions
                {
                    RoleName = "role01",
                    Permissions= Permissions.Read | Permissions.Write,
                },
                new RolePermissions
                {
                    RoleName = "role02",
                    Permissions= Permissions.Read | Permissions.Delete,
                },
            ],
        },
        new FeatureKey
        {
            Name = "fk02",
            RolePermissions =
            [
                new RolePermissions
                {
                    RoleName = "role03",
                    Permissions= Permissions.Read | Permissions.Write,
                },
                new RolePermissions
                {
                    RoleName = "role02",
                    Permissions= Permissions.Read | Permissions.Execute,
                },
            ],
        },
    ];

    [Test]
    public void CanCreateFlatPermissions()
    {
        var res = _allKeys
            .SelectMany(k => k.RolePermissions,
            (key, permissions) =>
                new PermissionItem
                {
                    KeyName = key.Name,
                    RoleName = permissions.RoleName,
                    Permissions = permissions.Permissions
                });

        Console.WriteLine(string.Join("\n", res));
        Assert.Pass();
    }

    public void AnAddNewFeatureKey()
    {
        var keysDict = new Dictionary<string, Dictionary<string, Permissions>>();

        foreach (var key in _allKeys)
        {
            keysDict.TryGetValue(key.Name, out var rolesDict);

            rolesDict ??= [];

            foreach(var role in key.RolePermissions)
            {

            }
        }
    }

}
