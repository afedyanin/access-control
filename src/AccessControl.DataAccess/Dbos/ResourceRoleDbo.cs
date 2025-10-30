using AccessControl.Contracts.Entities;

namespace AccessControl.DataAccess.Dbos;

internal class ResourceRoleDbo
{
    public Guid ResourceId { get; set; }

    public required ResourceDbo Resource { get; set; }

    public string RoleName { get; set; } = string.Empty;

    public required RoleDbo Role { get; set; }

    public Permissions Permissions { get; set; }
}
