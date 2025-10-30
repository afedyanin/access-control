namespace AccessControl.Contracts.Entities;

public class FeatureKey
{
    public required string Name { get; init; }

    public RolePermissions[] RolePermissions { get; init; } = [];
}
