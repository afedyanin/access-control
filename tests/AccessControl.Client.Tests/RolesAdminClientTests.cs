namespace AccessControl.Client.Tests;

[TestFixture(Category = "Web API", Explicit = true,
    Description = "Need to run Web API Server before start tests.")]
public class RolesAdminClientTests : ApiClientBase
{
    [Test]
    public async Task CanGetAllRoles()
    {
        var roles = await AdminClient.GetAllRoles();
        Assert.That(roles, Is.Not.Null);
    }
}
