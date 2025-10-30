namespace AccessControl.DataAccess.Dbos;

internal class ResourceDbo
{
    public Guid Id { get; set; }

    public required string Name { get; set; }

    public List<RoleDbo> Roles { get; set; } = [];

    public List<ResourceRoleDbo> ResourceRoles { get; set; } = [];
}
