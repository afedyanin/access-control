namespace AccessControl.Contracts;

public class FeatureKeyDto
{
    public required string Name { get; init; }

    public RolePermissions[] RolePermissions { get; init; } = [];
}
