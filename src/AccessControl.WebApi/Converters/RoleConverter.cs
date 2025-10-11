using AccessControl.Contracts;
using AccessControl.Model;

namespace AccessControl.WebApi.Converters;
internal static class RoleConverter
{
    public static RoleDto[] ToDto(this IEnumerable<Role> roles)
        => roles.Select(u => u.ToDto()).ToArray();

    public static RoleDto ToDto(this Role role)
        => new RoleDto
        {
            Name = role.Name,
            Description = role.Description,
        };

}
