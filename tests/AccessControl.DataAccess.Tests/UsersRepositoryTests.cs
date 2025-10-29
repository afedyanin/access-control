using AccessControl.Model;

namespace AccessControl.DataAccess.Tests;

[TestFixture(Category = "Database", Explicit = true)]
public class UsersRepositoryTests : RepositoryTestBase
{
    [Test]
    public async Task CanSaveRole()
    {
        var user = new User()
        {
            Name = "TestUser",
        };

        var saved = await UsersRepository.Save(user);
        Assert.That(saved, Is.True);
    }

}
