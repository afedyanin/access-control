using AccessControl.Contracts;

namespace AccessControl.Model;

public class FeatureKeyAccessRole
{
    public Guid FeatureKeyId { get; set; }

    public FeatureKey FeatureKey { get; set; }

    public Guid AccessRoleId { get; set; }

    public AccessRole AccessRole { get; set; }

    public Permissions Permissions { get; set; }
}
