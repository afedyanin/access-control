using AccessControl.AdminUI.Models;
using AccessControl.Contracts.Entities;

namespace AccessControl.AdminUI.Tests;

public class ModelPermissionsTests
{
    [Test]
    public void CanCreatePermissionsFromModel()
    {
        var model = new FeatureKeyRolePermissionsModel
        {
            FeatureKey = "FK01",
            RoleName = "Role",
            PermissionExecute = true
        };

        var res = model.GetPermissions();

        Assert.That(res, Is.EqualTo(Permissions.Execute));
    }

    [Test]
    public void CanSetPermissions()
    {
        var p = Permissions.None;
        p |= Permissions.Execute;
        Assert.That(p, Is.EqualTo(Permissions.Execute));
    }
}
