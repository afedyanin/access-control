using AccessControl.Contracts.Entities;

namespace AccessControl.DataAccess.Dbos;

internal class FeatureKeyRoleDbo
{
    public string FeatureKeyName { get; set; } = string.Empty;

    public required FeatureKeyDbo FeatureKey { get; set; }

    public string RoleName { get; set; } = string.Empty;

    public required RoleDbo Role { get; set; }

    public Permissions Permissions { get; set; }
}
