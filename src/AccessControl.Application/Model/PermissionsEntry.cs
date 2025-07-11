namespace AccessControl.Application.Model;
public class PermissionsEntry
{
    public required FeatureKey FeatureKey { get; set; }

    public required Role Role { get; set; }

    public Permissions Permissions { get; set; }
}
