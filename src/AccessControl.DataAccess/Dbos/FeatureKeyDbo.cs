namespace AccessControl.DataAccess.Dbos;

internal class FeatureKeyDbo
{
    public required string Name { get; set; }

    public List<RoleDbo> Roles { get; set; } = [];

    public List<FeatureKeyRoleDbo> FeatureKeyRoles { get; set; } = [];
}
