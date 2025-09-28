using AccessControl.Model;

namespace AccessControl.DataAccess.Tests;

[TestFixture(Category = "Database", Explicit = true)]
public class AccessRolesRepositoryTests : RepositoryTestBase
{
    [Test]
    public async Task CanSaveRole()
    {
        var role = new AccessRole()
        {
            Id = Guid.NewGuid(),
            Name = "Some Role",
            Description = "Test description"
        };

        var saved = await AccessRolesRepo.Save(role);
        Assert.That(saved, Is.True);
    }

    [Test]
    public async Task CanGetRole()
    {
        var role = new AccessRole()
        {
            Id = Guid.NewGuid(),
            Name = "Some Role 2",
        };

        var saved = await AccessRolesRepo.Save(role);
        var found = await AccessRolesRepo.GetById(role.Id);

        Assert.That(found, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(found.Name, Is.EqualTo(role.Name));
            Assert.That(found.Description, Is.EqualTo(role.Description));
        });
    }

    [Test]
    public async Task CanGetAllRoles()
    {
        var roles = await AccessRolesRepo.GetAll();
        Assert.That(roles, Is.Not.Null);
    }

    [Test]
    public async Task CanDeleteRoles()
    {
        var role = new AccessRole()
        {
            Id = Guid.NewGuid(),
            Name = "Some Role to delete",
        };

        var saved = await AccessRolesRepo.Save(role);
        var deleted = await AccessRolesRepo.Delete(role.Id);

        Assert.That(deleted, Is.GreaterThan(0));

        var found = await AccessRolesRepo.GetById(role.Id);
        Assert.That(found, Is.Null);
    }
}
