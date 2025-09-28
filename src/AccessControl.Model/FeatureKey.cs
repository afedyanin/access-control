namespace AccessControl.Model;

public class FeatureKey
{
    public Guid Id { get; set; }

    public required string Name { get; set; }

    public List<AccessRole> AccessRoles { get; set; } = [];

    public List<FeatureKeyAccessRole> FeatureKeyAccessRoles { get; set; } = [];
}
