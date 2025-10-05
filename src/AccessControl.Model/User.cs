namespace AccessControl.Model;
public class User
{
    public required string Name { get; set; }

    public string? Email { get; set; }

    public List<Role> Roles { get; set; } = [];

    public List<UserRole> UserRoles { get; set; } = [];
}
