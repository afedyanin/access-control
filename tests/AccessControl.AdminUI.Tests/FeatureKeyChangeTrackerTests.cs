using AccessControl.AdminUI.Models;
using AccessControl.Contracts.Entities;

namespace AccessControl.AdminUI.Tests;

public class FeatureKeyChangeTrackerTests
{
    private static readonly FeatureKey[] AllKeys =
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
    public void CanCreateChangeTracker()
    {
        var dict = FeatureKeyChangeTracker.CreateStructuredPermissions(AllKeys);
        Assert.Multiple(() =>
        {
            Assert.That(dict.ContainsKey("fk01"), Is.True);
            Assert.That(dict.ContainsKey("fk02"), Is.True);
            Assert.That(dict.ContainsKey("fk3"), Is.False);
        });

    }

    [TestCase("FK_new", "role01", Permissions.Read | Permissions.Write, true)]
    [TestCase("fk02", "role02", Permissions.None, false)]
    [TestCase("fk02", "role01", Permissions.None, true)]
    public void CanAddNewKey(string featureKey, string role, Permissions permission, bool expected)
    {
        var tracker = new FeatureKeyChangeTracker(AllKeys);

        var res = tracker.TryAdd(featureKey, role, permission);
        Assert.That(res, Is.EqualTo(expected));
    }

    [TestCase("FK_new", "role01", Permissions.Read | Permissions.Write, false)]
    [TestCase("fk02", "role02", Permissions.None, true)]
    [TestCase("fk02", "role01", Permissions.None, false)]
    public void CanUpdateKey(string featureKey, string role, Permissions permission, bool expected)
    {
        var tracker = new FeatureKeyChangeTracker(AllKeys);

        var res = tracker.TryUpdate(featureKey, role, permission);
        Assert.That(res, Is.EqualTo(expected));
    }

    [TestCase("FK_new", "role01", false)]
    [TestCase("fk02", "role02", true)]
    [TestCase("fk02", "role01", false)]
    public void CanDeleteKey(string featureKey, string role, bool expected)
    {
        var tracker = new FeatureKeyChangeTracker(AllKeys);

        var res = tracker.TryDelete(featureKey, role);
        Assert.That(res, Is.EqualTo(expected));
    }

    [Test]
    public void CanGetTrackedItemsAfterAdd()
    {
        var tracker = new FeatureKeyChangeTracker(AllKeys);

        tracker.TryAdd("FK_new", "role01", Permissions.Read); // true
        tracker.TryAdd("fk02", "role02", Permissions.Execute); // false
        tracker.TryAdd("fk02", "role01", Permissions.Write); // true

        var changed = tracker.GetChangedKeys();

        var fkNew = changed.FirstOrDefault(k => k.Name == "FK_new");
        Assert.That(fkNew, Is.Not.Null);

        var fk02 = changed.FirstOrDefault(k => k.Name == "fk02");
        Assert.That(fk02, Is.Not.Null);

        var fkNotChanged = changed.FirstOrDefault(k => k.Name == "fk01");
        Assert.That(fkNotChanged, Is.Null);

        var rp01 = fkNew.RolePermissions.FirstOrDefault(rp => rp.RoleName == "role01");
        Assert.That(rp01, Is.Not.Null);
        Assert.That(rp01.Permissions, Is.EqualTo(Permissions.Read));

        var rp02 = fk02.RolePermissions.FirstOrDefault(rp => rp.RoleName == "role02");
        Assert.That(rp02, Is.Not.Null);
        Assert.That(rp02.Permissions, Is.EqualTo(Permissions.Read | Permissions.Execute));

        var rp03 = fk02.RolePermissions.FirstOrDefault(rp => rp.RoleName == "role01");
        Assert.That(rp03, Is.Not.Null);
        Assert.That(rp03.Permissions, Is.EqualTo(Permissions.Write));

    }

    [Test]
    public void CanGetTrackedItemsAfterUpdate()
    {
        var tracker = new FeatureKeyChangeTracker(AllKeys);

        tracker.TryUpdate("FK_new", "role01", Permissions.Read); // false
        tracker.TryUpdate("fk02", "role02", Permissions.Execute); // true
        tracker.TryUpdate("fk02", "role01", Permissions.Write); // true

        var changed = tracker.GetChangedKeys();

        var fkNew = changed.FirstOrDefault(k => k.Name == "FK_new");
        Assert.That(fkNew, Is.Null);

        var fk02 = changed.FirstOrDefault(k => k.Name == "fk02");
        Assert.That(fk02, Is.Not.Null);

        var fkNotChanged = changed.FirstOrDefault(k => k.Name == "fk01");
        Assert.That(fkNotChanged, Is.Null);

        var rp02 = fk02.RolePermissions.FirstOrDefault(rp => rp.RoleName == "role02");
        Assert.That(rp02, Is.Not.Null);
        Assert.That(rp02.Permissions, Is.EqualTo(Permissions.Execute));

        var rp03 = fk02.RolePermissions.FirstOrDefault(rp => rp.RoleName == "role01");
        Assert.That(rp03, Is.Null);
    }

