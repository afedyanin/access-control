namespace AccessControl.Contracts.Entities;
public record class Role
{
    public required string Name { get; init; }

    public string? Description { get; init; }
}
