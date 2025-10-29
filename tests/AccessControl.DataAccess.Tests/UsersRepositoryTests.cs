using AccessControl.Model;

namespace AccessControl.DataAccess.Tests;

[TestFixture(Category = "Database", Explicit = true)]
public class UsersRepositoryTests : RepositoryTestBase
{
    [Test]
    public async Task CanSaveUserWithoutRoles()
    {
        var user = new UserDbo()
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

        var user = new UserDbo()
        {
            Name = $"UserWithRoles",
            Roles = roleNames
        };

        _ = await UsersRepository.Save(user);

        var savedUser = await UsersRepository.GetByName(user.Name);
        Assert.That(savedUser, Is.Not.Null);

        var savedRoles = savedUser.Roles.Select(x => x.Name).ToArray();

        Console.WriteLine($"saved roles: {string.Join(',', savedRoles)}");

        foreach (var role in roleNames)
        {
            if (string.IsNullOrEmpty(role))
            {
                continue;
            }
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
