namespace AccessControl.Contracts;
public class ResourceDto
{
    public Guid Id { get; init; }

    public required string Name { get; init; }

    public RolePermissions[] RolePermissions { get; init; } = [];
}
