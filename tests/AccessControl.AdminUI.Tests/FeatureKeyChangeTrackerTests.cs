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

    [TestCase("FK_new", "role01", Permissions.Read | Permissions.Write, true)]
    [TestCase("fk02", "role02", Permissions.None, false)]
    public void CanAddNewKey(string featureKey, string role, Permissions permission, bool expected)
    {
        var tracker = new FeatureKeyChangeTracker(_allKeys);

        var res = tracker.TryAdd(featureKey, role, permission);
        Assert.That(res, Is.EqualTo(expected));
    }

    [TestCase("FK_new", "role01", Permissions.Read | Permissions.Write, false)]
    [TestCase("fk02", "role02", Permissions.None, true)]
    public void CanUpdateKey(string featureKey, string role, Permissions permission, bool expected)
    {
        var tracker = new FeatureKeyChangeTracker(_allKeys);

        var res = tracker.TryUpdate(featureKey, role, permission);
        Assert.That(res, Is.EqualTo(expected));
    }

    // TODO: Full unit test public methods for FeatureKeyChangeTracker

}
