using AccessControl.Contracts;
using AccessControl.Model;

namespace AccessControl.WebApi.Converters;
internal static class FeatureKeyConverter
{
    public static FeatureKeyDto[] ToDto(this IEnumerable<FeatureKey> featureKeys)
        => featureKeys.Select(fk => fk.ToDto()).ToArray();

    public static FeatureKeyDto ToDto(this FeatureKey featureKey)
        => new FeatureKeyDto
        {
            Name = featureKey.Name,
            RolePermissions = featureKey.FeatureKeyRoles.ToDto()
        };

    private static RolePermissions[] ToDto(this IEnumerable<FeatureKeyRole> fkRoles)
        => fkRoles.Select(fkr => fkr.ToDto()).ToArray();

    private static RolePermissions ToDto(this FeatureKeyRole fkRole)
        => new RolePermissions
        {
            RoleName = fkRole.RoleName,
            Permissions = fkRole.Permissions,
        };

}
