using AccessControl.Contracts.Entities;

namespace AccessControl.DataAccess.Tests;

[TestFixture(Category = "Database", Explicit = true)]
internal sealed class UsersRepositoryTests : RepositoryTestBase
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
        var roleNames = roleNamesString.Split(',') ?? [];

        var user = new User()
        {
            Name = $"UserWithRoles",
            Roles = roleNames
        };

        _ = await UsersRepository.Save(user);

        var savedUser = await UsersRepository.GetByName(user.Name);
        Assert.That(savedUser, Is.Not.Null);

        Console.WriteLine($"saved roles: {string.Join(',', savedUser.Roles)}");

        foreach (var role in roleNames)
        {
            if (string.IsNullOrEmpty(role))
            {
                continue;
            }

            Assert.That(savedUser.Roles, Does.Contain(role));
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
