namespace AccessControl.Contracts.Entities;

public record class FeatureKey
{
    public required string Name { get; init; }

    public RolePermissions[] RolePermissions { get; init; } = [];
}
