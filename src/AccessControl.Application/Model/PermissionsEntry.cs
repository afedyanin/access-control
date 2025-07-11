using AccessControl.Application.Abstractions;

namespace AccessControl.Application.Model;
public class PermissionsEntry
{
    public required IResource Resource { get; set; }

    public required Role Role { get; set; }

    public Permissions Permissions { get; set; }
}