    [Test]
    public void CanGetTrackedItemsAfterAddAndUpdate()
    {
        var tracker = new FeatureKeyChangeTracker(AllKeys);

        tracker.TryAdd("FK_new", "role01", Permissions.Read); // true
        tracker.TryUpdate("FK_new", "role01", Permissions.Execute | Permissions.Write); // true

        var changed = tracker.GetChangedKeys();

        var fkNew = changed.FirstOrDefault(k => k.Name == "FK_new");
        var rp01 = fkNew!.RolePermissions.FirstOrDefault(rp => rp.RoleName == "role01");

        Assert.That(rp01, Is.Not.Null);
        Assert.That(rp01.Permissions, Is.EqualTo(Permissions.Write | Permissions.Execute));
    }

    [Test]
    public void CanGetTrackedItemsAfterUpdateAndAdd()
    {
        var tracker = new FeatureKeyChangeTracker(AllKeys);

        tracker.TryUpdate("FK_new", "role01", Permissions.Execute | Permissions.Write); // false
        tracker.TryAdd("FK_new", "role01", Permissions.Read); // true

        var changed = tracker.GetChangedKeys();

        var fkNew = changed.FirstOrDefault(k => k.Name == "FK_new");
        var rp01 = fkNew!.RolePermissions.FirstOrDefault(rp => rp.RoleName == "role01");

        Assert.That(rp01, Is.Not.Null);
        Assert.That(rp01.Permissions, Is.EqualTo(Permissions.Read));
    }

    [Test]
    public void CanGetTrackedItemsAfterAddAndDelete()
    {
        var tracker = new FeatureKeyChangeTracker(AllKeys);

        tracker.TryAdd("FK_new", "role01", Permissions.Execute | Permissions.Write); // true
        tracker.TryDelete("FK_new", "role01"); // true

        var changed = tracker.GetChangedKeys();
        var fkNew = changed.FirstOrDefault(k => k.Name == "FK_new");
        Assert.That(fkNew, Is.Null);

        var deleted = tracker.GetDeletedKeys();
        var fkNewDeleted = deleted.FirstOrDefault(k => k == "FK_new");
        Assert.That(fkNewDeleted, Is.Not.Null);
    }

    [Test]
    public void CanGetTrackedItemsAfterAddAndDelete2()
    {
        var tracker = new FeatureKeyChangeTracker(AllKeys);

        tracker.TryAdd("FK_new", "role01", Permissions.Execute | Permissions.Write); // true
        tracker.TryAdd("FK_new", "role02", Permissions.Read); // true
        tracker.TryDelete("FK_new", "role01"); // true

        var changed = tracker.GetChangedKeys();
        var fkNew = changed.FirstOrDefault(k => k.Name == "FK_new");
        Assert.That(fkNew, Is.Not.Null);

        var deleted = tracker.GetDeletedKeys();
        var fkNewDeleted = deleted.FirstOrDefault(k => k == "FK_new");
        Assert.That(fkNewDeleted, Is.Null);
    }

    [Test]
    public void CanGetTrackedItemsAfterAddAndDeleteAndAdd()
    {
        var tracker = new FeatureKeyChangeTracker(AllKeys);

        tracker.TryAdd("FK_new", "role01", Permissions.Execute | Permissions.Write); // true
        tracker.TryDelete("FK_new", "role01"); // true
        tracker.TryAdd("FK_new", "role02", Permissions.Read); // true

        var changed = tracker.GetChangedKeys();
        var fkNew = changed.FirstOrDefault(k => k.Name == "FK_new");
        Assert.That(fkNew, Is.Not.Null);

        var deleted = tracker.GetDeletedKeys();
        var fkNewDeleted = deleted.FirstOrDefault(k => k == "FK_new");
        Assert.That(fkNewDeleted, Is.Null);
    }

    [Test]
    public void CanGetTrackedItemsAfterUpdateAndDeleteAndAdd()
    {
        var tracker = new FeatureKeyChangeTracker(AllKeys);

        tracker.TryUpdate("fk02", "role02", Permissions.Execute); // true
        tracker.TryDelete("fk02", "role01"); // true
        tracker.TryDelete("fk02", "role02"); // true
        tracker.TryAdd("fk02", "role02", Permissions.Read); // true

        var changed = tracker.GetChangedKeys();
        var fk02 = changed.FirstOrDefault(k => k.Name == "fk02");
        Assert.That(fk02, Is.Not.Null);

        var rp02 = fk02.RolePermissions.FirstOrDefault(rp => rp.RoleName == "role02");
        Assert.That(rp02, Is.Not.Null);
        Assert.That(rp02.Permissions, Is.EqualTo(Permissions.Read));

        var deleted = tracker.GetDeletedKeys();
        var fkNewDeleted = deleted.FirstOrDefault(k => k == "fk02");
        Assert.That(fkNewDeleted, Is.Null);
    }
}
