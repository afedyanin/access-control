using AccessControl.Contracts.Entities;

namespace AccessControl.DataAccess.Tests;

[TestFixture(Category = "Database", Explicit = true)]
internal sealed class RolesRepositoryTests : RepositoryTestBase
{
    [TestCase("Admin")]
    [TestCase("Developer")]
    public async Task CanAddRole(string roleName)
    {
        var role = new Role()
        {
            Name = roleName
        };

        var saved = await RolesRepository.Save(role);
        Assert.That(saved, Is.True);
    }

    [Test]
    public async Task CanGetRole()
    {
        var role = new Role()
        {
            Name = "Some Role 2",
        };

        var saved = await RolesRepository.Save(role);
        var found = await RolesRepository.GetByName(role.Name);

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
        var roles = await RolesRepository.GetAll();
        Assert.That(roles, Is.Not.Null);
    }

    [Test]
    public async Task CanDeleteRoles()
    {
        var role = new Role()
        {
            Name = "Some Role to delete",
        };

        _ = await RolesRepository.Save(role);
        var deleted = await RolesRepository.Delete(role.Name);

        Assert.That(deleted, Is.GreaterThan(0));

        var found = await RolesRepository.GetByName(role.Name);
        Assert.That(found, Is.Null);
    }
}
