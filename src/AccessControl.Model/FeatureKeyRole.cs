using AccessControl.Contracts;

namespace AccessControl.Model;

public class FeatureKeyRole
{
    public string FeatureKeyName { get; set; } = string.Empty;

    public required FeatureKey FeatureKey { get; set; }

    public string RoleName { get; set; } = string.Empty;

    public required Role Role { get; set; }

    public Permissions Permissions { get; set; }
}
