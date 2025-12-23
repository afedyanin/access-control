using AccessControl.Contracts.Entities;

namespace AccessControl.DataAccess.Tests;

[TestFixture(Category = "Database", Explicit = true)]
internal sealed class FeatureKeysRepositoryTests : RepositoryTestBase
{
    [Test]
    public async Task CanSaveFeatureKeyWithoutPermissions()
    {
        var fk = new FeatureKey()
        {
            Name = "Some FK",
        };

        var saved = await FeatureKeysRepository.Save(fk);
        Assert.That(saved, Is.True);
    }

    [Test]
    public async Task CanGetFeatureKey()
    {
        var fk = new FeatureKey()
        {
            Name = "Some FK 2",
        };

        var saved = await FeatureKeysRepository.Save(fk);
        var found = await FeatureKeysRepository.GetByName(fk.Name);

        Assert.That(found, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(found.Name, Is.EqualTo(fk.Name));
        });
    }

    [Test]
    public async Task CanGetAllFeatureKeys()
    {
        var fkeys = await FeatureKeysRepository.GetAll();
        Assert.That(fkeys, Is.Not.Null);
    }

    [Test]
    public async Task CanDeleteFeatureKey()
    {
        var fk = new FeatureKey()
        {
            Name = "Some FK to delete",
        };

        _ = await FeatureKeysRepository.Save(fk);
        var deleted = await FeatureKeysRepository.Delete(fk.Name);

        Assert.That(deleted, Is.GreaterThan(0));

        var found = await FeatureKeysRepository.GetByName(fk.Name);
        Assert.That(found, Is.Null);
    }

    [TestCase("Admin", Permissions.Full)]
    [TestCase("Developer", Permissions.ReadWrite)]
    public async Task CanSaveFeatureKeyWithPermissions(string roleName, Permissions permissions)
    {
        var rp = new RolePermissions
        {
            RoleName = roleName,
            Permissions = permissions
        };

        var fk = new FeatureKey()
        {
            Name = $"FK with permissions",
            RolePermissions = [rp],
        };

        _ = await FeatureKeysRepository.Save(fk);

        var savedFk = await FeatureKeysRepository.GetByName(fk.Name);

        Assert.That(savedFk, Is.Not.Null);

        Assert.Multiple(() =>
        {
            Assert.That(savedFk.RolePermissions.Any(perm => perm.RoleName == rp.RoleName), Is.True);
            Assert.That(savedFk.RolePermissions.Single(perm => perm.RoleName == rp.RoleName).Permissions, Is.EqualTo(rp.Permissions));
        });
    }
}
