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
}
