using AccessControl.Contracts.Entities;

namespace AccessControl.AdminUI.Models;

public record class FeatureKeyRolePermissionsModel
{
    public required string FeatureKey { get; set; }

    public required string RoleName { get; set; }

    public bool PermissionRead { get; set; }

    public bool PermissionWrite { get; set; }

    public bool PermissionDelete { get; set; }

    public bool PermissionExecute { get; set; }

    public List<string> AllFeatureKeys { get; set; } = [];

    public string[] AllRoles { get; set; } = [];

    public IEnumerable<string>? SelectedRoles { get; set; } = [];

    internal Permissions GetPermissions()
    {
        var perm = Permissions.None;

        if (PermissionRead)
        {
            perm |= Permissions.Read;
        }

        if (PermissionWrite)
        {
            perm |= Permissions.Write;
        }

        if (PermissionExecute)
        {
            perm |= Permissions.Execute;
        }

        if (PermissionDelete)
        {
            perm |= Permissions.Delete;
        }

        return perm;
    }
}
