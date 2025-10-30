using AccessControl.Contracts.Entities;
using AccessControl.DataAccess.Dbos;

namespace AccessControl.DataAccess.Converters;

internal static class UserConverter
{
    public static User[] ToEntity(this IEnumerable<UserDbo> users)
        => [.. users.Select(u => u.ToEntity())];

    public static User ToEntity(this UserDbo user)
        => new()
        {
            Name = user.Name,
            Email = user.Email,
            Roles = [.. user.Roles.Select(r => r.Name)],
        };
}
