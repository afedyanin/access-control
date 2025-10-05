using AccessControl.Contracts;

namespace AccessControl.Model;

public class ResourceRole
{
    public Guid ResourceId { get; set; }

    public required Resource Resource { get; set; }

    public string RoleName { get; set; } = string.Empty;

    public required Role Role { get; set; }

    public Permissions Permissions { get; set; }
}
