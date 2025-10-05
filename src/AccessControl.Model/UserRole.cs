namespace AccessControl.Model;

public class UserRole
{
    public string UserName { get; set; } = string.Empty;

    public required User User { get; set; }

    public string RoleName { get; set; } = string.Empty;

    public required Role Role { get; set; }

}
