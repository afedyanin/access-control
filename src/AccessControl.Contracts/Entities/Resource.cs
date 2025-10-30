namespace AccessControl.Contracts.Entities;
public class Resource
{
    public Guid Id { get; init; }

    public required string Name { get; init; }

    public RolePermissions[] RolePermissions { get; init; } = [];
}
