namespace AccessControl.Model;

public class AccessRole
{
    public Guid Id { get; set; }

    public required string Name { get; set; }

    public string? Description { get; set; }
}
