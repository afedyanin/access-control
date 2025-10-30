using AccessControl.Contracts.Entities;
using AccessControl.DataAccess.Dbos;

namespace AccessControl.DataAccess.Converters;

internal static class FeatureKeyConverter
{
    public static FeatureKey[] ToEntity(this IEnumerable<FeatureKeyDbo> featureKeys)
        => [.. featureKeys.Select(fk => fk.ToEntity())];

    public static FeatureKey ToEntity(this FeatureKeyDbo featureKey)
        => new()
        {
            Name = featureKey.Name,
            RolePermissions = featureKey.FeatureKeyRoles.ToEntity()
        };

    private static RolePermissions[] ToEntity(this IEnumerable<FeatureKeyRoleDbo> fkRoles)
        => [.. fkRoles.Select(fkr => fkr.ToEntity())];

    private static RolePermissions ToEntity(this FeatureKeyRoleDbo fkRole)
        => new()
        {
            RoleName = fkRole.RoleName,
            Permissions = fkRole.Permissions,
        };
}
