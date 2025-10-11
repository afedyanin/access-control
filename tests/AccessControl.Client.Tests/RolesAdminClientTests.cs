using AccessControl.Contracts;
using Refit;

namespace AccessControl.Client.Tests;

[TestFixture(Category = "Web API", Explicit = true,
    Description = "Need to run Web API server before start tests. " +
    "Use dotnet run command inside AccessControl.Server project.")]
public class RolesAdminClientTests
{
    [Test]
    public async Task CanGetAllRoles()
    {
        var client = RestService.For<IAccessControlAdminClient>(ApiConsts.BaseUrl);
        var roles = await client.GetAllRoles();
        Assert.That(roles, Is.Not.Null);
    }
}
