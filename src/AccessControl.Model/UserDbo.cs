namespace AccessControl.Model;
public class UserDbo
{
    public required string Name { get; set; }

    public string? Email { get; set; }

    public string[] Roles { get; set; } = [];
}
