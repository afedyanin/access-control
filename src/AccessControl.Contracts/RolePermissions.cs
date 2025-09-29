namespace AccessControl.Contracts;
public record class RolePermissions
{
    public required string Role { get; set; }

    public Permissions Permissions { get; set; }
}
