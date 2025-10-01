namespace AccessControl.Contracts;
public record class RolePermissions
{
    public required string RoleName { get; init; }

    public Permissions Permissions { get; init; }
}
