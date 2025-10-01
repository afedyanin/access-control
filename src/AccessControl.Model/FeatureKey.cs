namespace AccessControl.Model;

public class FeatureKey
{
    public required string Name { get; set; }

    public List<Role> Roles { get; set; } = [];

    public List<FeatureKeyRole> FeatureKeyRoles { get; set; } = [];
}
