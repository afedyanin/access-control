namespace AccessControl.Contracts;

public class UserDto
{
    public required string Name { get; set; }

    public string? Email { get; set; }

    public string[] Roles { get; set; } = [];
}
