using System.Linq;
using AccessControl.Model;

namespace AccessControl.DataAccess.Tests;

[TestFixture(Category = "Database", Explicit = true)]
public class UsersRepositoryTests : RepositoryTestBase
{
    [Test]
    public async Task CanSaveUserWithoutRoles()
    {
        var user = new User()
        {
            Name = $"TestUserWithoutRoles_{Guid.NewGuid()}",
        };

        var saved = await UsersRepository.Save(user);
        Assert.That(saved, Is.True);
    }

    [TestCase("Admin")]
    [TestCase("Admin,Developer")]
    [TestCase("Developer")]
    [TestCase("")]
    public async Task CanSaveUserWithRoles(string roleNamesString)
    {
        var user = new User()
        {
            Name = $"UserWithRoles",
        };

        var roleNames = roleNamesString.Split(',') ?? [];
        var roles = await RolesRepository.GetByNames(roleNames);
        var roleNamesToSave = roles.Select(x => x.Name).ToArray();
        Console.WriteLine($"Roles to save: {string.Join(',', roleNamesToSave)}");

        foreach (var role in roles)
        {
            user.Roles.Add(role);
        }

        _ = await UsersRepository.Save(user);

        var savedUser = await UsersRepository.GetByName(user.Name);
        Assert.That(savedUser, Is.Not.Null);

        var savedRoles = savedUser.Roles.Select(x => x.Name).ToArray();

        Console.WriteLine($"saved roles: {string.Join(',', savedRoles)}");

        foreach (var role in roleNames)
        {
            Assert.That(savedRoles, Does.Contain(role));
        }
    }


    [Test]
    public async Task CanGetUsers()
    {
        var users = await UsersRepository.GetAll();

        Assert.That(users, Is.Not.Empty);
        Console.WriteLine($"{string.Join(',', users.Select(u => u.Name))}");
    }
}
