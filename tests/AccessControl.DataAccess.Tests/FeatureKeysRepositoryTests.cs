using AccessControl.Contracts.Entities;

namespace AccessControl.DataAccess.Tests;

[TestFixture(Category = "Database", Explicit = true)]
internal class FeatureKeysRepositoryTests : RepositoryTestBase
{
    [Test]
    public async Task CanSaveRole()
    {
        var fk = new FeatureKey()
        {
            Name = "Some FK",
        };

        var saved = await FeatureKeysRepository.Save(fk);
        Assert.That(saved, Is.True);
    }

    [Test]
    public async Task CanGetRole()
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
    public async Task CanGetAllRoles()
    {
        var fkeys = await FeatureKeysRepository.GetAll();
        Assert.That(fkeys, Is.Not.Null);
    }

    [Test]
    public async Task CanDeleteRoles()
    {
        var fk = new FeatureKey()
        {
            Name = "Some FK to delete",
        };

        var saved = await FeatureKeysRepository.Save(fk);
        var deleted = await FeatureKeysRepository.Delete(fk.Name);

        Assert.That(deleted, Is.GreaterThan(0));

        var found = await FeatureKeysRepository.GetByName(fk.Name);
        Assert.That(found, Is.Null);
    }
    /*
    [Test]
    public async Task CanAddRolesToFk()
    {
        var fk = new FeatureKey()
        {
            Name = $"Some FK with roles {Guid.NewGuid()}",
        };

        _ = await FeatureKeysRepository.Save(fk);

        var role1 = new Role()
        {
            Name = $"Role01 FK {Guid.NewGuid()}",
        };
        _ = await RolesRepository.Save(role1);

        var role2 = new Role()
        {
            Name = $"Role02 FK {Guid.NewGuid()}",
        };

        _ = await RolesRepository.Save(role2);

        fk.Roles.Add(role1);
        fk.Roles.Add(role2);
        _ = await FeatureKeysRepository.Save(fk);


        Assert.Pass();
    }
    */
    /*
    [Test]
    public async Task CanAddRolesToFkWithPermissions()
    {
        var fk = new FeatureKey()
        {
            Name = $"Some FK with roles {Guid.NewGuid()}",
        };

        _ = await FeatureKeysRepository.Save(fk);

        var role1 = new Role()
        {
            Name = $"Role01 FK {Guid.NewGuid()}",
        };
        _ = await RolesRepository.Save(role1);

        var role2 = new Role()
        {
            Name = $"Role02 FK {Guid.NewGuid()}",
        };

        _ = await RolesRepository.Save(role2);

        var fkar1 = new FeatureKeyRole
        {
            FeatureKey = fk,
            Role = role1,
            Permissions = Permissions.Execute,
        };

        var fkar2 = new FeatureKeyRole
        {
            FeatureKey = fk,
            Role = role2,
            Permissions = Permissions.Full,
        };

        fk.FeatureKeyRoles.Add(fkar1);
        fk.FeatureKeyRoles.Add(fkar2);

        _ = await FeatureKeysRepository.Save(fk);

        Assert.Pass();
    }
    */
}
