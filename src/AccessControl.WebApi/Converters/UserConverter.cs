using AccessControl.Contracts;
using AccessControl.Model;

namespace AccessControl.WebApi.Converters;
internal static class UserConverter
{
    public static UserDto[] ToDto(this IEnumerable<User> users)
        => users.Select(u => u.ToDto()).ToArray();

    public static UserDto ToDto(this User user)
        => new UserDto
        {
            Name = user.Name,
            Email = user.Email,
            Roles = user.Roles.Select(r => r.Name).ToArray(),
        };

    public static UserDbo FromDto(this UserDto user)
        => new UserDbo
        {
            Name = user.Name,
            Email = user.Email,
            Roles = user.Roles,
        };
}
