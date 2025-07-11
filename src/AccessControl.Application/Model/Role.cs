namespace AccessControl.Application.Model;

public class Role
{
    public Guid Id { get; set; }

    public required string Name { get; set; }

    public string? Description { get; set; }
}
