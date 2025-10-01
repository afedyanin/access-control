using AccessControl.Contracts;

namespace AccessControl.Model;

public class FeatureKeyRole
{
    public string FeatureKeyName { get; set; } = string.Empty;

    public string RoleName { get; set; } = string.Empty;

    public Permissions Permissions { get; set; }
}
