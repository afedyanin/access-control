namespace AccessControl.Contracts.Requests;
public class PermissionsRequest
{
    public RolePermissions[] Permissions { get; set; } = [];
}
