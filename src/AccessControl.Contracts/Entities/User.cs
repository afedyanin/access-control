namespace AccessControl.Contracts.Entities;

public record class User
{
    public required string Name { get; init; }

    public string? Email { get; init; }

    public string[] Roles { get; init; } = [];
}
