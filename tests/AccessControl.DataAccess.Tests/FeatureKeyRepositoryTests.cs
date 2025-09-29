using AccessControl.Contracts;
using AccessControl.Model;

namespace AccessControl.DataAccess.Tests;

[TestFixture(Category = "Database", Explicit = true)]
public class FeatureKeyRepositoryTests : RepositoryTestBase
{
    [Test]
    public async Task CanSaveRole()
    {
        var fk = new FeatureKey()
        {
            Id = Guid.NewGuid(),
            Name = "Some FK",
        };

        var saved = await FeatureKeyRepository.Save(fk);
        Assert.That(saved, Is.True);
    }

    [Test]
    public async Task CanGetRole()
    {
        var fk = new FeatureKey()
        {
            Id = Guid.NewGuid(),
            Name = "Some FK 2",
        };

        var saved = await FeatureKeyRepository.Save(fk);
        var found = await FeatureKeyRepository.GetById(fk.Id);

        Assert.That(found, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(found.Name, Is.EqualTo(fk.Name));
        });
    }

    [Test]
    public async Task CanGetAllRoles()
    {
        var fkeys = await FeatureKeyRepository.GetAll();
        Assert.That(fkeys, Is.Not.Null);
    }

    [Test]
    public async Task CanDeleteRoles()
    {
        var fk = new FeatureKey()
        {
            Id = Guid.NewGuid(),
            Name = "Some FK to delete",
        };

        var saved = await FeatureKeyRepository.Save(fk);
        var deleted = await FeatureKeyRepository.Delete(fk.Id);

        Assert.That(deleted, Is.GreaterThan(0));

        var found = await FeatureKeyRepository.GetById(fk.Id);
        Assert.That(found, Is.Null);
    }

    [Test]
    public async Task CanAddRolesToFk()
    {
        var fk = new FeatureKey()
        {
            Id = Guid.NewGuid(),
            Name = "Some FK with roles",
        };

        _ = await FeatureKeyRepository.Save(fk);

        var role1 = new AccessRole()
        {
            Id = Guid.NewGuid(),
            Name = "Role01 FK",
        };
        _ = await AccessRoleRepository.Save(role1);

        var role2 = new AccessRole()
        {
            Id = Guid.NewGuid(),
            Name = "Role02 FK",
        };

        _ = await AccessRoleRepository.Save(role2);

        fk.AccessRoles.Add(role1);
        fk.AccessRoles.Add(role2);
        _ = await FeatureKeyRepository.Save(fk);


        Assert.Pass();
    }

    [Test]
    public async Task CanAddRolesToFkWithPermissions()
    {
        var fk = new FeatureKey()
        {
            Id = Guid.NewGuid(),
            Name = "Some FK with roles",
        };

        _ = await FeatureKeyRepository.Save(fk);

        var role1 = new AccessRole()
        {
            Id = Guid.NewGuid(),
            Name = "Role01 FK",
        };
        _ = await AccessRoleRepository.Save(role1);

        var role2 = new AccessRole()
        {
            Id = Guid.NewGuid(),
            Name = "Role02 FK",
        };

        _ = await AccessRoleRepository.Save(role2);

        var fkar1 = new FeatureKeyAccessRole
        {
            FeatureKey = fk,
            AccessRole = role1,
            Permissions = Permissions.Execute,
        };

        var fkar2 = new FeatureKeyAccessRole
        {
            FeatureKey = fk,
            AccessRole = role2,
            Permissions = Permissions.Full,
        };

        fk.FeatureKeyAccessRoles.Add(fkar1);
        fk.FeatureKeyAccessRoles.Add(fkar2);

        _ = await FeatureKeyRepository.Save(fk);

        Assert.Pass();
    }
}
